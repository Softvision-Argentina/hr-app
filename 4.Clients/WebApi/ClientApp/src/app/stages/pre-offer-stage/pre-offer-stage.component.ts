import { Component, OnInit, Input, EventEmitter, Output, TemplateRef, ViewChild, ChangeDetectionStrategy, OnChanges } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { User } from 'src/entities/user';
import { Globals } from '../../app-globals/globals';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { preOfferStage } from 'src/entities/pre-offer-stage';
import { ProcessService } from '../../services/process.service';
import { PreOfferHistory } from '../pre-offer-history/pre-offer-history.component';
import { dniValidator, UniqueDniValidator } from 'src/app/directives/dni.validator';
import { formFieldHasRequiredValidator, CanShowReaddressPossibility } from 'src/app/utils/utils.functions';
import { FacadeService } from 'src/app/services/facade.service';
import { ReaddressReason } from 'src/entities/ReaddressReason';
import { ReaddressReasonType } from 'src/entities/ReaddressReasonType';
import { ReaddressStatus } from 'src/entities/ReaddressStatus'

@Component({
  selector: 'pre-offer-stage',
  templateUrl: './pre-offer-stage.component.html',
  styleUrls: ['./pre-offer-stage.component.scss'],
  providers: [PreOfferHistory]
})

export class PreOfferStageComponent implements OnInit {

  @Input()
  private _users: User[];
  public get users(): User[] {
    return this._users.sort((a, b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
  }
  public set users(value: User[]) {
    this._users = value;
  }

  @Input() processId: number;

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
    notes: '', // should be removed because it's in pre-offer-history
    firstday: [new Date(), [Validators.required]],  // should be removed because it's in pre-offer-history
    bonus: '', // should be removed because it's in pre-offer-history
    hireDate: [new Date(), [Validators.required]],  // should be removed because it's in pre-offer-history
    backgroundCheckDone: false,
    backgroundCheckDoneDate: [new Date()],
    preocupationalDone: false,
    preocupationalDoneDate: [new Date()],
    rejectionReason: [null],    
    reasonSelectControl: [null],
    reasonDescriptionTextAreaControl: [null]
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
    { label: 'Declined', value: true },
    { label: 'Accepted', value: false }
  ];

  currentStageStatus: StageStatusEnum;
  readdressStatus: ReaddressStatus = new ReaddressStatus();
  currentReaddressDescription: string = "";
  readdressFilteredList: ReaddressReason[] = [];
  selectedReasonId: number;
  selectedReason: string;

  @Input() preOfferStage: preOfferStage;
  @Input() readdressReasonList: ReaddressReason[] = [];
  @Input() readdressReasonTypeList: ReaddressReasonType[] = [];
  
  @ViewChild(PreOfferHistory) preOfferHistory: PreOfferHistory ;
  @Output() selectedSeniority = new EventEmitter();
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
    this.preOfferForm.controls['dni'].setAsyncValidators(UniqueDniValidator(this.facade.processService.data.value, this.processId));
    this.currentStageStatus = this.preOfferStage.status;
    let stageName = StageStatusEnum[this.currentStageStatus].toLowerCase();
    this.readdressFilteredList = this.readdressReasonList.filter((reason) => { return reason.type.toLowerCase() == stageName });
    
    this.selectedReason = undefined;
    this.readdressStatus.feedback = undefined;
    this.readdressStatus.fromStatus = undefined;
    this.readdressStatus.toStatus = undefined;
    this.readdressStatus.id = undefined;
  
    if (this.preOfferStage.readdressStatus){
      this.selectedReason = `${this.preOfferStage.readdressStatus.readdressReasonId}`;
      this.readdressStatus.feedback = this.preOfferStage.readdressStatus.feedback;
      this.readdressStatus.fromStatus = this.preOfferStage.readdressStatus.fromStatus;
      this.readdressStatus.toStatus = this.preOfferStage.readdressStatus.toStatus;
      this.readdressStatus.id = this.preOfferStage.readdressStatus.id
    }

    this.processService.selectedSeniorities.subscribe(sr => {
      this.seniorityList = sr;
      this.preOfferForm.controls['seniority'].setValue(this.seniorityList[0].id);
    });
    this.changeFormStatus(false);
    if (this.preOfferStage) { this.fillForm(this.preOfferStage); }
    this.preOfferForm.controls['dni'].setAsyncValidators(UniqueDniValidator(this.facade.processService.data.value, this.processId));
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
    this.preOfferForm.controls['reasonSelectControl'].setValue(undefined);
    this.preOfferForm.controls['reasonSelectControl'].enable();
    this.preOfferForm.controls['reasonDescriptionTextAreaControl'].enable();
  }

  statusChanged() {

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
    this.readdressStatus.toStatus = this.currentStageStatus
  }

  getFormData(processId: number): preOfferStage {
    const stage: preOfferStage = new preOfferStage();
    const form = this.preOfferForm;

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
    stage.backgroundCheckDone = this.getControlValue(form.controls.backgroundCheckDone);
    stage.backgroundCheckDoneDate = stage.backgroundCheckDone ? this.getControlValue(form.controls.backgroundCheckDoneDate) : null;
    stage.preocupationalDone = this.getControlValue(form.controls.preocupationalDone);
    stage.preocupationalDoneDate = stage.preocupationalDone ? this.getControlValue(form.controls.preocupationalDoneDate) : null;
    stage.rejectionReason = this.getControlValue(form.controls.rejectionReason);
    stage.readdressStatus = this.readdressStatus;
    return stage;
  }

  getControlValue(control: any): any {
    return (control === null ? null : control.value);
  }

  fillForm(preOfferStage: preOfferStage) {
    const status: number = this.statusList.filter(s => s.id === preOfferStage.status)[0].id;

    if (status === StageStatusEnum.InProgress || status === StageStatusEnum.PendingReply) {
      this.changeFormStatus(true);
    }

    this.preOfferForm.controls['status'].setValue(status);

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

    if (preOfferStage.readdressStatus){
      if(preOfferStage.readdressStatus.feedback)
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

  validatorsOnReaddressControls(flag: boolean)
  {
    let reasonSelectControl = this.preOfferForm.controls['reasonSelectControl'];
    let feedbackTextAreaControl = this.preOfferForm.controls['reasonDescriptionTextAreaControl'];

    function enableValidations(){
      reasonSelectControl.setValidators(Validators.required);
      feedbackTextAreaControl.setValidators([Validators.required]);
    }

    function disableValidations(){
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
    if (CanShowReaddressPossibility(this.currentStageStatus)){
      this.validatorsOnReaddressControls(true);
      return true;
    }
    else{
      this.validatorsOnReaddressControls(false);
      return false;
    }
  }

  getSelectedReason(reason){
    this.selectedReasonId = reason;
    this.readdressStatus.readdressReasonId = this.selectedReasonId;
  }

  onDescriptionChange(description: string): void {  
    this.readdressStatus.feedback = description;
  }
}
