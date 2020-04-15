import { Component, OnInit, ViewChild, Input, TemplateRef, SimpleChanges } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { trimValidator } from 'src/app/directives/trim.validator';
import { Candidate } from 'src/entities/candidate';
import { AppComponent } from 'src/app/app.component';
import { User } from 'src/entities/user';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { CandidateDetailsComponent } from 'src/app/candidates/details/candidate-details.component';
import { CandidateAddComponent } from 'src/app/candidates/add/candidate-add.component';
import { CandidateStatusEnum } from '../../../entities/enums/candidate-status.enum';
import { EnglishLevelEnum } from '../../../entities/enums/english-level.enum';
import { Community } from 'src/entities/community';
import { CandidateProfile } from 'src/entities/Candidate-Profile';
import { replaceAccent } from 'src/app/helpers/string-helpers'
import { ReferralsComponent } from '../referrals/referrals.component';

@Component({
  selector: 'app-referrals-contact',
  templateUrl: './referrals-contact.component.html',
  styleUrls: ['./referrals-contact.component.css']
})

export class ReferralsContactComponent implements OnInit {


  @ViewChild('dropdown') nameDropdown;
  @ViewChild(CandidateAddComponent) candidateAdd: CandidateAddComponent;

  @Input()
  private _users: User[];
  public get users(): User[] {
    return this._users;
  }
  public set users(value: User[]) {
    this.fillUsers = value;
  }


  
  @Input()
  private _visible: boolean;
  public get visibles(): boolean {
    return this._visible;
  }
  public set visibles(value: boolean) {
    this.visible = value;
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

  @Input()
  private _processModal: TemplateRef<{}>;
  public get processModal(): TemplateRef<{}> {
    return this._processModal;
  }
  public set processModal(value: TemplateRef<{}>) {
    this.processStartModal = value;
  }

  @Input()
  private _processFooterModal: TemplateRef<{}>;
  public get processFootModal(): TemplateRef<{}> {
    return this._processFooterModal;
  }
  public set processFootModal(value: TemplateRef<{}>) {
    this.processFooterModal = value;
  }

  processStartModal: TemplateRef<{}>;
  processFooterModal: TemplateRef<{}>;
  fillUsers: User[] = [];
  comms: Community[] = [];
  filteredCommunity: Community[] = [];
  profiles: CandidateProfile[] = [];
  currentUser: User;
  candidateForm: FormGroup = this.fb.group({
    name: ['', [trimValidator]],
    firstName: [null, [Validators.required, trimValidator]],
    lastName: [null, [Validators.required, trimValidator]],
    email: [null, [Validators.email]],
    phoneNumberPrefix: ['+54'],
    phoneNumber: [null, trimValidator],
    user: [null, [Validators.required]],
    contactDay: [new Date(), [Validators.required]],
    community: [null, [Validators.required]],
    profile: [null, [Validators.required]],
    linkedInProfile: [null, [trimValidator]],
    isReferred: false,
    id: [null],
    cv: [null],
    knownFrom: [null],
    referredBy: [null]
  });
  visible: boolean = false;
  isNewCandidate: boolean = false;
  isEditCandidate: boolean = false;
  candidates: Candidate[] = [];

  searchValue = '';
  listOfSearchProcesses = [];

  filteredCandidate: Candidate[] = [];
  listOfDisplayData = [...this.filteredCandidate];

  emptyCandidate: Candidate;
  emptyUser: User;

  editingCandidateId: number = 0;
  // candidateForm: FormGroup;

  constructor(private fb: FormBuilder, private facade: FacadeService, private app: AppComponent, private detailsModal: CandidateDetailsComponent,
    private modalService: NzModalService, private process: ReferralsComponent) {
    this.currentUser = JSON.parse(localStorage.getItem("currentUser"));
  }

  ngOnInit() {
    this.fillUsers = this._users;
    this.comms = this._communities;
    this.filteredCommunity = this._communities;
    this.profiles = this._candidateProfiles;
    this.processFootModal = this._processFooterModal;
    this.processStartModal = this._processModal;
    this.getUsers();
    this.getCandidates();

    this.visible = this._visible;
    this.isNewCandidate = this.visible;
  }
  profileChanges(profileId){
    this.candidateForm.controls['community'].reset();
    this.filteredCommunity = this.comms.filter(c => c.profileId === profileId);
  }
  

  getCandidates() {
    this.facade.candidateService.get()
      .subscribe(res => {
        this.candidates = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      })
  }

  getUsers() {
    this.facade.userService.get()
      .subscribe(res => {
        this.fillUsers = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  searchedCandidateModal(modalContent: TemplateRef<{}>): void {
    const modal: NzModalRef = this.modalService.create({
      nzTitle: 'Candidate founded',
      nzContent: modalContent,
      nzFooter: [
        {
          label: 'NEW',
          type: 'primary',
          onClick: () => {
            modal.destroy();
            this.handleOk();
          }
        },
        {
          label: 'Close',
          shape: 'default',
          onClick: () => modal.destroy()
        }
      ]
    });
  }

  handleOk(): void {
    this.isNewCandidate = true;
    this.visible = true;
    this.isEditCandidate = false;
    this.resetForm();
    this.candidateForm.controls['user'].setValue(this.fillUsers.filter(r => r.username.toLowerCase() === this.currentUser.username.toLowerCase())[0].id);
    this.candidateForm.controls['contactDay'].setValue(new Date());
  }

  edit(id: number): void {
    this.resetForm();
    this.isEditCandidate = true;
    this.visible = true;
    this.isNewCandidate = false;
    this.editingCandidateId = id;
    let editedCandidate: Candidate = this.candidates.filter(Candidate => Candidate.id == id)[0];
    this.fillCandidateForm(editedCandidate);
    this.modalService.openModals[1].close(); // el 1 es un numero magico, despues habria que remplazarlo por un length
  }

  showDeleteConfirm(CandidateID: number): void {
    let CandidateDelete: Candidate = this.candidates.filter(c => c.id == CandidateID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete ' + CandidateDelete.name + ' ' + CandidateDelete.lastName + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.candidateService.delete(CandidateID)
        .subscribe(res => {
          this.getCandidates();
          this.facade.toastrService.success('Candidate was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  showDetailsModal(candidateID: number, modalContent: TemplateRef<{}>): void {
    this.emptyCandidate = this.filteredCandidate.filter(candidate => candidate.id == candidateID)[0];
    this.detailsModal.showModal(modalContent, this.emptyCandidate.name + " " + this.emptyCandidate.lastName);
  }

  searchCandidate(searchString: string, modalContent: TemplateRef<{}>) {
    let candidate = this.candidates.filter(s => {return (replaceAccent(s.name).toLowerCase() + " " + replaceAccent(s.lastName).toLowerCase()).indexOf(replaceAccent(searchString).toLowerCase()) !== -1});
    this.filteredCandidate = candidate;
    this.searchedCandidateModal(modalContent);
  }

  fillCandidateForm(Candidate: Candidate) {
    this.candidateForm.controls['firstName'].setValue(Candidate.name);
    this.candidateForm.controls['lastName'].setValue(Candidate.lastName);
    this.candidateForm.controls['phoneNumberPrefix'].setValue(Candidate.phoneNumber.substring(1, Candidate.phoneNumber.indexOf(')')));
    this.candidateForm.controls['phoneNumber'].setValue(Candidate.phoneNumber.split(')')[1]);
    this.candidateForm.controls['email'].setValue(Candidate.emailAddress);
    this.candidateForm.controls['user'].setValue(Candidate.user);
    this.candidateForm.controls['id'].setValue(Candidate.id);
    this.candidateForm.controls['contactDay'].setValue(new Date(Candidate.contactDay));
    this.candidateForm.controls['profile'].setValue(Candidate.profile.id);
    this.candidateForm.controls['community'].setValue(Candidate.community.id);
    this.candidateForm.controls['isReferred'].setValue(Candidate.isReferred);
    this.candidateForm.controls['referredBy'].setValue(Candidate.referredBy);
    this.candidateForm.controls['cv'].setValue(Candidate.cv);
    this.candidateForm.controls['knownFrom'].setValue(Candidate.knownFrom);
  }

  resetForm() {
    this.candidateForm = this.fb.group({
      name: ['', [trimValidator]],
      firstName: [null, [Validators.required, trimValidator]],
      lastName: [null, [Validators.required, trimValidator]],
      email: [null, [Validators.email]],
      phoneNumberPrefix: ['+54'],
      phoneNumber: [null],
      user: [null, [Validators.required]],
      contactDay: [null, [Validators.required]],
      community: [null, [Validators.required]],
      profile: [null, [Validators.required]],
      linkedInProfile: [null, [trimValidator]],
      isReferred: false,
      id: [null],
      knownFrom: [null],
      cv: [null],
      referredBy: [null]
    });
  }

  saveEdit(idCandidate: number) {
    let isCompleted: boolean = true;
    let editedCandidate: Candidate = this.candidates.filter(Candidate => Candidate.id == idCandidate)[0];
   if (isCompleted) {
      editedCandidate = {
        id: idCandidate,
        name: this.candidateForm.controls['firstName'].value.toString(),
        lastName: this.candidateForm.controls['lastName'].value.toString(),
        phoneNumber: '(' + this.candidateForm.controls['phoneNumberPrefix'].value.toString() + ')',
        dni: editedCandidate.dni,
        emailAddress: this.candidateForm.controls['email'].value ? this.candidateForm.controls['email'].value.toString() : null,
        user: this.candidateForm.controls['user'].value,
        contactDay: new Date(this.candidateForm.controls['contactDay'].value.toString()),
        linkedInProfile: editedCandidate.linkedInProfile,
        englishLevel: editedCandidate.englishLevel,
        additionalInformation: editedCandidate.additionalInformation,
        status: editedCandidate.status,
        candidateSkills: editedCandidate.candidateSkills,
        preferredOfficeId: editedCandidate.preferredOfficeId,
        profile: editedCandidate.profile,
        community: editedCandidate.community,
        isReferred: editedCandidate.isReferred,
        cv: editedCandidate.cv,
        knownFrom: editedCandidate.knownFrom,
        referredBy: editedCandidate.referredBy,
      }
      if (this.candidateForm.controls['phoneNumber'].value) {
        editedCandidate.phoneNumber += this.candidateForm.controls['phoneNumber'].value.toString();
      }
      this.facade.candidateService.update(idCandidate, editedCandidate)
        .subscribe(res => {
          this.getCandidates();
          this.facade.toastrService.success('Candidate was successfully edited !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    }
    this.isEditCandidate = false;
    this.visible = false;
  }

  Recontact(idCandidate: number) {
    let editedCandidate: Candidate = this.candidates.filter(Candidate => Candidate.id == idCandidate)[0];
    editedCandidate = {
      id: idCandidate,
      name: editedCandidate.name,
      lastName: editedCandidate.lastName,
      phoneNumber: editedCandidate.phoneNumber,
      dni: editedCandidate.dni,
      emailAddress: editedCandidate.emailAddress,
      user: this.fillUsers.filter(r => r.username.toLowerCase() === this.currentUser.username.toLowerCase())[0],
      contactDay: new Date(),
      linkedInProfile: editedCandidate.linkedInProfile,
      englishLevel: editedCandidate.englishLevel,
      additionalInformation: editedCandidate.additionalInformation,
      status: CandidateStatusEnum.Recall,
      preferredOfficeId: editedCandidate.preferredOfficeId,
      candidateSkills: editedCandidate.candidateSkills,
      profile: editedCandidate.profile,
      community: editedCandidate.community,
      isReferred: editedCandidate.isReferred,
      cv: editedCandidate.cv,
      knownFrom: editedCandidate.knownFrom,
      referredBy: editedCandidate.referredBy
    }

    this.facade.candidateService.update(idCandidate, editedCandidate)
      .subscribe(res => {
        this.getCandidates();
        this.facade.toastrService.success('Candidate was successfully edited !');
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      })
  }

  createNewCandidate() {
    this.app.showLoading;
    let isCompleted: boolean = true;

    for (const i in this.candidateForm.controls) {
      this.candidateForm.controls[i].markAsDirty();
      this.candidateForm.controls[i].updateValueAndValidity();
      if (!this.candidateForm.controls[i].valid) isCompleted = false;
    }

    if (isCompleted) {
      let newCandidate: Candidate = {
        id: 0,
        name: this.candidateForm.controls['firstName'].value.toString(),
        lastName: this.candidateForm.controls['lastName'].value.toString(),
        phoneNumber: '(' + this.candidateForm.controls['phoneNumberPrefix'].value.toString() + ')',
        dni: 0,
        emailAddress: this.candidateForm.controls['email'].value ? this.candidateForm.controls['email'].value.toString() : null,
        user: new User(this.candidateForm.controls['user'].value, null),
        contactDay: new Date(this.candidateForm.controls['contactDay'].value.toString()),
        //linkedInProfile: this.candidateForm.controls['linkedInProfile'].value.toString(),
        linkedInProfile: null,
        englishLevel: EnglishLevelEnum.None,
        additionalInformation: '',
        status: CandidateStatusEnum.New,
        preferredOfficeId: null,
        candidateSkills: [],
        isReferred: this.candidateForm.controls['isReferred'].value,
        community: new Community(this.candidateForm.controls['community'].value),
        profile: new CandidateProfile(this.candidateForm.controls['profile'].value),
        cv: null,
        knownFrom: null,
        referredBy: this.currentUser.firstName + ' ' + this.currentUser.lastName
      }
      if (this.candidateForm.controls['phoneNumber'].value) {
        newCandidate.phoneNumber += this.candidateForm.controls['phoneNumber'].value.toString();
      }
      this.facade.referralsService.add(newCandidate)
        .subscribe(res => {
          this.facade.toastrService.success('Candidate was successfully created !');
          this.isNewCandidate = false;
          this.visible = false;
          this.app.hideLoading();
          this.facade.referralsService.addNew(newCandidate);
          this.modalService.closeAll();
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
          this.app.hideLoading;
        })
    }
  }
}
