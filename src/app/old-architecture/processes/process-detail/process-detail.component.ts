import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { Candidate } from '@shared/models/candidate.model';
import { Process } from '@shared/models/process.model';
import { Stage } from '@shared/models/stage.model';
import { User } from '@shared/models/user.model';
import { CandidateDetailsComponent } from '@old-architecture/candidates/details/candidate-details.component';
import { UserDetailsComponent } from '@old-architecture/users/details/user-details.component';
import { FacadeService } from '@shared/services/facade.service';

@Component({
  selector: 'app-process-detail',
  templateUrl: './process-detail.component.html',
  styleUrls: ['./process-detail.component.scss'],
  providers: [CandidateDetailsComponent, UserDetailsComponent]
})
export class ProcessDetailComponent implements OnInit {

  constructor(private route: ActivatedRoute, private facade: FacadeService, private formBuilder: FormBuilder,
    private userDetailsModal: UserDetailsComponent, private candidateDetailsModal: CandidateDetailsComponent) { }

  @ViewChild('dropdown') nameDropdown;

  stageForm: FormGroup;
  process: Process = null;

  states = ['error', 'finish', 'process', 'wait', 'notStarted'];

  statusList: any[] = [
    { id: 0, name: 'Error' }, { id: 1, name: 'Finish' },
    { id: 2, name: 'Process' }, { id: 3, name: 'Wait' }, { id: 4, name: 'NotStarted' }
  ];

  candidates: Candidate[] = [];
  users: User[] = [];
  processID: number = this.route.snapshot.params['id'];

  isEdit: boolean = false;

  searchValue = '';
  listOfSearchStages = [];
  sortName = null;
  sortValue = null;

  isDetailsVisible: boolean = false;
  isAddVisible: boolean = false;

  emptyStage: Stage = null;

  filteredStages: Stage[] = []
  listOfDisplayData = [...this.filteredStages];

  emptyCandidate: Candidate;
  emptyUser: User;

  userOwner: User;
  userDelegate: User;

  feedbackContent:string = "";

  dropStage(event: CdkDragDrop<string[]>) {
    console.log("Drop method in table");
    console.log(event);
    moveItemInArray(this.listOfDisplayData, event.previousIndex, event.currentIndex);
  }

  ngOnInit() {
    this.getProcessByID(this.processID);
    this.getUsers();
    this.getCandidates();

    this.stageForm = this.formBuilder.group({
      title: [null, [Validators.required]],
      startDate: [null, [Validators.required]],
      endDate: [null, [Validators.required]],
      description: [null, [Validators.required]],
      feedback: [null, [Validators.required]],
      status: [null, [Validators.required]],
      userOwnerId: [null, [Validators.required]],
      userDelegateId: [null, [Validators.required]]
    });
  }

  getFeedbackContent(content: string): void {
    this.feedbackContent = content;
  }

  getCandidates() {
    this.facade.candidateService.get()
      .subscribe(res => {
        this.candidates = res;
      }, err => {
        console.log(err);
      })
  }

  getProcessByID(id) {
    this.facade.processService.getByID(id)
      .subscribe(res => {
        this.process = res;
      }, err => {
        console.log(err);
      })
  }

  getUsers() {
    this.facade.userService.get()
      .subscribe(res => {
        this.users = res.sort((a,b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
      }, err => {
        console.log(err);
      });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {

    if (this.process && this.process.status) {
      if (this.process.status === ProcessStatusEnum.Hired ||
        this.process.status === ProcessStatusEnum.Declined ||
        this.process.status === ProcessStatusEnum.Rejected) {
          const message = 'You cannot add new stages to a finished process. You can re open the process by changing its status';
          const title = 'Process finished';
          this.facade.toastrService.error(message, title);
        return;
      }
    }

    // Add New Skill Modal
    this.isEdit = false;
    this.stageForm.reset();

    if (this.users.length > 0) {
      this.stageForm.controls['userOwnerId'].setValue(this.users[0].id);
      this.stageForm.controls['userDelegateId'].setValue(this.users[0].id);
    }

    this.stageForm.controls['status'].setValue('3');

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--lg',
      nzTitle: 'Add new stage',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        {
          label: 'Cancel',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            let isCompleted = true;
            for (const i in this.stageForm.controls) {
              if (this.stageForm.controls.hasOwnProperty(i)) {
                this.stageForm.controls[i].markAsDirty();
                this.stageForm.controls[i].updateValueAndValidity();
                if (!this.stageForm.controls[i].valid && i !== 'startDate' && i !== 'endDate') {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {
              const newStage: Stage = {
                id: 0,
                date: new Date,
                feedback: this.feedbackContent,
                status: this.stageForm.controls['status'].value.toString(),
                userOwnerId: this.stageForm.controls['userOwnerId'].value.toString(),
                userDelegateId: this.stageForm.controls['userDelegateId'].value.toString(),
                processId: this.processID
              };

              this.facade.stageService.add(newStage)
                .subscribe(res => {
                  this.getProcessByID(this.processID);
                  modal.destroy();
                }, err => {
                  this.facade.toastrService.error(err.message);
                })
            }
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    //Edit Skill Modal
    this.isEdit = true;
    this.stageForm.reset();
  }

  showDeleteConfirm(stageID: number): void {
    let stageText = '';
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete the stage called ' + stageText + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.stageService.delete(stageID)
        .subscribe(res => {
          this.getProcessByID(this.processID);
          this.facade.toastrService.success('Stage was deleted !')
        }, err => {
          this.facade.toastrService.error(err.message);
        })
    });
  }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyStage = {
      id: 0,
      processId: 0,
      date: new Date,
      feedback: '',
      status: 0,
      userOwnerId: 0,
      userDelegateId: 0
    };
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchStages.length ? this.listOfSearchStages.some(p => item.title.indexOf(p) !== -1) : true) &&
        (item.title.toString().toUpperCase().indexOf(this.searchValue.toUpperCase()) !== -1)
    };
    const data = this.filteredStages.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => {
      if (this.sortValue === 'ascend') {
        return a[this.sortName] > b[this.sortName] ? 1 : -1;
      } else {
        return b[this.sortName] > a[this.sortName] ? 1 : -1;
      }
    });
    this.searchValue = '';
    this.nameDropdown.nzVisible = false;
  }

  sort(sortName: string, value: boolean): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  showCandidateDetailsModal(candidateID: number, modalContent: TemplateRef<{}>): void {
    this.emptyCandidate = this.candidates.filter(candidate => candidate.id === candidateID)[0];
    this.candidateDetailsModal.showModal(modalContent, this.emptyCandidate.name + ' ' + this.emptyCandidate.lastName);
  }

  showUserDetailsModal(userID: number, modalContent: TemplateRef<{}>): void {
    this.emptyUser = this.users.filter(user => user.id === userID)[0];
    this.userDetailsModal.showModal(modalContent, this.emptyUser.firstName + ' ' + this.emptyUser.lastName);
  }
}
