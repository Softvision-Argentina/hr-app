import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { Globals } from 'src/app/app-globals/globals';
import { ClientStage } from 'src/entities/client-stage';
import { formFieldHasRequiredValidator } from 'src/app/utils/utils.functions'
import { Interview } from 'src/entities/interview';
import { getLocaleDateTimeFormat } from '@angular/common';

@Component({
  selector: 'client-stage',
  templateUrl: './client-stage.component.html',
  styleUrls: ['./client-stage.component.css']
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
  feedbackContent:string = "";

  statusList: any[];
  interviews: Interview[] = []
  panelControl: any = {
    active: false,
    name: 'Add New Interview',
    arrow: false
  }
  editCache: { [key: number]: { edit: boolean; data: Interview } } = {};

  @Input() clientStage: ClientStage;
  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {
    this.statusList = globals.stageStatusList.filter(x => x.id !== StageStatusEnum.Hired);
  }

  resetInterviewDate;

  ngOnInit() {
    this.changeFormStatus(false);
    this.getInterviews();
    if (this.clientStage) { this.fillForm(this.clientStage); }
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
    stage.interviews = this.interviews;
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
        id: 0,
        client: this.getControlValue(this.interviewForm.controls.interviewClient),
        clientInterviewer: this.getControlValue(this.interviewForm.controls.clientInterviewer),
        interviewDate: this.getControlValue(this.interviewForm.controls.interviewDate),
        feedback: this.getControlValue(this.interviewForm.controls.interviewFeedback),
        project: this.getControlValue(this.interviewForm.controls.project),
        clientStageId: this.clientStage.id
      };

      this.interviews.push(interview);
      this.facade.InterviewSevice.add(interview)
        .subscribe(res => {
          this.interviews = [...this.interviews];
          this.updateEditCache();
        })
      this.panelControl = {
        active: true,
        name: 'Add New Interview',
        arrow: false
      };
    this.facade.toastrService.success('Interview added!');
      this.resetInterviewDate = new Date();
      this.interviewClient.nativeElement.value = '';
      this.interviewClient.nativeElement.value = '';
      this.clientInterviewer.nativeElement.value = '';
      this.project.nativeElement.value = '';
      this.interviewFeedback.nativeElement.value = '';
    }
  }

  getInterviews() {
    this.facade.InterviewSevice.get()
      .subscribe(res => {
        this.interviews = res.filter(i => i.clientStageId === this.clientStage.id)
        this.updateEditCache();
      })
  }

  deleteInterview(interviewId: number) {

    this.facade.InterviewSevice.delete(interviewId)
      .subscribe(res => {
        this.getInterviews();
      }, err => {
        console.log(err);

      })
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
    this.facade.InterviewSevice.update(id, this.editCache[id].data)
      .subscribe(res => {
        this.getInterviews()
        this.editCache[id].edit = false;
      })
  }
  updateEditCache(): void {
    this.interviews.forEach(item => {
      this.editCache[item.id] = {
        edit: false,
        data: { ...item }
      };
    });

  }
}
