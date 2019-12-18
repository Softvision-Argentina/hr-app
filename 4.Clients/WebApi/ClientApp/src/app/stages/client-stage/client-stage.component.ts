import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Consultant } from 'src/entities/consultant';
import { FacadeService } from 'src/app/services/facade.service';
import { Process } from 'src/entities/process';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { Globals } from 'src/app/app-globals/globals';
import { ClientStage } from 'src/entities/client-stage';

@Component({
  selector: 'client-stage',
  templateUrl: './client-stage.component.html',
  styleUrls: ['./client-stage.component.css']
})
export class ClientStageComponent implements OnInit {

    @Input()
    private _consultants: Consultant[];
    public get consultants(): Consultant[] {
        return this._consultants;
    }
    public set consultants(value: Consultant[]) {
        this._consultants = value;
    }
    
  clientForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    consultantOwnerId: [null, [Validators.required]],
    interviewer: [null],
    consultantDelegateId:  [null, [Validators.required]],
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
      if (this.clientForm.controls[i] != this.clientForm.controls['status']) {
        if (enable) this.clientForm.controls[i].enable();
        else this.clientForm.controls[i].disable();
      }
    }
  }

  statusChanged(){
    if (this.clientForm.controls['status'].value == 1){
       this.changeFormStatus(true);
       this.clientForm.markAsTouched();
    }
    else{
       this.changeFormStatus(false);
    }
  }

  getFormData(processId: number): ClientStage {
    let stage: ClientStage = new ClientStage();
    let form = this.clientForm;

    stage.id = this.getControlValue(form.controls.id);
    stage.date = this.getControlValue(form.controls.date);
    stage.feedback = this.getControlValue(form.controls.feedback);
    stage.interviewer = this.getControlValue(form.controls.interviewer);
    stage.status = this.getControlValue(form.controls.status);
    stage.consultantOwnerId = this.getControlValue(form.controls.consultantOwnerId);
    stage.consultantDelegateId = this.getControlValue(form.controls.consultantDelegateId);
    stage.delegateName = this.getControlValue(form.controls.delegateName);
    stage.processId = processId;
    stage.rejectionReason = this.getControlValue(form.controls.rejectionReason);
    return stage;
  }

  getControlValue(control: any): any {
    return (control === null ? null : control.value);
  }

  fillForm(clientStage: ClientStage){
    const status: number = this.statusList.filter(s => s.id === clientStage.status)[0].id;
    if (status === StageStatusEnum.InProgress) { this.changeFormStatus(true); }
    this.clientForm.controls['status'].setValue(status);
    if (clientStage.id != null) { this.clientForm.controls['id'].setValue(clientStage.id); }
    if (clientStage.date != null) { this.clientForm.controls['date'].setValue(clientStage.date); }
    if (clientStage.consultantOwnerId != null) {
      this.clientForm.controls['consultantOwnerId'].setValue(clientStage.consultantOwnerId);
    }
    if (clientStage.interviewer != null) {
      this.clientForm.controls['interviewer'].setValue(clientStage.interviewer);
    }
    if (clientStage.consultantDelegateId != null) {
      this.clientForm.controls['consultantDelegateId'].setValue(clientStage.consultantDelegateId);
    }
    if (clientStage.delegateName != null) {
      this.clientForm.controls['delegateName'].setValue(clientStage.delegateName);
    }
    if (clientStage.feedback != null) { this.clientForm.controls['feedback'].setValue(clientStage.feedback); }
    if (clientStage.rejectionReason != null) { this.clientForm.controls['rejectionReason'].setValue(clientStage.rejectionReason); }
  }

  showRejectionReason() {
    if (this.clientForm.controls['status'].value === StageStatusEnum.Rejected || this.clientForm.controls['status'].value === StageStatusEnum.Declined) {
      this.clientForm.controls['rejectionReason'].enable();
      return true;
    }
    this.clientForm.controls['rejectionReason'].disable();
    return false;
  }

}
