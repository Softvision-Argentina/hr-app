import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormControl, AbstractControl } from '@angular/forms';
import { trimValidator } from 'src/app/directives/trim.validator';
import { Consultant } from 'src/entities/consultant';
import { User } from 'src/entities/user';
import { FacadeService } from 'src/app/services/facade.service';
import { Candidate } from 'src/entities/candidate';
import { Skill } from 'src/entities/skill';
import { Process } from 'src/entities/process';
import { AppComponent } from 'src/app/app.component';
import { Globals } from '../../app-globals/globals';
import { EnglishLevelEnum } from '../../../entities/enums/english-level.enum';
import { Office } from 'src/entities/office';
import { Community } from 'src/entities/community';
import { CandidateProfile } from 'src/entities/Candidate-Profile';
import { CandidateStatusEnum } from 'src/entities/enums/candidate-status.enum';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'candidate-add',
  templateUrl: './candidate-add.component.html',
  styleUrls: ['./candidate-add.component.css']
})
export class CandidateAddComponent implements OnInit {

  @Input()
  private _process: Process;
  public get process(): Process {
    return this._process;
  }
  public set process(value: Process) {
    this._process = value;
  }

  @Input()
  private _consultants: Consultant[];
  public get consultants(): Consultant[] {
    return this._consultants;
  }
  public set consultants(value: Consultant[]) {
    this.recruiters = value;
  }

  @Input()
  private _candidate: Candidate;
  public get candidate(): Candidate {
    return this._candidate;
  }
  public set candidate(value: Candidate) {
    this.fillCandidate = value;
  }

  @Input()
  private _communities: Community[];
  public get communities(): Community[] {
    return this._communities;
  }
  public set communities(value: Community[]) {
    this.comms = value;
  }

  @Input()
  private _candidateProfiles: CandidateProfile[];
  public get candidateProfiles(): CandidateProfile[] {
    return this._candidateProfiles;
  }
  public set candidateProfiles(value: CandidateProfile[]) {
    this.profiles = value;
  }

  fillCandidate: Candidate;
  recruiters: Consultant[] = [];
  comms: Community[] = [];
  profiles: CandidateProfile[] = [];
  @Input() _offices: Office[] = [];
  currentConsultant: User;
  candidateForm: FormGroup = this.fb.group({
    name: [null, [Validators.required, trimValidator]],
    lastName: [null, [Validators.required, trimValidator]],
    dni: [null, [Validators.required, Validators.pattern('^\\d{7,8}$')]],
    email: [null, [Validators.email]],
    phoneNumberPrefix: ['+54'],
    phoneNumber: [null],
    linkedin: [null, [trimValidator]],
    additionalInformation: [null, [trimValidator]],
    recruiter: [null, [Validators.required]],
    preferredOffice: [null, [Validators.required]],
    englishLevel: 'none',
    status: CandidateStatusEnum.New,
    contacDay : [null],
    profile: [null, [Validators.required]],
    community: [null, [Validators.required]],
    isReferred: [null],
    cv: [null],
    knownFrom: [null],
    referredBy: [null]
  });
  isDniValid = false;
  isDniLoading = false;
  controlArray: Array<{ id: number, controlInstance: string[] }> = [];
  skills: Skill[] = [];
  private completeSkillList: Skill[] = [];
  isEdit = false;

  statusList: any[];

  selectedValue = 1;

  constructor(private fb: FormBuilder, private facade: FacadeService, private app: AppComponent,
    private globals: Globals) {
    this.statusList = globals.candidateStatusList;
    this.currentConsultant = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.recruiters = this._consultants;
    this.comms = this._communities;
    this.profiles = this._candidateProfiles;
    this.isEdit = this._process.id !== 0;
    this.setRecruiter();
    if (this.isEdit) {
      this.fillCandidate = this._candidate;
      this.fillCandidateForm(this._process.candidate);
      this.candidateForm.controls['dni'].disable();
      this.candidateForm.controls['additionalInformation'].disable();
      this.candidateForm.controls['linkedin'].disable();
      this.candidateForm.controls['preferredOffice'].disable();
      this.changeFormStatus(false);
    } else if (!!this._candidate.id) {
      this.fillCandidateForm(this._candidate);
    }
  }
  onCheckAndSave(): boolean {
    if (this.candidateForm.invalid) {
      this.checkForm();
      return false;
    } else {
      return true;
    }

  }
  setRecruiter() {
    const currentRecruiter = this.consultants.find(consultant => consultant.emailAddress === this.currentConsultant.email);
    if (!!currentRecruiter) {
      this.candidateForm.controls['recruiter'].setValue(currentRecruiter.id);
    } else {
      this.candidateForm.controls['recruiter'].setValue(1);
    }
  }
  checkForm() {
    for (const i in this.candidateForm.controls) {
      if (this.candidateForm.controls[i]) {
        this.candidateForm.controls[i].markAsDirty();
        this.candidateForm.controls[i].updateValueAndValidity();
      }
    }
  }


  changeFormStatus(enable: boolean) {
    for (const i in this.candidateForm.controls) {
      if ((this.candidateForm.controls[i] !== this.candidateForm.controls['dni']) &&
        (this.candidateForm.controls[i] !== this.candidateForm.controls['additionalInformation']) &&
        (this.candidateForm.controls[i] !== this.candidateForm.controls['linkedin']) &&
        (this.candidateForm.controls[i] !== this.candidateForm.controls['preferredOffice'])) {
        if (enable) { this.candidateForm.controls[i].enable(); } else { this.candidateForm.controls[i].disable(); }
      }
    }
  }

  checkID(id: number) {
    this.facade.processService.getActiveProcessByCandidate(id)
      .subscribe((res: Process[]) => {
        if (res.length > 0) {
          this.facade.modalService.confirm({
            // tslint:disable-next-line: max-line-length
            nzTitle: 'There is already another process of ' + res[0].candidate.lastName + ', ' + res[0].candidate.name + '. Do you want to open a new one ?',
            nzContent: '',
            nzOkText: 'Yes',
            nzOkType: 'danger',
            nzCancelText: 'No',
            nzOnOk: () => {
              this.fillCandidateForm(res[0].candidate);
              this.changeFormStatus(false);
            },
            nzOnCancel: () => {
              this.candidateForm.reset();
            }
          });
        } else {
          this.candidateForm.reset();
          return false;
        }
      });
  }

  fillCandidateForm(candidate: Candidate) {
    this.candidateForm.controls['dni'].setValue(candidate.dni);
    this.candidateForm.controls['name'].setValue(candidate.name);
    this.candidateForm.controls['lastName'].setValue(candidate.lastName);
    this.candidateForm.controls['email'].setValue(candidate.emailAddress);
    this.candidateForm.controls['linkedin'].setValue(candidate.linkedInProfile);
    this.candidateForm.controls['phoneNumberPrefix'].setValue(candidate.phoneNumber.substring(1, candidate.phoneNumber.indexOf(')')));
    this.candidateForm.controls['phoneNumber'].setValue(candidate.phoneNumber.split(')')[1]); // (54),1123445678
    this.candidateForm.controls['additionalInformation'].setValue(candidate.additionalInformation);
    this.candidateForm.controls['recruiter'].setValue(candidate.recruiter.id);
    this.candidateForm.controls['preferredOffice'].setValue(candidate.preferredOfficeId);
    this.candidateForm.controls['status'].setValue(candidate.status);
    this.candidateForm.controls['community'].setValue(candidate.community.id);
    this.candidateForm.controls['profile'].setValue(candidate.profile.id);
    this.candidateForm.controls['isReferred'].setValue(candidate.isReferred);
    this.candidateForm.controls['referredBy'].setValue(candidate.referredBy);
    this.candidateForm.controls['cv'].setValue(candidate.cv);
    this.candidateForm.controls['knownFrom'].setValue(candidate.knownFrom);
    if (candidate.candidateSkills.length > 0) {
      candidate.candidateSkills.forEach(skill => {
        const id = skill.skillId || skill.skill.id;

        const control = {
          id,
          controlInstance: [`skillEdit${id}`, `slidderEdit${id}`, `commentEdit${id}`]
        };

        const index = this.controlArray.push(control);
        this.candidateForm.addControl(this.controlArray[index - 1].controlInstance[0], new FormControl(id.toString()));
        this.candidateForm.addControl(this.controlArray[index - 1].controlInstance[1], new FormControl(skill.rate));
        this.candidateForm.addControl(this.controlArray[index - 1].controlInstance[2], new FormControl(skill.comment, Validators.required));
      });
    }
  }

  getFormControl(name: string): AbstractControl {
    return this.candidateForm.controls[name];
  }

  getFormData(): Candidate {
    const pn = this.candidateForm.controls['phoneNumber'].value === undefined
      || this.candidateForm.controls['phoneNumber'].value === null ? ''
      : this.candidateForm.controls['phoneNumber'].value.toString();

    const prefix = this.candidateForm.controls['phoneNumberPrefix'].value === undefined
      || this.candidateForm.controls['phoneNumberPrefix'].value === null ? ''
      : '(' + this.candidateForm.controls['phoneNumberPrefix'].value.toString() + ')';

    const newCandidate: Candidate = {
      id: !this.isEdit ? this._candidate.id : this._process.candidate.id,
      name: this.candidateForm.controls['name'].value === null ? null : this.candidateForm.controls['name'].value.toString(),
      lastName: this.candidateForm.controls['lastName'].value === null ? null : this.candidateForm.controls['lastName'].value.toString(),
      dni: this.candidateForm.controls['dni'].value === null ? null : this.candidateForm.controls['dni'].value,
      emailAddress: this.candidateForm.controls['email'].value === null ? null : this.candidateForm.controls['email'].value.toString(),
      phoneNumber: prefix + pn,
      // tslint:disable-next-line: max-line-length
      linkedInProfile: this.candidateForm.controls['linkedin'].value === null ? null : this.candidateForm.controls['linkedin'].value.toString(),
      candidateSkills: null,
      // tslint:disable-next-line: max-line-length
      additionalInformation: this.candidateForm.controls['additionalInformation'].value === null ? null : this.candidateForm.controls['additionalInformation'].value.toString(),
      englishLevel: EnglishLevelEnum.None,
      status: this.candidateForm.controls['status'].value === null ? null : this.candidateForm.controls['status'].value,
      // tslint:disable-next-line: max-line-length
      recruiter: !this.candidateForm.controls['recruiter'].value ? null : new Consultant(this.candidateForm.controls['recruiter'].value, null, null),
      // tslint:disable-next-line: max-line-length
      preferredOfficeId: this.candidateForm.controls['preferredOffice'].value === null ? null : this.candidateForm.controls['preferredOffice'].value,
      contactDay: new Date(),
      // tslint:disable-next-line: max-line-length
      profile: this.candidateForm.controls['profile'].value === null ? null : new CandidateProfile(this.candidateForm.controls['profile'].value),
      // tslint:disable-next-line: max-line-length
      community: this.candidateForm.controls['community'].value === null  ? null : new Community(this.candidateForm.controls['community'].value),
      isReferred: this.candidateForm.controls['isReferred'].value === null ? false : this.candidateForm.controls['community'].value,
      // contactDay: this.candidateForm.controls['contactDay'].value
      cv: this.candidateForm.controls['cv'].value === null ? null : this.candidateForm.controls['cv'].value,
      knownFrom: this.candidateForm.controls['knownFrom'].value === null ? null : this.candidateForm.controls['knownFrom'].value,
      referredBy: !this.candidateForm.controls['referredBy'].value ? null : this.candidateForm.controls['referredBy'].value
    };
    newCandidate.phoneNumber.toString();
    return newCandidate;
  }

}
