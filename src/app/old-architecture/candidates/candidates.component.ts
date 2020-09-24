import { Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { CandidateSkill } from '@shared/models/candidate-skill.model';
import { Candidate } from '@shared/models/candidate.model';
import { Community } from '@shared/models/community.model';
import { Office } from '@shared/models/office.model';
import { Skill } from '@shared/models/skill.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { Globals } from '@shared/utils/globals';
import { replaceAccent } from '@shared/utils/replace-accent.util';
import { Subscription } from 'rxjs';
import { AppComponent } from '@app/app.component';
import { validateCandidateForm } from './candidate-form.validator';
import { CandidateDetailsComponent } from './details/candidate-details.component';
import { dniValidator } from '@app/shared/utils/dni.validator';
import { UniqueEmailValidator } from '@app/shared/utils/email.validator';
import { resizeModal } from '@app/shared/utils/resize-modal.util';
import { SeniorityEnum } from '@shared/enums/seniority.enum';
import { CandidateInfoService } from '@shared/services/candidate-info.service';
import { Router } from '@angular/router';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { CandidateStatusEnum } from '@shared/enums/candidate-status.enum';

@Component({
  selector: 'app-candidates',
  templateUrl: './candidates.component.html',
  styleUrls: ['./candidates.component.scss'],
  providers: [CandidateDetailsComponent, AppComponent]
})

export class CandidatesComponent implements OnInit, OnDestroy {

  @ViewChild('dropdown', { static: false }) nameDropdown;
  @ViewChild('dropdownStatus') statusDropdown;
  @ViewChild('focusInput') inputFocus;
  filteredCandidates: Candidate[] = [];
  isLoadingResults = false;
  searchValue = '';
  listOfSearchCandidates: any[] = [];
  listOfDisplayData: Candidate[] = [...this.filteredCandidates];
  sortName = 'name';
  sortValue = 'ascend';
  users: User[] = [];
  profiles: CandidateProfile[] = [];
  communities: Community[] = [];
  _offices: Office[] = [];
  searchDni = '';
  searchName = '';
  searchSub: Subscription;
  candidateSubscriptions: Subscription = new Subscription();
  sourceArray = [
    { name: 'Linkedin' },
    { name: 'Instagram' },
    { name: 'Facebook' },
    { name: 'Twitter' },
    { name: 'Event / Meetup' },
    { name: 'Mailing' },
    { name: 'Indeed/ Glassdoor' },
    { name: 'A friend / colleague' },
    { name: 'Online Ad' },
    { name: 'Other' }
  ];
  // Modals
  skills: Skill[] = [];
  private completeSkillList: Skill[] = [];
  validateForm: FormGroup;
  isAddVisible = false;
  isAddOkLoading = false;
  emptyCandidate: Candidate;
  controlArray: Array<{ id: number, controlInstance: string[] }> = [];
  controlEditArray: Array<{ id: number, controlInstance: string[] }> = [];
  isEdit = false;
  editingCandidateId = 0;
  isDniLoading = false;
  currentUser: User;
  searchValueStatus = '';
  statusList: any[];
  englishLevelList: any[];
  candidateInfo : Candidate ;
  referredBy: string;
  seniorityList: any[];
  candidateStatus: CandidateStatusEnum;

  constructor(private router : Router,private facade: FacadeService, private fb: FormBuilder, private detailsModal: CandidateDetailsComponent, private globals: Globals , private _candidateInfoService : CandidateInfoService) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.statusList = globals.candidateStatusList;
    this.englishLevelList = globals.englishLevelList;
    this.seniorityList = globals.seniorityList;    
  }

  ngOnInit() {
    this._candidateInfoService._candidateInfoSource.subscribe(info => this.candidateInfo = info);
    this.facade.appService.startLoading();
    this.facade.appService.removeBgImage();
    this.getCandidates();
    this.getUsers();
    this.getProfiles();
    this.getCommunities();
    this.getOffices();
    this.getSkills();
    this.resetForm();
    this.getSearchInfo();
    this.facade.appService.stopLoading();
  }

  goToProcesses(candidate) {
    this._candidateInfoService.sendCandidateInfo(candidate);
    this.router.navigateByUrl('/processes');
}

  setFocusTrue() {
    setTimeout(() => {
      this.inputFocus.nativeElement.focus();
    }, 100);
  }

  getCandidates() {
    this.facade.candidateService.get()
      .subscribe(res => {
        this.filteredCandidates = this.currentUser.role === 'Admin' ? res : res.filter(x=>x.status !== CandidateStatusEnum.Eliminated);
        this.listOfDisplayData = res.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1)
          : (b[this.sortName] > a[this.sortName] ? 1 : -1));
          this.listOfDisplayData.forEach(x=> { x.source == null ? x.source ='N/A' : x.source});
          this.listOfDisplayData = this.currentUser.role == 'Admin' ? this.listOfDisplayData : this.listOfDisplayData.filter(x=>x.status != CandidateStatusEnum.Eliminated) ;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getUsers() {
    this.facade.userService.get()
      .subscribe(res => {
        this.users = res.sort((a, b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getProfiles() {
    const profilesSubscription = this.facade.candidateProfileService.getData().subscribe(res => {
      if (!!res) {
        this.profiles = res.sort((a, b) => (a.name.localeCompare(b.name)));
        for (let i = 0; i < this.profiles.length; i++) {
          if (this.profiles[i].name === 'N/A') {
            const NA = this.profiles.splice(i, 1);
            this.profiles.unshift(NA[0]);
          }
        }
      }
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.candidateSubscriptions.add(profilesSubscription);
  }

  getCommunities() {
    const communitiesSubscription = this.facade.communityService.getData().subscribe(res => {
      if (!!res) {
        this.communities = res.sort((a, b) => (a.name.localeCompare(b.name)));
      }
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.candidateSubscriptions.add(communitiesSubscription);
  }

  getSkills() {
    this.facade.skillService.get()
      .subscribe(res => {
        this.skills = res.sort((a, b) => (a.name).localeCompare(b.name));
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getOffices() {
    const officesSubscription = this.facade.OfficeService.getData().subscribe(res => {
      if (!!res) {
        this._offices = res.sort((a, b) => (a.name.localeCompare(b.name)));
      }
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.candidateSubscriptions.add(officesSubscription);
  }
  getSearchInfo() {
    this.searchSub = this.facade.searchbarService.searchChanged.subscribe(data => {
      if (isNaN(Number(data))) {
        this.listOfDisplayData = this.filteredCandidates;
        this.listOfDisplayData = this.listOfDisplayData.filter(candidate => {
          const fullName = candidate.name + candidate.lastName;
          const value = data.toString().toUpperCase();
          return fullName.toString().toUpperCase().indexOf(value) !== -1;
        });
      } else {
        this.listOfDisplayData = this.filteredCandidates;
        this.listOfDisplayData = this.listOfDisplayData.filter(candidate => {
          const value = data.toString().toUpperCase();
          return candidate.dni.toString().toUpperCase().indexOf(value) !== -1;
        });
      }
    });
    this.candidateSubscriptions.add(this.searchSub);
  }

    private handleCandidateResponse(res: Candidate[]) {
    if (res) {
      this.filteredCandidates = res;
      this.listOfDisplayData = res.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1)
        : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    }
  }
  

  resetForm(id:number = 0) {
    this.validateForm = this.fb.group({
      name: [null, [Validators.required, Validators.pattern(/^[a-zA-Z\s]*$/)]],
      lastName: [null, [Validators.required, Validators.pattern(/^[a-zA-Z\s]*$/)]],
      dni: [0, [Validators.required, dniValidator]],
      email: [null,
        {
          validators: [Validators.email],
          asyncValidators: UniqueEmailValidator(this.facade.candidateService, id),
          updateOn: "blur"
        }
      ],
      phoneNumberPrefix: ['+54'],
      phoneNumber: [null, [Validators.pattern(/^\+?[1-9]\d{9,11}$/), Validators.minLength(10), Validators.maxLength(12)]],
      linkedin: [null],
      user: [null, [Validators.required]],
      englishLevel: 'none',
      status: null,
      contactDay: null,
      preferredOffice: [null],
      community: [null, [Validators.required]],
      profile: [null],
      isReferred: [null],
      knownFrom: [null],
      referredBy: [null],
      source: null
    });
  }

  checkLength(field) {
    let fieldName = field.attributes.id.nodeValue,
      maxLength = Number(field.attributes.maxlength.nodeValue) || 8,
      inputValue = this.validateForm.controls[fieldName].value;

    if (inputValue == null) { return }

    if (inputValue.toString().length > maxLength) {
      this.validateForm.controls[fieldName].setValue(inputValue.toString().replace(".", "").substring(0, maxLength));
    }
  }

  reset(): void {
    this.searchValue = '';
    this.searchValueStatus = '';
    this.search();
  }

  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchCandidates.length ?
        this.listOfSearchCandidates.some(candidates => item.name.indexOf(candidates) !== -1) : true) &&
        (replaceAccent(item.name.toString().toUpperCase() +
          item.lastName.toString().toUpperCase()).indexOf(replaceAccent(this.searchValue.toUpperCase())) !== -1);
    };
    const data = this.filteredCandidates.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend')
      ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.nameDropdown.nzVisible = false;
  }

  searchStatus(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchCandidates.length ? this.listOfSearchCandidates.some(p => item.status.indexOf(p) !== -1) : true) &&
        (item.status === this.searchValueStatus);
    };
    const data = this.filteredCandidates.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend')
      ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.searchValueStatus = '';

  }

  sort(sortName: string, value: string): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.resetForm(id);
    this.getSkills();
    if (this.completeSkillList.length === 0) { this.skills.forEach(sk => this.completeSkillList.push(sk)); }
    this.editingCandidateId = id;
    this.isEdit = true;
    this.controlArray = [];
    this.controlEditArray = [];
    let editedCandidate: Candidate = this.filteredCandidates.filter(candidate => candidate.id === id)[0];
    this.fillCandidateForm(editedCandidate);
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--lg',
      nzTitle: 'Edit Candidate',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        {
          label: 'Cancel',

          onClick: () => modal.destroy()
        },
        {
          label: 'Save',
          type: 'primary',
          loading: false,
          onClick: () => {
            this.facade.appService.startLoading();
            if (validateCandidateForm(this.validateForm)) {
              const candidateSkills: CandidateSkill[] = [];
              this.controlEditArray.forEach(skillEdit => {
                const skill: CandidateSkill = {
                  candidateId: id,
                  candidate: null,
                  skillId: skillEdit.id,
                  skill: null,
                  rate: this.validateForm.controls[skillEdit.controlInstance[1]].value,
                  comment: this.validateForm.controls[skillEdit.controlInstance[2]].value
                };
                candidateSkills.push(skill);
              });
              this.controlArray.forEach(skillControl => {
                const skill: CandidateSkill = {
                  candidateId: id,
                  candidate: null,
                  skillId: this.validateForm.controls[skillControl.controlInstance[0]].value,
                  skill: null,
                  rate: this.validateForm.controls[skillControl.controlInstance[1]].value,
                  comment: this.validateForm.controls[skillControl.controlInstance[2]].value
                };
                candidateSkills.push(skill);
              });
              let knownFrom;
              knownFrom = this.validateForm.controls['knownFrom'].value;
              editedCandidate = {
                id: 0,
                name: this.validateForm.controls['name'].value.toString(),
                lastName: this.validateForm.controls['lastName'].value.toString(),
                dni: this.validateForm.controls['dni'].value,
                emailAddress: this.validateForm.controls['email'].value ? this.validateForm.controls['email'].value.toString() : null,
                phoneNumber: '(' + this.validateForm.controls['phoneNumberPrefix'].value.toString() + ')',
                linkedInProfile: this.validateForm.controls['linkedin'].value === null
                  ? null : this.validateForm.controls['linkedin'].value.toString(),
                candidateSkills: candidateSkills,
                englishLevel: this.validateForm.controls['englishLevel'].value,
                status: this.validateForm.controls['status'].value,
                preferredOfficeId: this.validateForm.controls['preferredOffice'].value,
                user: new User(this.validateForm.controls['user'].value, null),
                contactDay: new Date(),
                profile: new CandidateProfile(this.validateForm.controls['profile'].value),
                community: new Community(this.validateForm.controls['community'].value),
                isReferred: this.validateForm.controls['isReferred'].value,
                cv: null,
                knownFrom: knownFrom,
                referredBy: this.referredBy,
                source: this.validateForm.controls['source'].value
              };
              if (this.validateForm.controls['phoneNumber'].value) {
                editedCandidate.phoneNumber += this.validateForm.controls['phoneNumber'].value.toString();
              }
              this.facade.candidateService.update(id, editedCandidate)
                .subscribe(res => {
                  this.getCandidates();
                  this.facade.appService.stopLoading();
                  this.facade.toastrService.success('Candidate was successfully edited !');
                  modal.destroy();
                }, err => {
                  this.facade.appService.stopLoading();
                  this.facade.errorHandlerService.showErrorMessage(err);
                });
            }
            this.facade.appService.stopLoading();
          }
        }],
    });
  }

  showDetailsModal(candidateID: number, modalContent: TemplateRef<{}>): void {
    this.emptyCandidate = this.filteredCandidates.filter(candidate => candidate.id === candidateID)[0];
    this.detailsModal.showModal(modalContent, this.emptyCandidate.name + ' ' + this.emptyCandidate.lastName);
  }

  showDeleteConfirm(candidateID: number): void {
    const candidateDelete: Candidate = this.filteredCandidates.filter(candidate => candidate.id === candidateID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + candidateDelete.lastName + ', ' + candidateDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.candidateService.delete(candidateID)
        .subscribe(res => {
          this.getCandidates();
          this.facade.toastrService.success('Candidate was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  reactivateCandidate(candidateID: number): void {
    const candidateDelete: Candidate = this.filteredCandidates.filter(candidate => candidate.id === candidateID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to reactivate ' + candidateDelete.lastName + ', ' + candidateDelete.name + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.candidateService.reactivate(candidateID)
        .subscribe(res => {
          this.getCandidates();
          this.facade.toastrService.success('Candidate was reactivated!');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
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
      const skills = this.skills;
      const newSetOfSkills: Skill[] = [];
      const skill: Skill = this.skills.filter(s => s.id === this.validateForm.controls[`skill${id - 1}`].value)[0];
      skills.forEach(sk => {
        if (sk !== skill) { newSetOfSkills.push(sk); }
      });
      this.skills = newSetOfSkills;
    }
    if (this.editingCandidateId > 0) { this.removeCandidateSkills(); }
    const index = this.controlArray.push(control);
    this.validateForm.addControl(this.controlArray[index - 1].controlInstance[0], new FormControl(null, Validators.required));
    this.validateForm.addControl(this.controlArray[index - 1].controlInstance[1], new FormControl(10));
    this.validateForm.addControl(this.controlArray[index - 1].controlInstance[2], new FormControl(null, Validators.required));
  }

  removeCandidateSkills() {
    const skills: Skill[] = [];
    this.completeSkillList.forEach(sk => {
      let exists = false;
      this.controlEditArray.forEach(control => {
        if (this.validateForm.controls[control.controlInstance[0]].value === sk.name) { exists = true; }
      });
      this.controlArray.forEach(control => {
        if (this.validateForm.controls[control.controlInstance[0]].value === sk.id) { exists = true; }
      });
      if (!exists && skills.filter(s => s.id === sk.id).length === 0) { skills.push(sk); }
    });
    this.skills = skills;
  }

  removeField(i: { id: number, controlInstance: string[] }, e: MouseEvent, isEdit: boolean): void {
    e.preventDefault();
    const skillList: Skill[] = [];
    this.completeSkillList.forEach(sk => skillList.push(sk));
    if (isEdit) {
      if (this.controlEditArray.length >= 1) {
        if (this.validateForm.controls[i.controlInstance[0]].value !== null) {
          this.skills.push(skillList.filter(skill => skill.name === this.validateForm.controls[i.controlInstance[0]].value)[0]);
          this.skills.sort((a, b) => (a.id > b.id ? 1 : -1));
        }
        let j = 0;
        const index = this.controlEditArray.indexOf(i);
        this.controlEditArray.splice(index, 1);
        for (j; j < 3; j++) { this.validateForm.removeControl(i.controlInstance[j]); }
      }
    } else {
      if (this.controlArray.length >= 1) {
        if (this.validateForm.controls[i.controlInstance[0]].value !== null) {
          this.skills.push(skillList.filter(skill => skill.id === this.validateForm.controls[i.controlInstance[0]].value)[0]);
          this.skills.sort((a, b) => (a.id > b.id ? 1 : -1));
        }
        let j = 0;
        const index = this.controlArray.indexOf(i);
        this.controlArray.splice(index, 1);
        for (j; j < 3; j++) { this.validateForm.removeControl(i.controlInstance[j]); }
      }
    }
  }

  getFormControl(name: string): AbstractControl {
    return this.validateForm.controls[name];
  }

  fillCandidateForm(candidate: Candidate) {
    const candidateReferredBy = candidate.referredBy !== null ? candidate.referredBy : '';
    const candidateKnownFrom = candidate.knownFrom !== null ? candidate.knownFrom : '';
    this.validateForm.controls['dni'].setValue(candidate.dni);
    this.validateForm.controls['dni'].disable();
    this.validateForm.controls['name'].setValue(candidate.name);
    this.validateForm.controls['name'].disable();
    this.validateForm.controls['lastName'].setValue(candidate.lastName);
    this.validateForm.controls['lastName'].disable();
    this.validateForm.controls['email'].setValue(candidate.emailAddress);
    this.validateForm.controls['linkedin'].setValue(candidate.linkedInProfile);
    this.validateForm.controls['linkedin'].disable();
    this.validateForm.controls['phoneNumberPrefix'].setValue(candidate.phoneNumber.substring(1, candidate.phoneNumber.indexOf(')')));
    this.validateForm.controls['phoneNumber'].setValue(candidate.phoneNumber.split(')')[1]);
    this.validateForm.controls['user'].setValue(candidate.user ? candidate.user.id : null);
    this.validateForm.controls['user'].disable();
    this.validateForm.controls['preferredOffice'].setValue(candidate.preferredOfficeId);
    this.validateForm.controls['preferredOffice'].disable();
    this.validateForm.controls['englishLevel'].setValue(candidate.englishLevel);
    this.validateForm.controls['englishLevel'].disable();
    this.validateForm.controls['status'].setValue(candidate.status);
    this.validateForm.controls['status'].disable();
    this.validateForm.controls['community'].setValue(candidate.community.id);
    this.validateForm.controls['community'].disable();
    this.validateForm.controls['profile'].setValue(candidate.profile ? candidate.profile.id : null);
    this.validateForm.controls['profile'].disable();
    this.validateForm.controls['isReferred'].setValue(candidate.isReferred);
    this.validateForm.controls['isReferred'].disable();
    this.validateForm.controls['referredBy'].setValue(candidateReferredBy);
    this.validateForm.controls['referredBy'].disable();
    this.referredBy = candidateReferredBy;
    this.validateForm.controls['knownFrom'].setValue(candidateKnownFrom);
    this.validateForm.controls['knownFrom'].disable();
    this.validateForm.controls['source'].setValue(candidate.source);
    this.validateForm.controls['source'].disable();
    if (candidate.candidateSkills.length > 0) {
      candidate.candidateSkills.forEach(skill => {
        const id = (this.controlEditArray.length > 0) ? this.controlEditArray[this.controlEditArray.length - 1].id + 1 : 0;
        const control = {
          id: skill.skillId,
          controlInstance: [`skillEdit${id}`, `slidderEdit${id}`, `commentEdit${id}`]
        };
        const index = this.controlEditArray.push(control);
        this.validateForm.addControl(this.controlEditArray[index - 1].controlInstance[0],
          new FormControl({value: skill.skill.name, disabled: true}));
          this.validateForm.addControl(this.controlEditArray[index - 1].controlInstance[1],
            new FormControl({value: skill.rate, disabled: true}));
          this.validateForm.addControl(this.controlEditArray[index - 1].controlInstance[2],
            new FormControl({value: skill.comment, disabled: true}));

      });
    }
  }

  changeFormStatus(enable: boolean) {
    for (const i in this.validateForm.controls) {
      if (this.validateForm.controls[i] !== this.validateForm.controls['dni']) {
        if (enable) { this.validateForm.controls[i].enable(); } else { this.validateForm.controls[i].disable(); }
      }
    }
  }

  getStatus(status: number): string {
    const statusFilter = this.statusList.filter(st => st.id === status);
    if (statusFilter.length !== 0) {
      return statusFilter[0].name;
    }
  }
  ngOnDestroy() {
    this.candidateSubscriptions.unsubscribe();
  }

  statusChanged() {
    //Temporal fix to make modal reize when the form creates new items dinamically that exceeds the height of the modal.
    resizeModal();
  }

  getSeniorityName(id: number): string {
    return this.seniorityList.filter(x => x.id === id)[0].name;
  }

  openCV(cv) {
    window.open(cv);
  }

  checkCV(cv) {
    return (!!cv);
  }
}
