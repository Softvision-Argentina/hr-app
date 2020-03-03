import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Process } from 'src/entities/process';
import { FacadeService } from 'src/app/services/facade.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Candidate } from 'src/entities/candidate';
import { Consultant } from 'src/entities/consultant';
import { Stage } from 'src/entities/stage';
import { CandidateDetailsComponent } from 'src/app/candidates/details/candidate-details.component';
import { ConsultantDetailsComponent } from 'src/app/consultants/details/consultant-details.component';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';

@Component({
  selector: 'app-process-detail',
  templateUrl: './process-detail.component.html',
  styleUrls: ['./process-detail.component.css'],
  providers: [CandidateDetailsComponent, ConsultantDetailsComponent]
})
export class ProcessDetailComponent implements OnInit {

  constructor(private route: ActivatedRoute, private facade: FacadeService, private formBuilder: FormBuilder,
    private consultantDetailsModal: ConsultantDetailsComponent, private candidateDetailsModal: CandidateDetailsComponent) { }

  @ViewChild('dropdown') nameDropdown;

  stageForm: FormGroup;
  process: Process = null;

  states = ['error', 'finish', 'process', 'wait', 'notStarted'];

  statusList: any[] = [
    { id: 0, name: 'Error' }, { id: 1, name: 'Finish' },
    { id: 2, name: 'Process' }, { id: 3, name: 'Wait' }, { id: 4, name: 'NotStarted' }
  ];

  candidates: Candidate[] = [];
  consultants: Consultant[] = [];
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
  emptyConsultant: Consultant;

  consultantOwner: Consultant;
  consultantDelegate: Consultant;

  dropStage(event: CdkDragDrop<string[]>) {
    console.log("Drop method in table");
    console.log(event);
    moveItemInArray(this.listOfDisplayData, event.previousIndex, event.currentIndex);
  }

  ngOnInit() {
    this.getProcessByID(this.processID);
    this.getConsultants();
    this.getCandidates();

    this.stageForm = this.formBuilder.group({
      title: [null, [Validators.required]],
      startDate: [null, [Validators.required]],
      endDate: [null, [Validators.required]],
      description: [null, [Validators.required]],
      feedback: [null, [Validators.required]],
      status: [null, [Validators.required]],
      consultantOwnerId: [null, [Validators.required]],
      consultantDelegateId: [null, [Validators.required]]
    });
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
        //this.filteredStages = this.process.stages;
        //this.listOfDisplayData = this.process.stages;
        //console.log('Proceso:' + this.process);
        //this.checkStatusOfProcess();
        console.log(res);
      }, err => {
        console.log(err);
      })
  }

  getConsultants() {
    this.facade.consultantService.get()
      .subscribe(res => {
        this.consultants = res;
      }, err => {
        console.log(err);
      });
  }

  showAddModal(modalContent: TemplateRef<{}>): void {

    if (this.process && this.process.status && (this.process.status === ProcessStatusEnum.Hired || this.process.status === ProcessStatusEnum.Declined || this.process.status === ProcessStatusEnum.Rejected)) {
      this.facade.toastrService.error('You cannot add new stages to a finished process. You can re open the process by changing its status', 'Process finished');
      return;
    }

    //Add New Skill Modal
    this.isEdit = false;
    this.stageForm.reset();

    if (this.consultants.length > 0) {
      this.stageForm.controls['consultantOwnerId'].setValue(this.consultants[0].id);
      this.stageForm.controls['consultantDelegateId'].setValue(this.consultants[0].id);
    }

    this.stageForm.controls['status'].setValue('3');

    const modal = this.facade.modalService.create({
      nzTitle: 'Add new stage',
      nzContent: modalContent,
      nzClosable: true,
      nzWidth: '90%',
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            modal.nzFooter[1].loading = true;
            let isCompleted: boolean = true;
            for (const i in this.stageForm.controls) {
              this.stageForm.controls[i].markAsDirty();
              this.stageForm.controls[i].updateValueAndValidity();
              if ((!this.stageForm.controls[i].valid) && i != 'startDate' && i != 'endDate') isCompleted = false;
            }
            if (isCompleted) {
              let newStage: Stage = {
                id: 0,
                date: new Date,
                // title: chk['label'],
                // startDate: null,
                // endDate: null,
                // description: that.getStageDescription(chk['value']),
                feedback: this.stageForm.controls['feedback'].value.toString(),
                status: this.stageForm.controls['status'].value.toString(),
                consultantOwnerId: this.stageForm.controls['consultantOwnerId'].value.toString(),
                consultantDelegateId: this.stageForm.controls['consultantDelegateId'].value.toString(),
                processId: this.processID
              }
              this.facade.stageService.add(newStage)
                .subscribe(res => {
                  this.getProcessByID(this.processID);
                  modal.destroy();
                }, err => {
                  modal.nzFooter[1].loading = false;
                  this.facade.toastrService.error(err.message);
                })
            }
            else modal.nzFooter[1].loading = false;
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    //Edit Skill Modal
    this.isEdit = true;
    this.stageForm.reset();

    //let editedStage: Stage = this.process.stages.filter(p => p.id == id)[0];
    // let statusIndex = this.states.indexOf(editedStage.status.toLowerCase());
    // this.stageForm.controls['title'].setValue(editedStage.title);
    // this.stageForm.controls['startDate'].setValue(editedStage.startDate);
    // this.stageForm.controls['endDate'].setValue(editedStage.endDate);
    // this.stageForm.controls['description'].setValue(editedStage.description);
    // this.stageForm.controls['feedback'].setValue(editedStage.feedback);
    // this.stageForm.controls['status'].setValue(editedStage.status);
    // this.stageForm.controls['consultantOwnerId'].setValue(editedStage.consultantOwnerId);
    // this.stageForm.controls['consultantDelegateId'].setValue(editedStage.consultantDelegateId);

    // const modal = this.facade.modalService.create({
    //   nzTitle: 'Edit Stage',
    //   nzContent: modalContent,
    //   nzClosable: true,
    //   nzWrapClassName: 'vertical-center-modal',
    //   nzWidth: '90%',
    //   nzFooter: [
    //     {
    //       label: 'Cancel',
    //       shape: 'default',
    //       onClick: () => modal.destroy()
    //     },
    //     {
    //       label: 'Save',
    //       type: 'primary',
    //       loading: false,
    //       onClick: () => {
    //         modal.nzFooter[1].loading = true;
    //         let isCompleted: boolean = true;
    //         for (const i in this.stageForm.controls) {
    //           this.stageForm.controls[i].markAsDirty();
    //           this.stageForm.controls[i].updateValueAndValidity();
    //           if ((!this.stageForm.controls[i].valid) && i != 'startDate' && i != 'endDate') isCompleted = false;
    //         }

    //         if (isCompleted) {
    //           editedStage = {
    //             id: editedStage.id,
    //             date: new Date,
    //             // title: chk['label'],
    //             // startDate: null,
    //             // endDate: null,
    //             // description: that.getStageDescription(chk['value']),
    //             feedback: this.stageForm.controls['feedback'].value.toString(),
    //             status: this.stageForm.controls['status'].value.toString(),
    //             processId: this.processID,
    //             consultantOwnerId: this.stageForm.controls['consultantOwnerId'].value.toString(),
    //             consultantDelegateId: this.stageForm.controls['consultantDelegateId'].value.toString()
    //           }
    //           this.facade.stageService.update(id, editedStage)
    //             .subscribe(res => {
    //               this.getProcessByID(this.processID);
    //               this.facade.toastrService.success('Stage successfully edited !');
    //               modal.destroy();
    //             }, err => {
    //               modal.nzFooter[1].loading = false;
    //               this.facade.toastrService.error(err.message);
    //             })
    //         }
    //         else modal.nzFooter[1].loading = false;
    //       }
    //     }],
    // });
  }

  showDeleteConfirm(stageID: number): void {
    //let stageDelete: Stage = this.process.stages.find(p => p.id == stageID);
    // let stageText = stageDelete.title;
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

  // showDetailsModal(stage: Stage): void {
  //   this.emptyStage = this.process.stages.find(s => s.id == stage.id);

  //   let ownerConsultant = this.consultants.find(c => c.id === stage.consultantOwnerId);
  //   let delegateConsultant = this.consultants.find(c => c.id === stage.consultantDelegateId);

  //   this.consultantOwner = ownerConsultant;
  //   this.consultantDelegate = delegateConsultant;


  //   this.isDetailsVisible = true;
  // }

  handleCancel(): void {
    this.isDetailsVisible = false;
    this.isAddVisible = false;
    this.emptyStage = {
      id: 0,
      processId: 0,
      date: new Date,
      // title: chk['label'],
      // startDate: null,
      // endDate: null,
      // description: that.getStageDescription(chk['value']),
      feedback: '',
      status: 0,
      consultantOwnerId: 0,
      consultantDelegateId: 0
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
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.searchValue = '';
    this.nameDropdown.nzVisible = false;
  }

  sort(sortName: string, value: boolean): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  showCandidateDetailsModal(candidateID: number, modalContent: TemplateRef<{}>): void {
    this.emptyCandidate = this.candidates.filter(candidate => candidate.id == candidateID)[0];
    this.candidateDetailsModal.showModal(modalContent, this.emptyCandidate.name + " " + this.emptyCandidate.lastName);
  }

  showConsultantDetailsModal(consultantID: number, modalContent: TemplateRef<{}>): void {
    this.emptyConsultant = this.consultants.filter(consultant => consultant.id == consultantID)[0];
    this.consultantDetailsModal.showModal(modalContent, this.emptyConsultant.name + " " + this.emptyConsultant.lastName);
  }
}
