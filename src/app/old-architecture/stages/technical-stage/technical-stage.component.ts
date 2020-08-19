import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, OnDestroy } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { EnglishLevelEnum } from '@shared/enums/english-level.enum';
import { StageStatusEnum } from '@shared/enums/stage-status.enum';
import { CandidateSkill } from '@shared/models/candidate-skill.model';
import { Candidate } from '@shared/models/candidate.model';
import { Process } from '@shared/models/process.model';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
import { ReaddressReason } from '@shared/models/readdress-reason.model';
import { ReaddressStatus } from '@shared/models/readdress-status.model';
import { Skill } from '@shared/models/skill.model';
import { TechnicalStage } from '@shared/models/technical-stage.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { ProcessService } from '@shared/services/process.service';
import { Globals } from '@shared/utils/globals';
import { CanShowReaddressPossibility, formFieldHasRequiredValidator } from '@shared/utils/utils.functions';
import { resizeModal } from '@app/shared/utils/resize-modal.util';
import { Subscription } from 'rxjs';

@Component({
  selector: 'technical-stage',
  templateUrl: './technical-stage.component.html',
  styleUrls: ['./technical-stage.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TechnicalStageComponent implements OnInit, OnDestroy {

  @Input()
  private _users: User[];
  public get users(): User[] {
    return this._users;
  }
  public set users(value: User[]) {
    this._users = value;
  }

  @Input()
  private _process: Process;
  public get process(): Process {
    return this._process;
  }
  public set process(value: Process) {
    this._process = value;
  }

  currentStageStatus: StageStatusEnum;
  @Input() readdressReasonList: ReaddressReason[] = [];
  @Input() readdressReasonTypeList: ReaddressReasonType[] = [];

  @Input() technicalStage: TechnicalStage;

  @Output() selectedSeniority = new EventEmitter();

  technicalForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    seniority: [null],
    alternativeSeniority: [null],
    userOwnerId: [null, [Validators.required]],
    userDelegateId: [null],
    feedback: [null],
    englishLevel: EnglishLevelEnum.None,
    client: [null, Validators.pattern(/^[a-zA-Z0-9\s]*$/)],
    rejectionReason: [null],
    sentEmail: [false],
    reasonSelectControl: [null],
    reasonDescriptionTextAreaControl: [null]
  });

  feedbackContent: string = "";

  controlArray: Array<{ id: number, controlInstance: string[] }> = [];
  skills: Skill[] = [];
  usedSkills: Skill[] = [];
  private completeSkillList: Skill[] = [];

  statusList: any[];
  disabled: boolean;

  seniorityList: any[];

  selectedSeniorities: any[2];
  usersFiltered: User[];

  englishLevelList: any[];
  disabledSeniority = false;
  chosenSeniority: number;
  ownerId: number;

  readdressStatus: ReaddressStatus = new ReaddressStatus();

  currentReaddressDescription: string = "";
  readdressFilteredList: ReaddressReason[] = [];
  selectedReasonId: number;
  selectedReason: string;
  subscriptions: Subscription[] = [];

  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals, private processService: ProcessService) {
    this.statusList = globals.stageStatusList.filter(x => x.id !== StageStatusEnum.Hired);
    this.seniorityList = globals.seniorityList;
    this.englishLevelList = globals.englishLevelList;
  }

  ngOnInit() {
    this.currentStageStatus = this.technicalStage.status;
    let stageName = StageStatusEnum[this.currentStageStatus].toLowerCase();
    this.readdressFilteredList = this.readdressReasonList?.filter((reason) => { return reason.type.toLowerCase() == stageName });

    this.selectedReason = undefined;
    this.readdressStatus.feedback = undefined;
    this.readdressStatus.fromStatus = undefined;
    this.readdressStatus.toStatus = undefined;
    this.readdressStatus.id = undefined;

    if (this.technicalStage.readdressStatus) {
      this.selectedReason = `${this.technicalStage.readdressStatus.readdressReasonId}`;
      this.readdressStatus.feedback = this.technicalStage.readdressStatus.feedback;
      this.readdressStatus.fromStatus = this.technicalStage.status;
      this.readdressStatus.toStatus = this.technicalStage.readdressStatus.toStatus;
      this.readdressStatus.id = this.technicalStage.readdressStatus.id
    }

    this.processService.selectedSeniorities.subscribe(sr => this.selectedSeniorities = sr);
    this.getSkills();
    this.changeFormStatus(false);
    this.getFilteredUsersForTech();
  }

  getFeedbackContent(content: string): void {
    this.feedbackContent = content;
  }

  getFilteredUsersForTech() {
    const techSubscription = this.facade.userService.getFilteredForTech()
      .subscribe(res => {
        this.usersFiltered = res.sort((a, b) => ((a.firstName + ' ' + a.lastName).localeCompare(b.firstName + ' ' + b.lastName)));
        if (this.technicalStage) { this.fillForm(this.technicalStage, this._process.candidate); }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
    this.subscriptions.push(techSubscription);
  }

    updateSeniority(seniorityId: number) {
      this.technicalForm.controls['alternativeSeniority'].enable();
      this.technicalForm.controls["alternativeSeniority"].setValue(null);
      if (this.chosenSeniority) {
        if (this.chosenSeniority !== seniorityId + 1 && this.chosenSeniority !== seniorityId - 1) {
          this.technicalForm.controls['alternativeSeniority'].setValue(0);
        }
      }
      this.chosenSeniority = seniorityId;
      this.selectedSeniority.emit(seniorityId);
      this.selectedSeniorities = [];
      if (seniorityId !== this.seniorityList.find(
        s => s.id === this.technicalForm.controls['alternativeSeniority'].value).id) {
        this.selectedSeniorities[0] = this.seniorityList.find(s => s.id === seniorityId);
        // N/A should be shown in offer stage?
        this.selectedSeniorities[1] = this.seniorityList.find(s => s.id === this.technicalForm.controls['alternativeSeniority'].value);
      } else {
        this.selectedSeniorities[0] = this.seniorityList.find(s => s.id === seniorityId);
      }
      this.processService.changeSeniority(this.selectedSeniorities);
  }

  updateAlternativeSeniority(seniorityId) {
    this.selectedSeniority.emit(seniorityId);
    this.selectedSeniorities = [];
    if (seniorityId !== this.seniorityList.find(
      s => s.id === this.technicalForm.controls['seniority'].value).id) {
      this.selectedSeniorities[0] = this.seniorityList.find(s => s.id === this.technicalForm.controls['seniority'].value);
      // N/A should be shown in offer stage?
      this.selectedSeniorities[1] = this.seniorityList.find(s => s.id === seniorityId);
    } else {
      this.selectedSeniorities[0] = this.seniorityList.find(s => s.id === seniorityId);
    }
    this.processService.changeSeniority(this.selectedSeniorities);
  }

  getFormControl(name: string): AbstractControl {
    return this.technicalForm.controls[name];
  }


  changeFormStatus(enable: boolean) {
    for (const i in this.technicalForm.controls) {
      if (this.technicalForm.controls[i] !== this.technicalForm.controls['status']) {
        if (enable) {
          this.technicalForm.controls[i].enable();
          if (this.technicalForm.controls[i] === this.technicalForm.controls['englishLevel']) {
            this.disabled = false;
            this.technicalForm.controls[i].enable();
          }
          if (this.technicalForm.controls[i] === this.technicalForm.controls['alternativeSeniority']) {
            if (this.technicalForm.controls['seniority'].value === null) {
              this.technicalForm.controls[i].disable();
            }
          }
        } else {
          this.technicalForm.controls[i].disable();
          this.disabled = true;
        }
      }
    }

    this.technicalForm.controls['reasonSelectControl'].setValue(undefined);
    this.technicalForm.controls['reasonSelectControl'].enable();
    this.technicalForm.controls['reasonDescriptionTextAreaControl'].enable();
  }

  statusChanged(status: number) {

    //Hago este check porque este metodo (StatusChanged()) se dispara cada vez que se abre un 
    //proceso por ejecutar en el onInit el fillform() adentro del subscribe de getFilteredUsersForTech,
    // lo cual dispara toda la logica adentro aun cuando el status no sufrio ningun cambio
    
    if (status == this.currentStageStatus){
      return;
    }
    this.readdressStatus.readdressReasonId = undefined;
    this.readdressStatus.feedback = undefined;
    this.technicalForm.controls['reasonDescriptionTextAreaControl'].setValue("");
    this.currentStageStatus = this.technicalForm.controls['status'].value;

    if (this.technicalForm.controls['status'].value === 1) {
      this.changeFormStatus(true);
      this.technicalForm.markAsTouched();
    } else {
      this.changeFormStatus(false);
    }

    let stageName = StageStatusEnum[this.currentStageStatus].toLowerCase();
    this.readdressFilteredList = this.readdressReasonList.filter((reason) => { return reason.type.toLowerCase() == stageName });
    this.readdressStatus.toStatus = this.currentStageStatus;

    //Temporal fix to make modal reize when the form creates new items dinamically that exceeds the height of the modal.
    resizeModal();
  }

  getFormData(processId: number): TechnicalStage {
    const stage: TechnicalStage = new TechnicalStage();
    const form = this.technicalForm;

    stage.id = this.getControlValue(form.controls.id);
    stage.date = this.getControlValue(form.controls.date);
    stage.feedback = this.feedbackContent;
    stage.englishLevel = this.getControlValue(form.controls.englishLevel);
    stage.status = this.getControlValue(form.controls.status);
    stage.userOwnerId = this.getControlValue(form.controls.userOwnerId);
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.processId = processId;
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.seniority = this.getControlValue(form.controls.seniority) === null ? 0 : this.getControlValue(form.controls.seniority);
    stage.alternativeSeniority = this.getControlValue(form.controls.alternativeSeniority) === null ? 0 : this.getControlValue(form.controls.alternativeSeniority);
    stage.client = this.getControlValue(form.controls.client);
    stage.rejectionReason = this.getControlValue(form.controls.rejectionReason);
    stage.sentEmail = this.getControlValue(form.controls.sentEmail);
    stage.readdressStatus = this.readdressStatus;
    return stage;
  }

  getFormDataSkills(): CandidateSkill[] {
    this.removeEmptyOrInvalidSkillFields();
    const candidateSkills: CandidateSkill[] = [];
    this.controlArray.forEach(skillControl => {
      const skill: CandidateSkill = {
        candidateId: 0,
        candidate: null,
        skillId: this.technicalForm.controls[skillControl.controlInstance[0]].value,
        skill: null,
        rate: this.technicalForm.controls[skillControl.controlInstance[1]].value,
        comment: this.technicalForm.controls[skillControl.controlInstance[2]].value
      };

      candidateSkills.push(skill);
    });

    return candidateSkills;
  }

  getControlValue(control: any): any {
    return (control === null ? null : control.value);
  }

  fillForm(technicalStage: TechnicalStage, candidate: Candidate) {
    const status: number = this.statusList.filter(s => s.id === technicalStage.status)[0].id;

    if (status === StageStatusEnum.InProgress) {
      this.changeFormStatus(true);
    }

    this.technicalForm.controls['status'].setValue(status);

    if (technicalStage.id) {
      this.technicalForm.controls['id'].setValue(technicalStage.id);
    }

    if (technicalStage.date) {
      this.technicalForm.controls['date'].setValue(technicalStage.date);
    }

    if (technicalStage.userOwnerId) {
      if (technicalStage.status === StageStatusEnum.NA) {
        this.technicalForm.controls['userOwnerId'].setValue(null);
      } else {
        this.technicalForm.controls['userOwnerId'].setValue(technicalStage.userOwnerId);
      }
    }

    if (technicalStage.userDelegateId) {
      this.technicalForm.controls['userDelegateId'].setValue(technicalStage.userDelegateId);
    }

    if (technicalStage.feedback) {
      this.feedbackContent = technicalStage.feedback;
    }

    if (technicalStage.englishLevel) {
      this.technicalForm.controls['englishLevel'].setValue(technicalStage.englishLevel);
    }

    if (technicalStage.seniority) {
      this.technicalForm.controls['seniority'].setValue(technicalStage.seniority);
      this.chosenSeniority = technicalStage.seniority;
    }

    if (technicalStage.alternativeSeniority) {
      this.technicalForm.controls['alternativeSeniority'].setValue(technicalStage.alternativeSeniority);
    }

    if (technicalStage.client) {
      this.technicalForm.controls['client'].setValue(technicalStage.client);
    }

    if (technicalStage.rejectionReason) {
      this.technicalForm.controls['rejectionReason'].setValue(technicalStage.rejectionReason);
    }

    if (technicalStage.seniority !== technicalStage.alternativeSeniority) {
      this.selectedSeniorities = [
        this.seniorityList.find(s => s.id === technicalStage.seniority),
        this.seniorityList.find(s => s.id === technicalStage.alternativeSeniority)];
    } else {
      this.selectedSeniorities = [
        this.seniorityList.find(s => s.id === technicalStage.seniority)];
    }

    this.processService.changeSeniority(this.selectedSeniorities);

    if (candidate.candidateSkills.length > 0) {
      candidate.candidateSkills.forEach(skill => {
        const id = skill.skillId || skill.skill.id;

        const control = {
          id,
          controlInstance: [`skillEdit${id}`, `slidderEdit${id}`, `commentEdit${id}`]
        };
        const index = this.controlArray.push(control);
        this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[0], new FormControl(id.toString()));
        this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[1], new FormControl(skill.rate));
        this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[2], new FormControl(skill.comment, [Validators.required, Validators.pattern(/^[a-zA-Z0-9\s]*$/)]));
      });
    } else if (candidate.profile && candidate.profile.id){
      let skillCounter = 0;
      const skillsByProfile = this.facade.skillService.getSkills(candidate.profile.id)
      .subscribe(data => {
        let availableSkills: Skill[] = [];
        for (const i in data){
          const skills = this.skills.filter(skill => skill.id === data[i]?.skillId);
          if(skills.length > 0){
            availableSkills = [...availableSkills, ...skills];
          }
        }
        availableSkills = availableSkills.sort((a , b) => a.name.localeCompare(b.name));
        availableSkills.forEach(skill => {
          const id = skillCounter;

          const control = {
            id,
            controlInstance: [`skillEdit${id}`, `slidderEdit${id}`, `commentEdit${id}`]
          };
          const index = this.controlArray.push(control);
          this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[0], new FormControl(skill.id.toString()));
          this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[1], new FormControl(0));
          this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[2], new FormControl('', [Validators.required, Validators.pattern(/^[a-zA-Z0-9\s]*$/)]));
          skillCounter++;
        });
      });
      this.subscriptions.push(skillsByProfile);
      
    }
    if (technicalStage.sentEmail) {
      this.technicalForm.controls['sentEmail'].setValue(technicalStage.sentEmail);
    }

    if (technicalStage.readdressStatus)
      if (technicalStage.readdressStatus.feedback) {
        this.technicalForm.controls['reasonDescriptionTextAreaControl'].setValue(technicalStage.readdressStatus.feedback);
      }
  }

  validatorsOnReaddressControls(flag: boolean) {
    let reasonSelectControl = this.technicalForm.controls['reasonSelectControl'];
    let feedbackTextAreaControl = this.technicalForm.controls['reasonDescriptionTextAreaControl'];

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


  addField(e?: MouseEvent): void {
    if (e) {
      e.preventDefault();
    }
    const id = (this.controlArray.length > 0) ? this.controlArray[this.controlArray.length - 1].id + 1 : 0;

    const control = {
      id,
      controlInstance: [`skill${id}`, `slidder${id}`, `comment${id}`]
    };

    if (id > 0) {
      this.skills = this.skills.filter(
        s => !this.controlArray.some(
          cai => s.id === this.technicalForm.controls[cai.controlInstance[0]].value));

      for (let i = 0; i < this.controlArray.length; i++) {
        this.skills.forEach(skill => {
          const skillInForm: Number = +this.technicalForm.controls[this.controlArray[i].controlInstance[0]].value;
          if (skill.id === skillInForm) {
            this.usedSkills.push(skill);
          }
        });
      }
    }

    const index = this.controlArray.push(control);
    this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[0], new FormControl(null, Validators.required));
    this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[1], new FormControl(10));
    this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[2], new FormControl(null, [Validators.required, Validators.pattern(/^[a-zA-Z0-9\s]*$/)]));

    //Temporal fix to make modal reize when the form creates new items dinamically that exceeds the height of the modal.
    resizeModal();
  }

  updateSkills(skillControl) {
    const skillForm = this.technicalForm.controls[skillControl.controlInstance[0]];

    skillForm.valueChanges
      .subscribe(selectedValue => {
        // Remove previous skill selected from usedSkills
        this.usedSkills = this.usedSkills.filter(skill => skill.id !== +selectedValue);
        // Add skill selected to usedSkills
        this.usedSkills.push(this.skills.filter(skill => skill.id === +selectedValue)[0]);
      });
  }

  isAvailable(selectedSkill: Skill) {
    return !this.usedSkills.some(usedSkill => usedSkill.id === selectedSkill.id);
  }

  removeField(i: { id: number, controlInstance: string[] }, e: MouseEvent): void {
    e.preventDefault();
    const skillList: Skill[] = [];
    this.completeSkillList.forEach(sk => skillList.push(sk));

    if (this.controlArray.length >= 1) {

      if (this.technicalForm.controls[i.controlInstance[0]].value !== null) {
        const singleSkill = skillList.filter(skill => (skill.id === this.technicalForm.controls[i.controlInstance[0]].value)
          || (skill.id === i.id))[0];

        if (singleSkill) {
          this.skills.push(singleSkill);
          this.skills.sort((a, b) => (a.id > b.id ? 1 : -1));
        }
        this.usedSkills = this.usedSkills.filter(usedSkill => usedSkill.id !== +this.technicalForm.controls[i.controlInstance[0]].value);
      }
      let j = 0;
      const index = this.controlArray.indexOf(i);
      this.controlArray.splice(index, 1);
      for (j; j < 3; j++) { this.technicalForm.removeControl(i.controlInstance[j]); }
    }

    //Temporal fix to make modal reize when the form creates new items dinamically that exceeds the height of the modal.
    resizeModal();
  }

  getSkills() {

    const skills = this.facade.skillService.get()
      .subscribe(res => {
        this.skills = res.sort((a, b) => (a.name.localeCompare(b.name)));
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
    this.subscriptions.push(skills);
  }

  isRequiredField(field: string): boolean {
    return formFieldHasRequiredValidator(field, this.technicalForm)
  }

  isFirstSkillControl(control: { id: number, controlInstance: string[] }): boolean {
    return control.id === this.controlArray[0].id;
  }

  hideAddSkillButton(): boolean {
    return !(this.technicalForm.controls.status.value === StageStatusEnum.InProgress);
  }

  removeEmptyOrInvalidSkillFields() {
    let skillControl, skillIdField, index;

    for (index = this.controlArray.length - 1; index >= 0; index--) {
      skillControl = this.controlArray[index];
      skillIdField = this.technicalForm.controls[skillControl.controlInstance[0]];

      if (skillIdField.value === null) {
        let fakeMouseEvent = new MouseEvent('click');
        this.removeField(skillControl, fakeMouseEvent);
      }
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

  ngOnDestroy(){
    this.subscriptions.forEach(sub => {
      sub.unsubscribe();
    });
  }
}
