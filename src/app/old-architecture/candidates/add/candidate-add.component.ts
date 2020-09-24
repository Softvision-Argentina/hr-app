import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidationErrors, Validators, ValidatorFn } from '@angular/forms';
import { CandidateStatusEnum } from '@app/shared/enums/candidate-status.enum';
import { formFieldHasRequiredValidator } from '@shared/utils/utils.functions';
import { EnglishLevelEnum } from '@shared/enums/english-level.enum';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Candidate } from '@shared/models/candidate.model';
import { Community } from '@shared/models/community.model';
import { Office } from '@shared/models/office.model';
import { Process } from '@shared/models/process.model';
import { Skill } from '@shared/models/skill.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { UniqueEmailValidator, checkIfEmailAndPhoneNulll } from '@shared/utils/email.validator';
import { Globals } from '@shared/utils/globals';
import { Subscription } from 'rxjs';
import { AppComponent } from '@app/app.component';
import { ReferralsService } from '@shared/services/referrals.service';
import { customEmailAndPhoneNumberValidator } from '@app/shared/utils/forms.validators';

@Component({
  selector: 'candidate-add',
  templateUrl: './candidate-add.component.html',
  styleUrls: ['./candidate-add.component.scss']
})
export class CandidateAddComponent implements OnInit, OnDestroy {
  candidateInfo: Candidate;

  @Input()
    private _process: Process;
    public get process(): Process {
        return this._process;
    }
    public set process(value: Process) {
        this._process = value;
    }

    @Input()
    private _users: User[];
    public get users(): User[] {
        return this._users;
    }
    public set users(value: User[]) {
        this.fillUsers = value;
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
  sourceArray = [
      {name: 'Linkedin'},
      {name: 'Instagram'},
      {name: 'Facebook'},
      {name: 'Twitter'},
      {name: 'Event / Meetup'},
      {name: 'Mailing'},
      {name: 'Indeed/ Glassdoor'},
      {name: 'A friend / colleague'},
      {name: 'Online Ad'},
      {name: 'Other'}
    ];
  fillCandidate: Candidate;
  fillUsers: User[] = [];
  comms: Community[] =[];
  isInputSelected = false;
  profiles: CandidateProfile[] = [];
  availableProfiles: CandidateProfile[] = [];
  @Input() _offices: Office[] = [];
  currentUser: User;
  naProfile : CandidateProfile; 
  candidateForm: FormGroup = this.fb.group({
    name: [null, [Validators.required, Validators.pattern(/^[a-zA-Z\s]*$/)] ],
    lastName: [null, [Validators.required, Validators.pattern(/^[a-zA-Z\s]*$/)] ],
    dni: [0],
    email: [null,
      {
        validators: [Validators.email],
        updateOn: 'blur'
      }
    ],

    phoneNumberPrefix: ['+54'],
    phoneNumber: [null, [Validators.pattern(/^\+?[1-9]\d{9,11}$/), Validators.minLength(10), Validators.maxLength(12)]],
    linkedin: [null],
    user: [null, [Validators.required]],
    preferredOffice: [null, [Validators.required]],
    englishLevel: 'none',
    status: CandidateStatusEnum.New,
    contacDay : [null],
    profile: [null],
    community: [null, [Validators.required]],
    isReferred: [null],
    cv: [null],
    knownFrom: [null],
    referredBy: [null],
    source: [null]
  }, { validator: customEmailAndPhoneNumberValidator });

  controlArray: Array<{ id: number, controlInstance: string[] }> = [];
  skills: Skill[] = [];
  isEdit: boolean = false;

  statusList: any[];
  candidates: Candidate[] = [];
  candidateSubscriptions: Subscription[] = [];
  selectedValue = 3;
  selectedId = '';

  constructor(private fb: FormBuilder, private facade: FacadeService, private app: AppComponent,
              private globals: Globals, private _referralsService: ReferralsService) {
                this.statusList = globals.candidateStatusList;
                this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
                this.getCandidates();
  }

  ngOnInit() {
    this._referralsService._candidateInfoSource.subscribe(info => this.candidateInfo = info);
    this.fillUsers = this._users;
    this.fillUsers.sort((a,b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)))
    this._offices.sort((a,b) => (a.name.localeCompare(b.name)))
    this.comms = this._communities;
    this.comms.sort((a,b) => (a.name.localeCompare(b.name)));
    this.profiles = this._candidateProfiles;
    this.profiles.sort((a, b) => (a.name.localeCompare(b.name)));
    this.isEdit = this._process.id !== 0;
    this.setRecruiter();
    if (this.isEdit) {
      this.fillCandidate = this._candidate;
      this.fillCandidateForm(this._process.candidate);
      this.candidateForm.get('email').setAsyncValidators(UniqueEmailValidator(this.facade.candidateService, this._process.candidate.id));
      this.changeFormStatus(false);
      this.enableEmptyFields();
    } else if (!!this._candidate.id) {
      this.fillCandidateForm(this._candidate);
    }
    this.candidateForm.get('email').setAsyncValidators(UniqueEmailValidator(this.facade.candidateService));
    setTimeout(() => {
      if (this.candidateInfo) {
        this.fillCandidateForm(this.candidateInfo);
      }
    });
  }

  onCheckAndSave(): boolean {
    if (this.candidateForm.invalid) {
      this.checkForm();
      return false;
    }
    else {
      return true;
    }
  }

  setRecruiter() {
    const currentRecruiter = this.fillUsers.find(user => user.username === this.currentUser.username);
    if(!!currentRecruiter) {
      this.candidateForm.controls['user'].setValue(currentRecruiter.id);
    }
  }
  
  checkForm() {
    for (const i in this.candidateForm.controls) {
      this.candidateForm.controls[i].markAsDirty();
      this.candidateForm.controls[i].updateValueAndValidity();
    }
  }

  getCandidates(){
    const candidateSub = this.facade.candidateService.getData()
    .subscribe(res => {
      this.candidates = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.candidateSubscriptions.push(candidateSub);
  }

  changeFormStatus(enable: boolean) {
    for (const i in this.candidateForm.controls) {
      if ((this.candidateForm.controls[i] != this.candidateForm.controls['dni'])){
        if (enable){
          this.candidateForm.controls[i].enable();
        }else{
          this.candidateForm.controls[i].disable();
        }
      }
    }
  }

  checkID(id:number) {
    this.facade.processService.getActiveProcessByCandidate(id)
      .subscribe((res: Process[]) => {
        if (res.length > 0) {
          this.facade.modalService.confirm({
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
        }
        else {
          this.candidateForm.reset();
          return false;
        }
      })
  }

  fillCandidateForm(candidate: Candidate) {
    this.availableProfiles = this.profiles.filter(profile => profile.id === candidate.profile?.id);
    this.candidateForm.controls['dni'].setValue(candidate.dni);
    this.candidateForm.controls['name'].setValue(candidate.name);
    this.candidateForm.controls['lastName'].setValue(candidate.lastName);
    this.candidateForm.controls['email'].setValue(candidate.emailAddress);
    this.candidateForm.controls['linkedin'].setValue(candidate.linkedInProfile);
    this.candidateForm.controls['phoneNumberPrefix'].setValue(candidate.phoneNumber.substring(1, candidate.phoneNumber.indexOf(')')));
    this.candidateForm.controls['phoneNumber'].setValue(candidate.phoneNumber.split(')')[1]);
    candidate.user ? this.candidateForm.controls['user'].setValue(candidate.user.id) : null;
    this.candidateForm.controls['preferredOffice'].setValue(candidate.preferredOfficeId);
    this.candidateForm.controls['status'].setValue(candidate.status);
    this.candidateForm.controls['community'].setValue(candidate.community.id);
    this.candidateForm.controls['isReferred'].setValue(candidate.isReferred);
    this.candidateForm.controls['referredBy'].setValue(candidate.referredBy);
    this.candidateForm.controls['cv'].setValue(candidate.cv);
    this.candidateForm.controls['knownFrom'].setValue(candidate.knownFrom);
    this.candidateForm.controls['source'].setValue(candidate.source);
    this.getProfile();
    candidate.profile ? this.candidateForm.controls['profile'].setValue(candidate.profile.id) : null;
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

  getProfile(){
    const communityId = this.candidateForm.get('community').value;
    const communityIndex = this.communities.findIndex(community => community.id === communityId);
    this.candidateForm.get('profile').setValue(null);
    this.availableProfiles = [];
    if (!this.communities[communityIndex].profiles){
      const profileCommunitySub = this.facade.candidateProfileService.getProfileByCommunity(communityId)
      .subscribe(data => {
        let profiles: CandidateProfile[] = [];
        if (data.length > 0){
          for (const algo in data){
            profiles.push(data[algo].profile);
          }
          this.communities[communityIndex].profiles = profiles;
          this.availableProfiles = profiles;
          this.addNaProfileToProfileList();
          this.facade.communityService.data.next(this.communities);
        } else {
          this.availableProfiles = this.profiles;
          this.addNaProfileToProfileList();

        }
      });
      this.candidateSubscriptions.push(profileCommunitySub);
    } else{
      this.availableProfiles = this.communities[communityIndex].profiles;
      this.addNaProfileToProfileList();

    }
  }
  setCurrentProfile(){
    const profileId = this.candidateForm.get('profile').value;
    this.facade.candidateProfileService.currentCandidateProfileId.next(profileId);
  }

  getFormControl(name: string): AbstractControl {
    return this.candidateForm.controls[name];
  }

  getFormData(): Candidate {
    let pn = this.candidateForm.controls['phoneNumber'].value == undefined
    || this.candidateForm.controls['phoneNumber'].value == null ? ''
    : this.candidateForm.controls['phoneNumber'].value.toString();


    let prefix = this.candidateForm.controls['phoneNumberPrefix'].value == undefined
    || this.candidateForm.controls['phoneNumberPrefix'].value == null ? ''
    : '(' + this.candidateForm.controls['phoneNumberPrefix'].value.toString() + ')';

    let newCandidate: Candidate = {
      id: !this.isEdit ? this._candidate.id : this._process.candidate.id,
      name: this.candidateForm.controls['name'].value === null ? null : this.candidateForm.controls['name'].value.toString(),
      lastName: this.candidateForm.controls['lastName'].value === null ? null : this.candidateForm.controls['lastName'].value.toString(),
      dni: this.candidateForm.controls['dni'].value === null ? 0 : this.candidateForm.controls['dni'].value,
      emailAddress: this.candidateForm.controls['email'].value === null ? null : this.candidateForm.controls['email'].value.toString(),
      phoneNumber: prefix + pn,
      linkedInProfile: this.candidateForm.controls['linkedin'].value === null ? null : this.candidateForm.controls['linkedin'].value.toString(),
      candidateSkills: null,
      englishLevel: EnglishLevelEnum.None,
      status: this.candidateForm.controls['status'].value === null ? null : this.candidateForm.controls['status'].value,
      user: !this.candidateForm.controls['user'].value ? null : new User(this.candidateForm.controls['user'].value, null),
      preferredOfficeId: this.candidateForm.controls['preferredOffice'].value === null ? null : this.candidateForm.controls['preferredOffice'].value,
      contactDay: new Date(),
      profile: this.candidateForm.controls['profile'].value === null ||  this.candidateForm.controls['profile'].value === this.naProfile.id ? null:new CandidateProfile(this.candidateForm.controls['profile'].value),
      community: this.candidateForm.controls['community'].value===null?null: new Community(this.candidateForm.controls['community'].value),
      isReferred: this.candidateForm.controls['isReferred'].value === null?false:this.candidateForm.controls['community'].value,
      cv: this.candidateForm.controls['cv'].value===null?null:this.candidateForm.controls['cv'].value,
      knownFrom: this.candidateForm.controls['knownFrom'].value===null?null:this.candidateForm.controls['knownFrom'].value,
      referredBy: !this.candidateForm.controls['referredBy'].value ? null : this.candidateForm.controls['referredBy'].value,
      source: !this.candidateForm.controls['source'].value ? null : this.candidateForm.controls['source'].value
    }
    return newCandidate;
  }

  isRequiredField(field: string) {
    return formFieldHasRequiredValidator(field, this.candidateForm)
  }

  ngOnDestroy() {
    this.candidateSubscriptions.forEach(sub => sub.unsubscribe());
  }

  enableEmptyFields() {
    let phoneNumberControl = this.candidateForm.controls["phoneNumber"];
    let emailControl = this.candidateForm.controls["email"];

    if(phoneNumberControl.value === null || phoneNumberControl.value === '') {
      phoneNumberControl.enable();
    }
    if(emailControl.value === null) {
      emailControl.enable();
    }
    
    this.candidateForm.controls["preferredOffice"].enable();
    this.candidateForm.controls["linkedin"].enable();
    this.candidateForm.controls["profile"].enable();
    this.candidateForm.controls["source"].enable();
  }

  addNaProfileToProfileList(){
    if( !this.availableProfiles.some(x=>x.name === 'N/A')){
    this.naProfile = new CandidateProfile();
    let maxId = Math.max.apply(null, this.availableProfiles.map(i => i.id))
    this.naProfile.id= this.availableProfiles.length > 0 ? ++ maxId : 1;
    this.naProfile.name = 'N/A';
    this.naProfile.description = 'Not Applicable';
    this.availableProfiles.unshift(this.naProfile);
    }
  }

}
