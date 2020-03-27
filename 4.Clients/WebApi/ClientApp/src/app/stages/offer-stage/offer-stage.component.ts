import { Component, OnInit, Input, EventEmitter, Output, TemplateRef, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { Consultant } from 'src/entities/consultant';
import { Globals } from '../../app-globals/globals';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { OfferStage } from 'src/entities/offer-stage';
import { ProcessService } from '../../services/process.service';
import { OfferHistory } from '../offer-history/offer-history.component';

@Component({
  selector: 'offer-stage',
  templateUrl: './offer-stage.component.html',
  styleUrls: ['./offer-stage.component.css'],
  providers: [OfferHistory]
})
export class OfferStageComponent implements OnInit {

  @Input()
  private _consultants: Consultant[];
  public get consultants(): Consultant[] {
      return this._consultants;
  }
  public set consultants(value: Consultant[]) {
      this._consultants = value;
  }

  offerForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    consultantOwnerId: 0,
    consultantDelegateId: [null],
    feedback: '',
    seniority: [0, [Validators.required]],
    hireDate: [new Date(), [Validators.required]],
    backgroundCheckDone: false,
    backgroundCheckDoneDate: [new Date(), [Validators.required]],
    preocupationalDone: false,
    preocupationalDoneDate: [new Date(), [Validators.required]],
    rejectionReason: [null, [Validators.required]],
  });

  statusList: any[];
  seniorityList: any[];
  backCheckEnabled = false;
  backDateEnabled: boolean;
  preocupationalCheckEnabled = false;
  preocupationalDateEnabled: boolean;

  @Input() offerStage: OfferStage;
  @ViewChild(OfferHistory) historyOffer: OfferHistory ;
  @Output() selectedSeniority = new EventEmitter();

  constructor(
    private fb: FormBuilder,
    private globals: Globals,
    private processService: ProcessService,
    private historyOfferModal: OfferHistory) {

    this.statusList = globals.stageStatusList;
  }

  showOfferHistoryModal(modalContent: TemplateRef<{}>) {
    this.historyOfferModal.showModal(modalContent);
  }

  ngOnInit() {
    this.processService.selectedSeniorities.subscribe(sr => {
      this.seniorityList = sr;
      this.offerForm.controls['seniority'].setValue(this.seniorityList[0].id);
    });
    this.changeFormStatus(false);
    if (this.offerStage) { this.fillForm(this.offerStage); }
  }

  updateSeniority(seniorityId) {
    this.selectedSeniority.emit(seniorityId);
  }

  getFormControl(name: string): AbstractControl {
    return this.offerForm.controls[name];
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
    stage.feedback = this.getControlValue(form.controls.feedback);
    stage.status = this.getControlValue(form.controls.status);
    stage.consultantOwnerId = this.getControlValue(form.controls.consultantOwnerId);
    stage.consultantDelegateId = this.getControlValue(form.controls.consultantDelegateId);
    stage.processId = processId;
    stage.consultantDelegateId = this.getControlValue(form.controls.consultantDelegateId);
    stage.seniority = this.getControlValue(form.controls.seniority);
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

    if (offerStage.consultantOwnerId) {
      this.offerForm.controls['consultantOwnerId'].setValue(offerStage.consultantOwnerId);
    }

    if (offerStage.seniority) {
      this.offerForm.controls['seniority'].setValue(offerStage.seniority);
    }

    if (offerStage.hireDate) {
      this.offerForm.controls['hireDate'].setValue(offerStage.hireDate);
    }

    if (offerStage.feedback) {
      this.offerForm.controls['feedback'].setValue(offerStage.feedback);
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

  showRejectionReason() {
    if (this.offerForm.controls['status'].value === StageStatusEnum.Rejected) {
      this.offerForm.controls['rejectionReason'].enable();
      return true;
    }
    this.offerForm.controls['rejectionReason'].disable();
    return false;
  }
}
