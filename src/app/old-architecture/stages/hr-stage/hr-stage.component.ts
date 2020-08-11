import { ChangeDetectionStrategy, Component, Input, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EnglishLevelEnum } from '@shared/enums/english-level.enum';
import { StageStatusEnum } from '@shared/enums/stage-status.enum';
import { HrStage } from '@shared/models/hr-stage.model';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
import { ReaddressReason } from '@shared/models/readdress-reason.model';
import { ReaddressStatus } from '@shared/models/readdress-status.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { Globals } from '@shared/utils/globals';
import { CanShowReaddressPossibility, formFieldHasRequiredValidator } from '@shared/utils/utils.functions';
import { AppComponent } from '@app/app.component';
import { resizeModal } from '@app/shared/utils/resize-modal.util';
@Component({
  selector: 'hr-stage',
  templateUrl: './hr-stage.component.html',
  styleUrls: ['./hr-stage.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HrStageComponent implements OnInit {

  disabled = false;

  @Input()
  private _users: User[];
  public get users(): User[] {
    return this._users;
  }
  public set users(value: User[]) {
    this._users = value;
  }

  hrForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    actualSalary: [null],
    wantedSalary: [null],
    userOwnerId: [],
    userDelegateId: [null],
    feedback: [null, []],
    additionalInformation: [null],
    englishLevel: EnglishLevelEnum.None,
    rejectionReason: [null],
    rejectionReasonsHr: [0],
    sentEmail: [false],
    reasonSelectControl: [null],
    reasonDescriptionTextAreaControl: [null]
  });

  feedbackContent: string = "";

  statusList: any[];
  englishLevelList: any[];

  currentStageStatus: StageStatusEnum;
  usersFiltered: User[];
  readdressStatus: ReaddressStatus = new ReaddressStatus();

  @Input() hrStage: HrStage;
  @Input() readdressReasonList: ReaddressReason[] = [];
  @Input() readdressReasonTypeList: ReaddressReasonType[] = [];

  readdressFilteredList: ReaddressReason[] = [];
  selectedReasonId: number;
  selectedReason: string;
  constructor(private fb: FormBuilder, private facade: FacadeService,
    private globals: Globals, private _appComponent: AppComponent) {

    this.statusList = globals.hrStageStatusList;
    this.englishLevelList = globals.englishLevelList;
  }

  ngOnInit() {
    this.currentStageStatus = this.hrStage.status;
    let stageName = StageStatusEnum[this.currentStageStatus].toLowerCase();
    this.readdressFilteredList = this.readdressReasonList?.filter((reason) => { return reason.type.toLowerCase() == stageName });

    this.selectedReason = undefined;
    this.readdressStatus.feedback = undefined;
    this.readdressStatus.fromStatus = undefined;
    this.readdressStatus.toStatus = undefined;
    this.readdressStatus.id = undefined;

    if (this.hrStage.readdressStatus) {
      this.selectedReason = `${this.hrStage.readdressStatus.readdressReasonId}`;
      this.readdressStatus.feedback = this.hrStage.readdressStatus.feedback;
      this.readdressStatus.fromStatus = this.hrStage.status;
      this.readdressStatus.toStatus = this.hrStage.readdressStatus.toStatus;
      this.readdressStatus.id = this.hrStage.readdressStatus.id
    }

    this.changeFormStatus(false);
    if (this.hrStage) { this.fillForm(this.hrStage); }
    this.getFilteredUsersForHr();
  }

  getFilteredUsersForHr() {
    this.facade.userService.getFilteredForHr()
      .subscribe(res => {
        this.usersFiltered = res.sort((a, b) => ((a.firstName + ' ' + a.lastName).localeCompare(b.firstName + ' ' + b.lastName)));
      }, err => {
        console.log(err);
      });
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

    this.hrForm.controls['reasonSelectControl'].setValue(undefined);
    this.hrForm.controls['reasonSelectControl'].enable();
    this.hrForm.controls['reasonDescriptionTextAreaControl'].enable();
  }

  statusChanged() {
    this.readdressStatus.readdressReasonId = undefined;
    this.readdressStatus.feedback = undefined;
    this.hrForm.controls['reasonDescriptionTextAreaControl'].setValue("");
    this.currentStageStatus = this.hrForm.controls['status'].value;

    if (this.currentStageStatus === StageStatusEnum.InProgress) {
      this.changeFormStatus(true);
      this.hrForm.markAsTouched();
    }
    else {
      this.changeFormStatus(false);
    }

    let stageName = StageStatusEnum[this.currentStageStatus].toLowerCase();
    this.readdressFilteredList = this.readdressReasonList.filter((reason) => { return reason.type.toLowerCase() == stageName });
    this.readdressStatus.toStatus = this.currentStageStatus;

    //Temporal fix to make modal reize when the form creates new items dinamically that exceeds the height of the modal.
    resizeModal();
  }

  getFormData(processId: number): HrStage {
    const hrStage: HrStage = new HrStage();

    hrStage.id = this.getControlValue(this.hrForm.controls.id);
    hrStage.date = this.getControlValue(this.hrForm.controls.date);
    hrStage.feedback = this.feedbackContent;
    hrStage.status = this.getControlValue(this.hrForm.controls.status);
    hrStage.userOwnerId = this.getControlValue(this.hrForm.controls.userOwnerId);
    hrStage.userDelegateId = this.getControlValue(this.hrForm.controls.userDelegateId);
    hrStage.processId = processId;
    hrStage.englishLevel = this.getControlValue(this.hrForm.controls.englishLevel);
    hrStage.actualSalary = this.getControlValue(this.hrForm.controls.actualSalary) == null ? 0 : this.getControlValue(this.hrForm.controls.actualSalary);
    hrStage.wantedSalary = this.getControlValue(this.hrForm.controls.wantedSalary) == null ? 0 : this.getControlValue(this.hrForm.controls.wantedSalary);
    hrStage.additionalInformation = this.getControlValue(this.hrForm.controls.additionalInformation);
    hrStage.userDelegateId = this.getControlValue(this.hrForm.controls.userDelegateId);
    hrStage.rejectionReason = this.getControlValue(this.hrForm.controls.rejectionReason);
    hrStage.rejectionReasonsHr = this.getControlValue(this.hrForm.controls.rejectionReasonsHr);
    hrStage.sentEmail = this.getControlValue(this.hrForm.controls.sentEmail);
    hrStage.readdressStatus = this.readdressStatus;
    return hrStage;
  }

  getFeedbackContent(content: string): void {
    this.feedbackContent = content;
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

    if (hrStage.userOwnerId) {
      this.hrForm.controls['userOwnerId'].setValue(hrStage.userOwnerId);
    }

    if (hrStage.userDelegateId) {
      this.hrForm.controls['userDelegateId'].setValue(hrStage.userDelegateId);
    }

    if (hrStage.feedback) {
      this.feedbackContent = hrStage.feedback;
    }

    if (hrStage.actualSalary !== null) {
      this.hrForm.controls['actualSalary'].setValue(hrStage.actualSalary);
    }

    if (hrStage.wantedSalary !== null) {
      this.hrForm.controls['wantedSalary'].setValue(hrStage.wantedSalary);
    }

    if (hrStage.additionalInformation !== null) {
      this.hrForm.controls['additionalInformation'].setValue(hrStage.additionalInformation);
    }

    if (hrStage.englishLevel) {
      this.hrForm.controls['englishLevel'].setValue(hrStage.englishLevel);
    }

    if (hrStage.rejectionReason) {
      this.hrForm.controls['rejectionReason'].setValue(hrStage.rejectionReason);
    }
    if (hrStage.sentEmail) {
      this.hrForm.controls['sentEmail'].setValue(hrStage.sentEmail);
    }
    if (hrStage.readdressStatus) {
      if (hrStage.readdressStatus.feedback)
        this.hrForm.controls['reasonDescriptionTextAreaControl'].setValue(hrStage.readdressStatus.feedback);
    }
  }


  isUserRole(roles: string[]): boolean {
    return this._appComponent.isUserRole(roles);
  }

  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.hrForm);
  }

  validatorsOnReaddressControls(flag: boolean) {
    let reasonSelectControl = this.hrForm.controls['reasonSelectControl'];
    let feedbackTextAreaControl = this.hrForm.controls['reasonDescriptionTextAreaControl'];

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
}
