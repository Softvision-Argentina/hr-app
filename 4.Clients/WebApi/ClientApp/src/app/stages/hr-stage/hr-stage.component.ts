import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Consultant } from 'src/entities/consultant';
import { FacadeService } from 'src/app/services/facade.service';
import { trimValidator } from 'src/app/directives/trim.validator';
import { Globals } from '../../app-globals/globals';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { HrStage } from '../../../entities/hr-stage';
import { EnglishLevelEnum } from '../../../entities/enums/english-level.enum';
import { AppComponent } from 'src/app/app.component';

@Component({
  selector: 'hr-stage',
  templateUrl: './hr-stage.component.html',
  styleUrls: ['./hr-stage.component.css']
})
export class HrStageComponent implements OnInit {

  disabled = false;

    @Input()
    private _consultants: Consultant[];
    public get consultants(): Consultant[] {
        return this._consultants;
    }
    public set consultants(value: Consultant[]) {
        this._consultants = value;
    }

  hrForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    actualSalary: [null, [Validators.required]],
    wantedSalary: [null, [Validators.required]],
    consultantOwnerId: [null, [Validators.required]],
    consultantDelegateId: [null],
    feedback: [null, [trimValidator]],
    englishLevel: EnglishLevelEnum.None,
    rejectionReason: [null, [Validators.required]],
    rejectionReasonsHr: [0, [Validators.required]]
  });

  statusList: any[] ;
  englishLevelList: any[];
  rejectionReasonsHRList: any[];

  @Input() hrStage: HrStage;

  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals, private _appComponent: AppComponent) {
    this.statusList = globals.stageStatusList.filter(x => x.id !== StageStatusEnum.Hired);
    this.englishLevelList = globals.englishLevelList;
    this.rejectionReasonsHRList = globals.rejectionReasonsHRList;
   }

  ngOnInit() {
    this.changeFormStatus(false);
    if (this.hrStage) { this.fillForm(this.hrStage);
     }
  }

  getFormControl(name: string): AbstractControl {
    return this.hrForm.controls[name];
  }

  changeFormStatus(enable: boolean) {
    for (const i in this.hrForm.controls) {
      if (this.hrForm.controls[i] !== this.hrForm.controls['status']) {
        if (enable) {
          this.hrForm.controls[i].enable();
           if (this.hrForm.controls[i] === this.hrForm.controls['englishLevel']) {
             this.disabled = false;
          }
        } else {
          this.hrForm.controls[i].disable();
          this.disabled = true;
        }
      }
    }
  }

  statusChanged() {
    if (this.hrForm.controls['status'].value === 1) {
      this.changeFormStatus(true);
      this.hrForm.markAsTouched();
    } else {
      this.changeFormStatus(false);
    }
  }


  getFormData(processId: number): HrStage {
    const hrStage: HrStage = new HrStage();

    hrStage.id = this.getControlValue(this.hrForm.controls.id);
    hrStage.date = this.getControlValue(this.hrForm.controls.date);
    hrStage.feedback = this.getControlValue(this.hrForm.controls.feedback);
    hrStage.status = this.getControlValue(this.hrForm.controls.status);
    hrStage.consultantOwnerId = this.getControlValue(this.hrForm.controls.consultantOwnerId);
    hrStage.consultantDelegateId = this.getControlValue(this.hrForm.controls.consultantDelegateId);
    hrStage.processId = processId;
    hrStage.englishLevel = this.getControlValue(this.hrForm.controls.englishLevel);
    hrStage.actualSalary = this.getControlValue(this.hrForm.controls.actualSalary);
    hrStage.wantedSalary = this.getControlValue(this.hrForm.controls.wantedSalary);
    hrStage.consultantDelegateId = this.getControlValue(this.hrForm.controls.consultantDelegateId);
    hrStage.rejectionReason = this.getControlValue(this.hrForm.controls.rejectionReason);
    hrStage.rejectionReasonsHr = this.getControlValue(this.hrForm.controls.rejectionReasonsHr);
    return hrStage;
  }

  getControlValue(control: any): any {
    return (control === null ? null : control.value);
  }

  fillForm(hrStage: HrStage) {
    const status: number = this.statusList.filter(s => s.id === hrStage.status)[0].id;
    if (status === StageStatusEnum.InProgress) {
      this.changeFormStatus(true);
    }
    this.hrForm.controls['status'].setValue(status);

    if (hrStage.id) {
      this.hrForm.controls['id'].setValue(hrStage.id);
    }

    if (hrStage.date) {
      this.hrForm.controls['date'].setValue(hrStage.date);
    }

    if (hrStage.consultantOwnerId) {
      this.hrForm.controls['consultantOwnerId'].setValue(this.hrStage.consultantOwnerId);
    } else {
      this.hrForm.controls['consultantOwnerId'].setValue(1);
    }

    if (hrStage.consultantDelegateId) {
      this.hrForm.controls['consultantDelegateId'].setValue(hrStage.consultantDelegateId);
    } else {
      this.hrForm.controls['consultantDelegateId'].setValue(1);
    }

    if (hrStage.feedback) {
      this.hrForm.controls['feedback'].setValue(hrStage.feedback);
    }

    if (hrStage.actualSalary !== null) {
      this.hrForm.controls['actualSalary'].setValue(hrStage.actualSalary);
    }

    if (hrStage.wantedSalary !== null) {
      this.hrForm.controls['wantedSalary'].setValue(hrStage.wantedSalary);
    }

    if (hrStage.englishLevel) {
      this.hrForm.controls['englishLevel'].setValue(hrStage.englishLevel);
    }

    if (hrStage.rejectionReason) {
      this.hrForm.controls['rejectionReason'].setValue(hrStage.rejectionReason);
    }

    if (hrStage.rejectionReasonsHr) {
      this.hrForm.controls['rejectionReasonsHr'].setValue(hrStage.rejectionReasonsHr);
    }
  }

  showRejectionReason() {
    if (this.hrForm.controls['status'].value === StageStatusEnum.Rejected) {
      this.hrForm.controls['rejectionReason'].enable();
      this.hrForm.controls['rejectionReasonsHr'].enable();
      return true;
    }
    this.hrForm.controls['rejectionReason'].disable();
    this.hrForm.controls['rejectionReasonsHr'].disable();
    return false;
  }

  isUserRole(roles: string[]): boolean {
    return this._appComponent.isUserRole(roles);
  }

}
