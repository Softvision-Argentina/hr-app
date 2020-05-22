import { Component, OnInit, Input, EventEmitter, Output, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { User } from 'src/entities/user';
import { Globals } from '../../app-globals/globals';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { preOfferStage } from 'src/entities/pre-offer-stage';
import { ProcessService } from '../../services/process.service';
import { PreOfferHistory } from '../pre-offer-history/pre-offer-history.component';
import { dniValidator } from 'src/app/directives/dni.validator';
import { formFieldHasRequiredValidator } from 'src/app/utils/utils.functions'

@Component({
  selector: 'pre-offer-stage',
  templateUrl: './pre-offer-stage.component.html',
  styleUrls: ['./pre-offer-stage.component.css'],
  providers: [PreOfferHistory]
})

export class PreOfferStageComponent implements OnInit {

  @Input()
  private _users: User[];
  public get users(): User[] {
      return this._users.sort((a,b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
  }
  public set users(value: User[]) {
      this._users = value;
  }

  preOfferForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date()],
    dni: [0, [Validators.required, dniValidator]],
    userOwnerId: null,
    userDelegateId: [null],
    feedback: '',
    seniority: [0, [Validators.required]],
    remunerationOffer: [0, [Validators.required]],
    vacationDays: [0, [Validators.required]],
    firstday: [new Date(), [Validators.required]],
    bonus: '',
    hireDate: [new Date(), [Validators.required]],
    backgroundCheckDone: false,
    backgroundCheckDoneDate: [new Date()],
    preocupationalDone: false,
    preocupationalDoneDate: [new Date()],
    rejectionReason: [null, [Validators.required]],
  });

  statusList: any[];
  seniorityList: any[];
  backCheckEnabled = false;
  backDateEnabled: boolean;
  preocupationalCheckEnabled = false;
  preocupationalDateEnabled: boolean;
  isDniLoading = false;
  isDniValid = false;
  acceptedDeclined: any[] = [
    { label: 'Declined', value: true},
    { label: 'Accepted', value: false}
  ];
  @Input() preOfferStage: preOfferStage;
  @ViewChild(PreOfferHistory) prehistoryOffer: PreOfferHistory ;
  @Output() selectedSeniority = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private globals: Globals,
    private processService: ProcessService,
    public prehistoryOfferModal: PreOfferHistory) {

    this.statusList = globals.preOfferStatusList;
  }

  showPreOfferHistoryModal(modalContent: TemplateRef<{}>) {
    this.prehistoryOffer.showModal(modalContent);
  }

  ngOnInit() {
    this.processService.selectedSeniorities.subscribe(sr => {
      this.seniorityList = sr;
      this.preOfferForm.controls['seniority'].setValue(this.seniorityList[0].id);
    });
    this.changeFormStatus(false);
    if (this.preOfferStage) { this.fillForm(this.preOfferStage); }
  }

  updateSeniority(seniorityId) {
    this.selectedSeniority.emit(seniorityId);
  }

  getFormControl(name: string): AbstractControl {
    return this.preOfferForm.controls[name];
  }

  changeFormStatus(enable: boolean) {
    for (const i in this.preOfferForm.controls) {
      if (this.preOfferForm.controls[i] !== this.preOfferForm.controls['status']) {
        if (enable) {
          this.preOfferForm.controls[i].enable();
        } else {
          this.preOfferForm.controls[i].disable();
        }
      }
    }

    this.backCheckEnabled = enable;
    this.preocupationalCheckEnabled = enable;
  }

  statusChanged() {
    if (this.preOfferForm.controls['status'].value === StageStatusEnum.InProgress) {
       this.changeFormStatus(true);
       this.preOfferForm.markAsTouched();
    } else {
       this.changeFormStatus(false);
    }
  }

  getFormData(processId: number): preOfferStage {
    const stage: preOfferStage = new preOfferStage();
    const form = this.preOfferForm;

    stage.id = this.getControlValue(form.controls.id);
    stage.date = this.getControlValue(form.controls.date);
    stage.dni = this.getControlValue(form.controls.dni);
    stage.feedback = this.getControlValue(form.controls.feedback);
    stage.status = this.getControlValue(form.controls.status);
    stage.userOwnerId = this.getControlValue(form.controls.userOwnerId);
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.processId = processId;
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.seniority = this.getControlValue(form.controls.seniority);
    stage.remunerationOffer = this.getControlValue(form.controls.remunerationOffer);
    stage.vacationDays = this.getControlValue(form.controls.vacationDays);
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

  fillForm(preOfferStage: preOfferStage) {
    const status: number = this.statusList.filter(s => s.id === preOfferStage.status)[0].id;

    if (status === StageStatusEnum.InProgress) {
      this.changeFormStatus(true);
    }

    this.preOfferForm.controls['status'].setValue(status);

    if (preOfferStage.id) {
      this.preOfferForm.controls['id'].setValue(preOfferStage.id);
    }

    if (preOfferStage.dni !== 0) {
      this.preOfferForm.controls['dni'].setValue(preOfferStage.dni);
    }

    if (preOfferStage.vacationDays) {
      this.preOfferForm.controls['vacationDays'].setValue(preOfferStage.vacationDays);
    }
    if (preOfferStage.remunerationOffer) {
      this.preOfferForm.controls['remunerationOffer'].setValue(preOfferStage.remunerationOffer);
    }

    if (preOfferStage.date) {
      this.preOfferForm.controls['date'].setValue(preOfferStage.date);
    }

    if (preOfferStage.userOwnerId) {
      this.preOfferForm.controls['userOwnerId'].setValue(preOfferStage.userOwnerId);
    }

    if (preOfferStage.seniority) {
      this.preOfferForm.controls['seniority'].setValue(preOfferStage.seniority);
    }

    if (preOfferStage.hireDate) {
      this.preOfferForm.controls['hireDate'].setValue(preOfferStage.hireDate);
    }

    if (preOfferStage.feedback) {
      this.preOfferForm.controls['feedback'].setValue(preOfferStage.feedback);
    }

    if (preOfferStage.bonus) {
      this.preOfferForm.controls['bonus'].setValue(preOfferStage.bonus);
    }

    if (preOfferStage.firstday) {
      this.preOfferForm.controls['firstday'].setValue(preOfferStage.firstday);
    }

    if (preOfferStage.backgroundCheckDone) {
      this.preOfferForm.controls['backgroundCheckDone'].setValue(preOfferStage.backgroundCheckDone);
    }

    if (preOfferStage.backgroundCheckDoneDate) {
      this.preOfferForm.controls['backgroundCheckDoneDate'].setValue(preOfferStage.backgroundCheckDoneDate);
    }

    if (preOfferStage.preocupationalDone) {
      this.preOfferForm.controls['preocupationalDone'].setValue(preOfferStage.preocupationalDone);
    }

    if (preOfferStage.preocupationalDoneDate) {
      this.preOfferForm.controls['preocupationalDoneDate'].setValue(preOfferStage.preocupationalDoneDate);
    }

    if (preOfferStage.rejectionReason) {
      this.preOfferForm.controls['rejectionReason'].setValue(preOfferStage.rejectionReason);
    }
  }

  toggleBackgroundCheck() {
    this.backDateEnabled = !this.backDateEnabled;
  }

  togglePreocupationalCheck() {
    this.preocupationalDateEnabled = !this.preocupationalDateEnabled;
  }

  dniChanged() {
    this.isDniValid = false;
    this.changeFormStatus(false);
  }

  showRejectionReason() {
    if (this.preOfferForm.controls['status'].value === StageStatusEnum.Rejected) {
      this.preOfferForm.controls['rejectionReason'].enable();
      return true;
    }
    this.preOfferForm.controls['rejectionReason'].disable();
    return false;
  }

  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.preOfferForm);
  }
}
