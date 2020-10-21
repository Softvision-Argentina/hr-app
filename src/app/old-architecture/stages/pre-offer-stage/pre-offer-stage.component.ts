import { Component, EventEmitter, Input, OnInit, OnChanges, Output, TemplateRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { StageStatusEnum } from '@shared/enums/stage-status.enum';
import { CheckedEnum } from '@shared/enums/checked.enum';
import { PreOfferStage } from '@shared/models/pre-offer-stage.model';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
import { ReaddressReason } from '@shared/models/readdress-reason.model';
import { ReaddressStatus } from '@shared/models/readdress-status.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { ProcessService } from '@shared/services/process.service';
import { dniValidator, UniqueDniValidator } from '@shared/utils/dni.validator';
import { checkDateIsnotEmpty } from '@shared/utils/forms.validators';
import { Globals } from '@shared/utils/globals';
import { CanShowReaddressPossibility, formFieldHasRequiredValidator } from '@shared/utils/utils.functions';
import { PreOfferHistory } from '../pre-offer-history/pre-offer-history.component';
import { resizeModal } from '@app/shared/utils/resize-modal.util';
import { Process } from '@shared/models/process.model';

@Component({
  selector: 'pre-offer-stage',
  templateUrl: './pre-offer-stage.component.html',
  styleUrls: ['./pre-offer-stage.component.scss'],
  providers: [PreOfferHistory]
})

export class PreOfferStageComponent implements OnInit, OnChanges {

  @Input()
  private _users: User[];
  public get users(): User[] {
    return this._users.sort((a, b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
  }
  public set users(value: User[]) {
    this._users = value;
  }

  @Input() processId: number; 
  @Input() preOfferStage: PreOfferStage;
  @Input() readdressReasonList: ReaddressReason[] = [];
  @Input() readdressReasonTypeList: ReaddressReasonType[] = [];
  @Input() processes: Process[] = [];
  @ViewChild(PreOfferHistory) preOfferHistory: PreOfferHistory;
  @Output() selectedSeniority = new EventEmitter();

  statusList: any[];
  seniorityList: any[];
  backCheckEnabled = false;
  backDateEnabled: boolean;
  preocupationalCheckEnabled = false;
  preocupationalDateEnabled: boolean;
  isDniLoading = false;
  isDniValid = false;
  acceptedDeclined: any[] = [
    { label: 'Declined', value: true },
    { label: 'Accepted', value: false }
  ];

  currentStageStatus: StageStatusEnum;
  readdressStatus: ReaddressStatus = new ReaddressStatus();

  currentReaddressDescription: string = '';
  readdressFilteredList: ReaddressReason[] = [];
  selectedReasonId: number;
  selectedReason: string;
  checkedEnum = CheckedEnum;  

  preOfferForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [],
    dni: [null, [dniValidator]],
    userOwnerId: null,
    userDelegateId: [null],
    feedback: '',
    seniority: [0, [Validators.required]],
    remunerationOffer: [0, [Validators.required]],
    notes: '', // should be removed because it's in pre-offer-history
    firstday: [new Date(), [Validators.required]],  // should be removed because it's in pre-offer-history
    bonus: '', // should be removed because it's in pre-offer-history
    hireDate: [new Date(), [Validators.required]],  // should be removed because it's in pre-offer-history
    backgroundCheck: this.fb.group({
      done: this.checkedEnum.NA,
      date: null,
    }, {validator: checkDateIsnotEmpty }),
    preocupational: this.fb.group({
      done: this.checkedEnum.NA,
      date: null,
    }, { validator: checkDateIsnotEmpty }),
    rejectionReason: [null],
    reasonSelectControl: [null],
    reasonDescriptionTextAreaControl: [null]
  });

  constructor(
    private fb: FormBuilder,
    private globals: Globals,
    private facade: FacadeService,
    private processService: ProcessService,
    public preOfferHistoryModal: PreOfferHistory) {

    this.statusList = globals.preOfferStatusList;
  }

  showPreOfferHistoryModal(modalContent: TemplateRef<{}>) {
    this.preOfferHistoryModal.showModal(modalContent);
  }

  ngOnInit() {
    this.preOfferForm.controls['dni'].setAsyncValidators(UniqueDniValidator(this.processes, this.processId));
    this.currentStageStatus = this.preOfferStage.status;
    let stageName = StageStatusEnum[this.currentStageStatus].toLowerCase();
    this.readdressFilteredList = this.readdressReasonList?.filter((reason) => { return reason.type.toLowerCase() == stageName });
    
    this.selectedReason = undefined;
    this.readdressStatus.feedback = undefined;
    this.readdressStatus.fromStatus = undefined;
    this.readdressStatus.toStatus = undefined;
    this.readdressStatus.id = undefined;

    if (this.preOfferStage.readdressStatus) {
      this.selectedReason = `${this.preOfferStage.readdressStatus.readdressReasonId}`;
      this.readdressStatus.feedback = this.preOfferStage.readdressStatus.feedback;
      this.readdressStatus.fromStatus = this.preOfferStage.status;
      this.readdressStatus.toStatus = this.preOfferStage.readdressStatus.toStatus;
      this.readdressStatus.id = this.preOfferStage.readdressStatus.id;
    }

    this.processService.selectedSeniorities.subscribe(sr => {
      this.seniorityList = sr;
      this.preOfferForm.controls['seniority'].setValue(this.seniorityList[0].id);
    });
    this.changeFormStatus(false);
    if (this.preOfferStage) { this.fillForm(this.preOfferStage); }
  }


  ngOnChanges() {
    this.setFieldValidations();
  }

  updateSeniority(seniorityId) {
    this.selectedSeniority.emit(seniorityId);
  }

  getFormControl(name: string): AbstractControl {
    return this.preOfferForm.controls[name];
  }

  changeFormStatus(enable: boolean) {
    const statusControl = this.preOfferForm.controls['status'];
    const dniControl = this.preOfferForm.controls['dni'];
    const birthDateControl = this.preOfferForm.controls['date'];

    for (const i in this.preOfferForm.controls) {
      if (this.preOfferForm.controls[i] !== statusControl) {
        if (enable) {
          this.preOfferForm.controls[i].enable();
        } else {
          this.preOfferForm.controls[i].disable();
          if(this.currentStageStatus === StageStatusEnum.Accepted && 
              (this.preOfferForm.controls[i] === dniControl || this.preOfferForm.controls[i] === birthDateControl)) {
            this.preOfferForm.controls[i].enable();
          }
        }
      }
    }

    this.backCheckEnabled = enable;
    this.preocupationalCheckEnabled = enable;
    this.preOfferForm.controls['reasonSelectControl'].setValue(undefined);
    this.preOfferForm.controls['reasonSelectControl'].enable();
    this.preOfferForm.controls['reasonDescriptionTextAreaControl'].enable();
  }

  statusChanged() {

    this.readdressStatus.feedback = undefined
    this.readdressStatus.readdressReasonId = undefined;
    this.preOfferForm.controls['reasonDescriptionTextAreaControl'].setValue("");
    this.currentStageStatus = this.preOfferForm.controls['status'].value;

    if (this.preOfferForm.controls['status'].value === StageStatusEnum.InProgress || this.preOfferForm.controls['status'].value === StageStatusEnum.PendingReply) {
      this.changeFormStatus(true);
      this.preOfferForm.markAsTouched();
    } else {
      this.changeFormStatus(false);
    }

    let stageName = StageStatusEnum[this.currentStageStatus].toLowerCase();
    this.readdressFilteredList = this.readdressReasonList.filter((reason) => { return reason.type.toLowerCase() == stageName });
    this.readdressStatus.toStatus = this.currentStageStatus;

    //Temporal fix to make modal reize when the form creates new items dinamically that exceeds the height of the modal.
    resizeModal();
  }

  getFormData(processId: number): PreOfferStage {
    const stage: PreOfferStage = new PreOfferStage();
    const form = this.preOfferForm;
    const backgroundCheckDone = this.getControlValue(form.get('backgroundCheck.done'));
    const preocupationalDone = this.getControlValue(form.get('preocupational.done'));
    stage.id = this.getControlValue(form.controls.id);
    stage.date = this.getControlValue(form.controls.date);
    stage.dni = this.getControlValue(form.controls.dni) || 0;
    stage.feedback = this.getControlValue(form.controls.feedback);
    stage.status = this.getControlValue(form.controls.status);
    stage.userOwnerId = this.getControlValue(form.controls.userOwnerId);
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.processId = processId;
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.seniority = this.getControlValue(form.controls.seniority);
    stage.remunerationOffer = this.getControlValue(form.controls.remunerationOffer);
    stage.notes = this.getControlValue(form.controls.notes);
    stage.firstday = this.getControlValue(form.controls.firstday);
    stage.bonus = this.getControlValue(form.controls.bonus);
    stage.hireDate = this.getControlValue(form.controls.hireDate);

    if (backgroundCheckDone === null || backgroundCheckDone === this.checkedEnum.NA){
      stage.backgroundCheckDone = null;
      stage.backgroundCheckDoneDate = null;
    } else {
      stage.backgroundCheckDone = backgroundCheckDone;
      stage.backgroundCheckDoneDate = this.getControlValue(form.get('backgroundCheck.date'));
    }
    if (preocupationalDone === null || preocupationalDone === this.checkedEnum.NA){
      stage.preocupationalDone = null;
      stage.preocupationalDoneDate = null;
    } else {
      stage.preocupationalDone = preocupationalDone;
      stage.preocupationalDoneDate = this.getControlValue(form.get('preocupational.date'));
    }
    stage.rejectionReason = this.getControlValue(form.controls.rejectionReason);
    stage.readdressStatus = this.readdressStatus;
    return stage;
  }

  getControlValue(control: any): any {
    return (control === null ? null : control.value);
  }

  fillForm(preOfferStage: PreOfferStage) {
    if (this.currentStageStatus === StageStatusEnum.InProgress || this.currentStageStatus === StageStatusEnum.PendingReply) {
      this.changeFormStatus(true);
    }

    this.preOfferForm.controls['status'].setValue(preOfferStage.status as number);

    if (preOfferStage.id) {
      this.preOfferForm.controls['id'].setValue(preOfferStage.id);
    }

    if (preOfferStage.dni !== 0) {
      this.preOfferForm.controls['dni'].setValue(preOfferStage.dni);
    }

    if (preOfferStage.remunerationOffer) {
      this.preOfferForm.controls['remunerationOffer'].setValue(preOfferStage.remunerationOffer);
    }

    if (preOfferStage.notes) {
      this.preOfferForm.controls['notes'].setValue(preOfferStage.notes);
    }

    if (this.preOfferStage.id !== 0 && preOfferStage.date) {
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

    if (preOfferStage.backgroundCheckDoneDate) {
      const backgroundCheck: number = +preOfferStage.backgroundCheckDone;
      this.preOfferForm.get('backgroundCheck.done').setValue(backgroundCheck);
      this.preOfferForm.get('backgroundCheck.date').setValue(preOfferStage.backgroundCheckDoneDate);
    }
    
    if (preOfferStage.preocupationalDoneDate) {
      const preocupational: number = +preOfferStage.preocupationalDone;
      this.preOfferForm.get('preocupational.done').setValue(preocupational);
      this.preOfferForm.get('preocupational.date').setValue(preOfferStage.preocupationalDoneDate);
    }

    if (preOfferStage.rejectionReason) {
      this.preOfferForm.controls['rejectionReason'].setValue(preOfferStage.rejectionReason);
    }

    if (preOfferStage.readdressStatus) {
      if (preOfferStage.readdressStatus.feedback)
        this.preOfferForm.controls['reasonDescriptionTextAreaControl'].setValue(preOfferStage.readdressStatus.feedback);
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


  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.preOfferForm);
  }

  validatorsOnReaddressControls(flag: boolean) {
    let reasonSelectControl = this.preOfferForm.controls['reasonSelectControl'];
    let feedbackTextAreaControl = this.preOfferForm.controls['reasonDescriptionTextAreaControl'];

    function enableValidations() {
      reasonSelectControl.setValidators(Validators.required);
      feedbackTextAreaControl.setValidators([Validators.required]);
    }

    function disableValidations() {
      reasonSelectControl.clearValidators();
      feedbackTextAreaControl.clearValidators();
    }

    flag == true ? enableValidations() : disableValidations();

  }

  checkLength(field) {
    let fieldName = field.attributes.id.nodeValue,
      maxLength = Number(field.attributes.maxlength.nodeValue) || 8,
      inputValue = this.preOfferForm.controls[fieldName].value;

    if (inputValue == null) { return }

    if (inputValue.toString().length > maxLength) {
      this.preOfferForm.controls[fieldName].setValue(inputValue.toString().replace(".", "").substring(0, maxLength));
    }
  }

  CanShowReaddressPossibility() {
    if (CanShowReaddressPossibility(this.currentStageStatus)) {
      this.validatorsOnReaddressControls(true);
      return true;
    }
    else {
      this.validatorsOnReaddressControls(false);
      return false;
    }
  }

  getSelectedReason(reason) {
    this.selectedReasonId = reason;
    this.readdressStatus.readdressReasonId = this.selectedReasonId;
  }

  onDescriptionChange(description: string): void {
    this.readdressStatus.feedback = description;
  }

  disabledBirthDates(current: Date) {
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    current.setHours(0, 0, 0, 0);
    if(current > today){
      return true;
    } else{
      return false;
    }
  }

  setFieldValidations() {
    const dniControl = this.preOfferForm.controls['dni'];
    const birthDateControl = this.preOfferForm.controls["date"];
    const statusControl = this.preOfferForm.controls['status'];
    statusControl.valueChanges.subscribe(status => {
        if (status === StageStatusEnum.Accepted) {
          dniControl.setValidators([Validators.required, dniValidator]);
          birthDateControl.setValidators([Validators.required]);
        }
        else {
          dniControl.setValidators([dniValidator]);
          birthDateControl.setValidators([]);
        }
        dniControl.updateValueAndValidity();
        birthDateControl.updateValueAndValidity();
      });
  }

  cleanDate(formGroupName: string){
    const control = this.preOfferForm.get(formGroupName);
    if (control.get('done').value === this.checkedEnum.NA) {
      control.get('date').setValue(null);
    }
  }
}
