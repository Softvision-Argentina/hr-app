import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';
import { Process } from 'src/entities/process';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { Globals } from 'src/app/app-globals/globals';
import { ClientStage } from 'src/entities/client-stage';
import { formFieldHasRequiredValidator } from 'src/app/utils/utils.functions'

@Component({
  selector: 'client-stage',
  templateUrl: './client-stage.component.html',
  styleUrls: ['./client-stage.component.css']
})
export class ClientStageComponent implements OnInit {

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
    userDelegateId:  [null],
    feedback: [null],
    delegateName: [null],
    rejectionReason: [null, [Validators.required]]
  });

  statusList: any[];

  @Input() clientStage: ClientStage;

  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals) {
    this.statusList = globals.stageStatusList.filter(x => x.id !== StageStatusEnum.Hired);
   }

  ngOnInit() {
    this.changeFormStatus(false);
    if (this.clientStage) { this.fillForm(this.clientStage); }
  }

  getFormControl(name: string): AbstractControl {
    return this.clientForm.controls[name];
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
    stage.feedback = this.getControlValue(form.controls.feedback);
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
      this.clientForm.controls['feedback'].setValue(clientStage.feedback);
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
}
