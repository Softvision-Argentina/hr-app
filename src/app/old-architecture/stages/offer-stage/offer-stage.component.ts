import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HealthInsuranceEnum } from '@shared/enums/health-insurance.enum';
import { StageStatusEnum } from '@shared/enums/stage-status.enum';
import { OfferStage } from '@shared/models/offer-stage.model';
import { User } from '@shared/models/user.model';
import { ProcessService } from '@shared/services/process.service';
import { Globals } from '@shared/utils/globals';
import { formFieldHasRequiredValidator } from '@shared/utils/utils.functions';

@Component({
  selector: 'offer-stage',
  templateUrl: './offer-stage.component.html',
  styleUrls: ['./offer-stage.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OfferStageComponent implements OnInit, OnChanges {

  @Input()
  private _users: User[];
  public get users(): User[] {
      return this._users.sort((a,b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
  }
  public set users(value: User[]) {
      this._users = value;
  }

  offerForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    userOwnerId: null,
    userDelegateId: [null],
    feedback: '',
    seniority: [0, [Validators.required]],
    remunerationOffer: [0, [Validators.required]],
    vacationDays: [0, [Validators.required]],
    healthInsurance: [0, [Validators.required]],
    notes: '',
    firstday: [new Date(), [Validators.required]],
    bonus: '',
    hireDate: [new Date(), [Validators.required]],
    backgroundCheckDone: false,
    backgroundCheckDoneDate: [new Date(), [Validators.required]],
    preocupationalDone: false,
    preocupationalDoneDate: [new Date(), [Validators.required]],
    rejectionReason: [null]
  });

  remunerationOfferControl: AbstractControl;
  vacationDaysControl: AbstractControl;
  healthInsuranceControl: AbstractControl;
  bonusControl: AbstractControl;
  hireDateControl: AbstractControl;

  feedbackContent:string = "";

  statusList: any[];
  seniorityList: any[];
  healthInsuranceList: any[];
  backCheckEnabled = false;
  backDateEnabled: boolean;
  preocupationalCheckEnabled = false;
  preocupationalDateEnabled: boolean;

  @Input() offerStage: OfferStage;
  @Output() selectedSeniority = new EventEmitter();

  @Input() preOfferData: {tentativeStartDate: Date, bonus: number, grossSalary: number, vacationDays: number, healthInsurance: HealthInsuranceEnum} = null;

  constructor(
    private fb: FormBuilder,
    private globals: Globals,
    private processService: ProcessService) {

    this.statusList = globals.offerStatusList;
    this.healthInsuranceList = globals.healthInsuranceList;
  }

  ngOnInit() {
    this.processService.selectedSeniorities.subscribe(sr => {
      this.seniorityList = sr;
      this.offerForm.controls['seniority'].setValue(this.seniorityList[0].id);
    });
    this.changeFormStatus(false);
    if (this.offerStage) { this.fillForm(this.offerStage); }

    this.setAbstractControls();
  }

  ngOnChanges() {
    if(this.preOfferData && (this.offerForm.controls['status'].value === StageStatusEnum.NA)) {
      this.hireDateControl.setValue(this.preOfferData.tentativeStartDate);
      this.bonusControl.setValue(this.preOfferData.bonus);
      this.remunerationOfferControl.setValue(this.preOfferData.grossSalary);
      this.vacationDaysControl.setValue(this.preOfferData.vacationDays);
      this.healthInsuranceControl.setValue(this.preOfferData.healthInsurance);
    }
  }

  updateSeniority(seniorityId) {
    this.selectedSeniority.emit(seniorityId);
  }

  getFormControl(name: string): AbstractControl {
    return this.offerForm.controls[name];
  }

  getFeedbackContent(content: string): void {
    this.feedbackContent = content;
  }

  changeFormStatus(enable: boolean) {
    for (const i in this.offerForm.controls) {
      if (this.offerForm.controls[i] !== this.offerForm.controls['status'] &&
      this.offerForm.controls[i] !== this.offerForm.controls.backgroundCheckDoneDate &&
      this.offerForm.controls[i] !== this.offerForm.controls.preocupationalDoneDate) {
        if (enable) {
          this.offerForm.controls[i].enable();
        } else {
          this.offerForm.controls[i].disable();
        }
      }
    }

    this.backCheckEnabled = enable;
    this.preocupationalCheckEnabled = enable;
    this.enableBackDate();
    this.enablePreocupationalDate();
  }

  statusChanged() {
    if (this.offerForm.controls['status'].value === StageStatusEnum.InProgress) {
       this.changeFormStatus(true);
       this.offerForm.markAsTouched();
    } else {
       this.changeFormStatus(false);
    }
  }

  getFormData(processId: number): OfferStage {
    const stage: OfferStage = new OfferStage();
    const form = this.offerForm;

    stage.id = this.getControlValue(form.controls.id);
    stage.date = this.getControlValue(form.controls.date);
    stage.feedback = this.feedbackContent;
    stage.status = this.getControlValue(form.controls.status);
    stage.userOwnerId = this.getControlValue(form.controls.userOwnerId);
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.processId = processId;
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.seniority = this.getControlValue(form.controls.seniority);
    stage.remunerationOffer = this.getControlValue(form.controls.remunerationOffer);
    stage.vacationDays = this.getControlValue(form.controls.vacationDays);
    stage.healthInsurance = this.getControlValue(form.controls.healthInsurance);
    stage.notes = this.getControlValue(form.controls.notes);
    stage.firstday = this.getControlValue(form.controls.firstday);
    stage.bonus = this.getControlValue(form.controls.bonus);
    stage.hireDate = this.getControlValue(form.controls.hireDate);
    stage.backgroundCheckDone = this.getControlValue(form.controls.backgroundCheckDone);
    stage.backgroundCheckDoneDate = stage.backgroundCheckDone ? this.getControlValue(form.controls.backgroundCheckDoneDate) : null;
    stage.preocupationalDone = this.getControlValue(form.controls.preocupationalDone);
    stage.preocupationalDoneDate = stage.preocupationalDone ? this.getControlValue(form.controls.preocupationalDoneDate) : null;
    stage.rejectionReason = this.getControlValue(form.controls.rejectionReason);
    return stage;
  }

  getControlValue(control: any): any {
    return (control === null ? null : control.value);
  }

  fillForm(offerStage: OfferStage) {
    const status: number = this.statusList.filter(s => s.id === offerStage.status)[0].id;

    if (status === StageStatusEnum.InProgress) {
      this.changeFormStatus(true);
    }

    this.offerForm.controls['status'].setValue(status);

    if (offerStage.id) {
      this.offerForm.controls['id'].setValue(offerStage.id);
    }

    if (offerStage.date) {
      this.offerForm.controls['date'].setValue(offerStage.date);
    }

    if (offerStage.userOwnerId) {
      this.offerForm.controls['userOwnerId'].setValue(offerStage.userOwnerId);
    }

    if (offerStage.seniority) {
      this.offerForm.controls['seniority'].setValue(offerStage.seniority);
    }

    if (offerStage.hireDate) {
      this.offerForm.controls['hireDate'].setValue(offerStage.hireDate);
    }

    if (offerStage.feedback) {
      this.feedbackContent = offerStage.feedback;
    }

    if (offerStage.remunerationOffer) {
      this.offerForm.controls['remunerationOffer'].setValue(offerStage.remunerationOffer);
    }

    if (offerStage.vacationDays) {
      this.offerForm.controls['vacationDays'].setValue(offerStage.vacationDays);
    }

    if (offerStage.healthInsurance) {
      this.offerForm.controls['healthInsurance'].setValue(offerStage.healthInsurance);
    }
    
    if (offerStage.notes) {
      this.offerForm.controls['notes'].setValue(offerStage.notes);
    }

    if (offerStage.firstday) {
      this.offerForm.controls['firstday'].setValue(offerStage.firstday);
    }

    if (offerStage.bonus) {
      this.offerForm.controls['bonus'].setValue(offerStage.bonus);
    }

    if (offerStage.backgroundCheckDone) {
      this.offerForm.controls['backgroundCheckDone'].setValue(offerStage.backgroundCheckDone);
      this.backDateEnabled = offerStage.backgroundCheckDone;
      this.enableBackDate();
    }

    if (offerStage.backgroundCheckDoneDate) {
      this.offerForm.controls['backgroundCheckDoneDate'].setValue(offerStage.backgroundCheckDoneDate);
    }

    if (offerStage.preocupationalDone) {
      this.offerForm.controls['preocupationalDone'].setValue(offerStage.preocupationalDone);
      this.preocupationalDateEnabled = offerStage.preocupationalDone;
      this.enablePreocupationalDate();
    }

    if (offerStage.preocupationalDoneDate) {
      this.offerForm.controls['preocupationalDoneDate'].setValue(offerStage.preocupationalDoneDate);
    }

    if (offerStage.rejectionReason) {
      this.offerForm.controls['rejectionReason'].setValue(offerStage.rejectionReason);
    }
  }

  toggleBackgroundCheck() {
    this.backDateEnabled = !this.backDateEnabled;
    this.enableBackDate();
  }

  togglePreocupationalCheck() {
    this.preocupationalDateEnabled = !this.preocupationalDateEnabled;
    this.enablePreocupationalDate();
  }

  enableBackDate() {
    if (this.backCheckEnabled && this.backDateEnabled) {
      this.offerForm.controls.backgroundCheckDoneDate.enable();
    } else {
      this.offerForm.controls.backgroundCheckDoneDate.disable();
    }
  }

  enablePreocupationalDate() {
    if (this.preocupationalCheckEnabled && this.preocupationalDateEnabled) {
      this.offerForm.controls.preocupationalDoneDate.enable();
    } else {
      this.offerForm.controls.preocupationalDoneDate.disable();
    }
  }


  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.offerForm)
  }

  setAbstractControls() {
    this.remunerationOfferControl = this.offerForm.controls["remunerationOffer"];
    this.vacationDaysControl = this.offerForm.controls["vacationDays"];
    this.healthInsuranceControl = this.offerForm.controls["healthInsurance"];
    this.bonusControl = this.offerForm.controls["bonus"];
    this.hireDateControl = this.offerForm.controls["hireDate"];
  }
}
