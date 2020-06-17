import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { Globals } from 'src/app/app-globals/globals';
import { ClientStage } from 'src/entities/client-stage';
import { formFieldHasRequiredValidator } from 'src/app/utils/utils.functions'
import { Interview } from 'src/entities/interview';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'client-stage',
  templateUrl: './client-stage.component.html',
  styleUrls: ['./client-stage.component.scss']
})
export class ClientStageComponent implements OnInit {

  @ViewChild('interviewClient') interviewClient;
  @ViewChild('clientInterviewer') clientInterviewer;
  @ViewChild('project') project;
  @ViewChild('interviewFeedback') interviewFeedback;

  @Input()
  private _users: User[];
  public get users(): User[] {
    return this._users;
  }
  public set users(value: User[]) {
    this._users = value;
  }


  @Input()
  processSaveEvent: Observable<number>;

  clientForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    userOwnerId: [null],
    interviewer: [null],
    userDelegateId: [null],
    feedback: [null],
    delegateName: [null],
    rejectionReason: [null, [Validators.required]],
    interviews: [null]
  });

  interviewForm: FormGroup = this.fb.group({
    interviewDate: [new Date(), [Validators.required]],
    interviewClient: [null, [Validators.required]],
    clientInterviewer: [null],
    project: [null],
    interviewFeedback: [null, [Validators.required, Validators.maxLength(300)]]
  });
  feedbackContent: string = "";

  statusList: any[];
  interviews: Interview[] = []
  panelControl: any = {
    active: false,
    name: 'Add New Interview',
    arrow: false
  }
  editCache: { [key: number]: { edit: boolean; data: Interview } } = {};
  interviewOperations: { operation: string; data: Interview }[];
  editInterviewCounter: number;
  processSaveSubscription: Subscription;

  @Input() clientStage: ClientStage;
  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {
    this.statusList = globals.stageStatusList.filter(x => x.id !== StageStatusEnum.Hired);
  }

  resetInterviewDate;

  ngOnInit() {
    this.processSaveSubscription = this.processSaveEvent.subscribe((res) => this.saveChangesToInterviewListInDatabase(res))
    this.interviewOperations = [];
    this.changeFormStatus(false);
    this.getInterviews();
    if (this.clientStage) { this.fillForm(this.clientStage); }
  }

  saveChangesToInterviewListInDatabase(processId: number): void {

    this.facade.processService.getByID(processId)
      .subscribe(res => {
        this.interviewOperations.forEach(o => {
          let interviewToSave = o.data;
          interviewToSave.clientStageId = res.clientStage.id;

          if (o.operation === "add") {
            this.facade.InterviewSevice.add(interviewToSave)
              .subscribe(res => {
              }, err => {
                console.log(err);
              })
          }

          if (o.operation === "delete") {
            this.facade.InterviewSevice.delete(interviewToSave.id)
              .subscribe(res => {
              }, err => {
                console.log(err);
              })
          }
          if (o.operation === "edit") {
            this.facade.InterviewSevice.update(interviewToSave.id, interviewToSave)
              .subscribe(res => {
              }, err => {
                console.log(err);
              })
          }
        })
        this.processSaveSubscription.unsubscribe();


      })
  }

  getFormControl(name: string): AbstractControl {
    return this.clientForm.controls[name];
  }

  getFeedbackContent(content: string): void {
    this.feedbackContent = content;
  }

  changeFormStatus(enable: boolean) {
    for (const i in this.clientForm.controls) {
      if (this.clientForm.controls[i] !== this.clientForm.controls['status']) {
        if (enable) {
          this.clientForm.controls[i].enable();
        } else {
          this.clientForm.controls[i].disable();
        }
      }
    }
  }

  statusChanged() {
    if (this.clientForm.controls['status'].value === 1) {
      this.changeFormStatus(true);
      this.clientForm.markAsTouched();
    } else {
      this.changeFormStatus(false);
    }
  }

  getFormData(processId: number): ClientStage {
    const stage: ClientStage = new ClientStage();
    const form = this.clientForm;

    stage.id = this.getControlValue(form.controls.id);
    stage.date = this.getControlValue(form.controls.date);
    stage.feedback = this.feedbackContent;
    stage.interviewer = this.getControlValue(form.controls.interviewer);
    stage.status = this.getControlValue(form.controls.status);
    stage.userOwnerId = this.getControlValue(form.controls.userOwnerId);
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.delegateName = this.getControlValue(form.controls.delegateName);
    stage.processId = processId;
    stage.rejectionReason = this.getControlValue(form.controls.rejectionReason);
    return stage;
  }

  getControlValue(control: any): any {
    return (control === null ? null : control.value);
  }

  fillForm(clientStage: ClientStage) {
    const status: number = this.statusList.filter(s => s.id === clientStage.status)[0].id;
    if (status === StageStatusEnum.InProgress) {
      this.changeFormStatus(true);
    }
    this.clientForm.controls['status'].setValue(status);

    if (clientStage.id) {
      this.clientForm.controls['id'].setValue(clientStage.id);
    }

    if (clientStage.date) {
      this.clientForm.controls['date'].setValue(clientStage.date);
    }

    if (clientStage.userOwnerId) {
      this.clientForm.controls['userOwnerId'].setValue(clientStage.userOwnerId);
    }

    if (clientStage.interviewer) {
      this.clientForm.controls['interviewer'].setValue(clientStage.interviewer);
    }

    if (clientStage.userDelegateId) {
      this.clientForm.controls['userDelegateId'].setValue(clientStage.userDelegateId);
    }

    if (clientStage.delegateName) {
      this.clientForm.controls['delegateName'].setValue(clientStage.delegateName);
    }

    if (clientStage.feedback) {
      this.feedbackContent = clientStage.feedback;
    }
    if (clientStage.rejectionReason) {
      this.clientForm.controls['rejectionReason'].setValue(clientStage.rejectionReason);
    }
  }

  showRejectionReason() {
    if (this.clientForm.controls['status'].value === StageStatusEnum.Rejected) {
      this.clientForm.controls['rejectionReason'].enable();
      return true;
    }
    this.clientForm.controls['rejectionReason'].disable();
    return false;
  }

  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.clientForm)
  }
  addInterview() {
    if (this.validateInterviewForm()) {
      let interview: Interview = {
        id: this.editInterviewCounter,
        client: this.getControlValue(this.interviewForm.controls.interviewClient),
        clientInterviewer: this.getControlValue(this.interviewForm.controls.clientInterviewer),
        interviewDate: this.getControlValue(this.interviewForm.controls.interviewDate),
        feedback: this.getControlValue(this.interviewForm.controls.interviewFeedback),
        project: this.getControlValue(this.interviewForm.controls.project),
        clientStageId: this.clientStage.id
      };

      this.editInterviewCounter++
      this.panelControl = {
        active: true,
        name: 'Add New Interview',
        arrow: false
      };

      this.interviews.push(interview);
      this.interviews = [...this.interviews];


      this.interviewOperations.push(
        {
          operation: "add",
          data: interview,
        }
      )

      this.facade.toastrService.success('Interview added!');
      this.resetInterviewDate = new Date();
      this.interviewClient.nativeElement.value = '';
      this.clientInterviewer.nativeElement.value = '';
      this.project.nativeElement.value = '';
      this.interviewFeedback.nativeElement.value = '';

      this.updateEditCache();
    }
  }

  getInterviews() {
    this.facade.InterviewSevice.get()
      .subscribe(res => {
        this.editInterviewCounter = this.interviews.length
        this.interviews = res.filter(i => i.clientStageId === this.clientStage.id)
        this.updateEditCache();
      })
  }

  showDeleteConfirmDialog(interviewId: number) {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete this interview?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.deleteInterview(interviewId)
    });
  }

  deleteInterview(interviewId: number) {

    let operation = this.interviewOperations.find(o => o.data.id === interviewId && o.operation === "add");

    if (operation) {
      this.interviewOperations = this.interviewOperations.filter(o => o.data.id !== interviewId);
    } else {
      this.interviewOperations = this.interviewOperations.filter(o => o.data.id !== interviewId);

      this.interviewOperations.push(
        {
          operation: "delete",
          data: this.interviews.find(interview => interview.id === interviewId)
        }
      )
    }
    this.interviews = [...this.interviews.filter(interview => interview.id !== interviewId)];
  }


  private validateInterviewForm() {
    for (const i in this.interviewForm.controls) {
      this.interviewForm.controls[i].markAsDirty();
      this.interviewForm.controls[i].updateValueAndValidity();
    }
    if (this.interviewForm.invalid) {
      return false
    }
    return true
  }
  startEdit(id: string): void {
    this.editCache[id].edit = true;
  }

  cancelEdit(id: number): void {
    const index = this.interviews.findIndex(item => item.id === id);
    this.editCache[id] = {
      data: { ...this.interviews[index] },
      edit: false
    };
  }
  saveEdit(id: number): void {

    let operation = this.interviewOperations.find(o => o.data.id === id && o.operation === "add");
    const index = this.interviews.findIndex(item => item.id === id);
    if (operation) {

      this.interviewOperations = this.interviewOperations.filter(o => o.data.id !== id);
      this.interviewOperations.push(
        {
          operation: "add",
          data: this.interviews[index],
        }
      )
    } else {

      this.interviewOperations = this.interviewOperations.filter(o => o.data.id !== id);
      this.interviewOperations.push(
        {
          operation: "edit",
          data: this.interviews[index],
        }
      )

    }
    Object.assign(this.interviews[index], this.editCache[id].data);
    this.editCache[id].edit = false;
  }

  updateEditCache(): void {
    this.interviews.forEach(item => {
      this.editCache[item.id] = {
        edit: false,
        data: { ...item }
      };
    });

  }

  hideAddNewInterviewForm() {
    return !(this.clientForm.controls.status.value === StageStatusEnum.InProgress);
  }
}
