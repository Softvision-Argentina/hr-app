import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl, FormControl } from '@angular/forms';
import { trimValidator } from 'src/app/directives/trim.validator';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';
import { Process } from 'src/entities/process';
import { Globals } from '../../app-globals/globals';
import { SeniorityEnum } from '../../../entities/enums/seniority.enum';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { TechnicalStage } from 'src/entities/technical-stage';
import { Skill } from 'src/entities/skill';
import { Candidate } from 'src/entities/candidate';
import { CandidateSkill } from 'src/entities/candidateSkill';
import { ProcessService } from '../../services/process.service';
import { formFieldHasRequiredValidator } from 'src/app/utils/utils.functions'

@Component({
  selector: 'technical-stage',
  templateUrl: './technical-stage.component.html',
  styleUrls: ['./technical-stage.component.css']
})
export class TechnicalStageComponent implements OnInit {

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

  technicalForm: FormGroup = this.fb.group({
    id: [0],
    status: [0, [Validators.required]],
    date: [new Date(), [Validators.required]],
    seniority: [0, [Validators.required]],
    alternativeSeniority: [0, [Validators.required]],
    userOwnerId: [null, [Validators.required]],
    userDelegateId: [null],
    feedback: [null, [trimValidator]],
    client: [null],
    rejectionReason: [null, [Validators.required]]
  });

  controlArray: Array<{ id: number, controlInstance: string[] }> = [];
  skills: Skill[] = [];
  private completeSkillList: Skill[] = [];

  statusList: any[];
  disable: boolean;

  seniorityList: any[];

  selectedSeniorities: any[2];
  usersFiltered: User[];

  @Input() technicalStage: TechnicalStage;

  @Output() selectedSeniority = new EventEmitter();

  constructor(private fb: FormBuilder, private facade: FacadeService, private globals: Globals, private processService: ProcessService) {
    this.statusList = globals.stageStatusList.filter(x => x.id !== StageStatusEnum.Hired);
    this.seniorityList = globals.seniorityList;
  }

  ngOnInit() {
    this.processService.selectedSeniorities.subscribe(sr => this.selectedSeniorities = sr);

    this.getSkills();
    this.changeFormStatus(false);
    if (this.technicalStage) { this.fillForm(this.technicalStage, this._process.candidate); }
    this.getFilteredUsersForTech();
  }

  getFilteredUsersForTech() {
    this.facade.userService.getFilteredForTech()
    .subscribe(res => {
      this.usersFiltered = res.sort((a,b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
    }, err => {
      console.log(err);
    });
  }

  updateSeniority(seniorityId) {
    this.selectedSeniority.emit(seniorityId);
    this.selectedSeniorities = [];
    if (seniorityId !== this.seniorityList.find(
      s => s.id === this.technicalForm.controls['alternativeSeniority'].value).id) {
      this.selectedSeniorities[0] = this.seniorityList.find(s => s.id === seniorityId);
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
          this.disable = false;
        } else {
          this.technicalForm.controls[i].disable();
          this.disable = true;
        }
      }
    }
  }

  statusChanged() {
    if (this.technicalForm.controls['status'].value === 1) {
      this.changeFormStatus(true);
      this.technicalForm.markAsTouched();
    } else {
      this.changeFormStatus(false);
    }
  }

  getFormData(processId: number): TechnicalStage {
    const stage: TechnicalStage = new TechnicalStage();
    const form = this.technicalForm;

    stage.id = this.getControlValue(form.controls.id);
    stage.date = this.getControlValue(form.controls.date);
    stage.feedback = this.getControlValue(form.controls.feedback);
    stage.status = this.getControlValue(form.controls.status);
    stage.userOwnerId = this.getControlValue(form.controls.userOwnerId);
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.processId = processId;
    stage.userDelegateId = this.getControlValue(form.controls.userDelegateId);
    stage.seniority = this.getControlValue(form.controls.seniority);
    stage.alternativeSeniority = this.getControlValue(form.controls.alternativeSeniority);
    stage.client = this.getControlValue(form.controls.client);
    stage.rejectionReason = this.getControlValue(form.controls.rejectionReason);
    return stage;
  }

  getFormDataSkills(): CandidateSkill[] {
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
      this.technicalForm.controls['userOwnerId'].setValue(technicalStage.userOwnerId);
    }

    if (technicalStage.userDelegateId) {
      this.technicalForm.controls['userDelegateId'].setValue(technicalStage.userDelegateId);
    }

    if (technicalStage.feedback) {
      this.technicalForm.controls['feedback'].setValue(technicalStage.feedback);
    }

    if (technicalStage.seniority) {
      this.technicalForm.controls['seniority'].setValue(technicalStage.seniority);
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
        this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[2], new FormControl(skill.comment, Validators.required));
      });
    }
  }

  showRejectionReason() {
    if (this.technicalForm.controls['status'].value === StageStatusEnum.Rejected) {
      this.technicalForm.controls['rejectionReason'].enable();
      return true;
    }
    this.technicalForm.controls['rejectionReason'].disable();
    return false;
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
    }

    const index = this.controlArray.push(control);
    this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[0], new FormControl(null, Validators.required));
    this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[1], new FormControl(10));
    this.technicalForm.addControl(this.controlArray[index - 1].controlInstance[2], new FormControl(null, Validators.required));
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
      }
      let j = 0;
      const index = this.controlArray.indexOf(i);
      this.controlArray.splice(index, 1);
      for (j; j < 3; j++) { this.technicalForm.removeControl(i.controlInstance[j]); }
    }
  }

  getSkills() {
    this.facade.skillService.get()
      .subscribe(res => {
        this.skills = res.sort((a,b) => (a.name.localeCompare(b.name)));
      }, err => {
        console.log(err);
      });
  }

  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.technicalForm)
  }

}
