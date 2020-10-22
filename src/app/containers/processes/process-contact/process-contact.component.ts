import { Component, Input, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EnglishLevelEnum } from '@shared/enums/english-level.enum';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Candidate } from '@shared/models/candidate.model';
import { Community } from '@shared/models/community.model';
import { Process } from '@shared/models/process.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { replaceAccent } from '@shared/utils/replace-accent.util';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { tap } from 'rxjs/operators';
import { AppComponent } from '@app/app.component';
import { CandidateAddComponent } from '@old-architecture/candidates/add/candidate-add.component';
import { CandidateDetailsComponent } from '@old-architecture/candidates/details/candidate-details.component';
import { ProcessesComponent } from '@app/containers/processes/processes/processes.component';
import { CandidateStatusEnum } from '@shared/enums/candidate-status.enum';
import { UniqueEmailValidator } from '@app/shared/utils/email.validator';

@Component({
  selector: 'app-process-contact',
  templateUrl: './process-contact.component.html',
  styleUrls: ['./process-contact.component.scss']
})

export class ProcessContactComponent implements OnInit {


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
    name: [''],
    firstName: [null, [Validators.required]],
    lastName: [null, [Validators.required]],
    email: [null,
      {
        validators: [Validators.email],
        asyncValidators: UniqueEmailValidator(this.facade.candidateService),
        updateOn: "blur"
      }
    ],
    phoneNumberPrefix: ['+54'],
    phoneNumber: [null, [Validators.pattern(/^\+?[1-9]\d{9,11}$/), Validators.minLength(10), Validators.maxLength(12)]],
    user: [null, [Validators.required]],
    contactDay: [new Date(), [Validators.required]],
    community: [null, [Validators.required]],
    profile: [null, [Validators.required]],
    linkedInProfile: [null],
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

  constructor(
    private fb: FormBuilder,
    private facade: FacadeService,
    private app: AppComponent,
    private detailsModal: CandidateDetailsComponent,
    private modalService: NzModalService,
    private process: ProcessesComponent) {
    this.currentUser = JSON.parse(localStorage.getItem("currentUser"));
  }

  ngOnInit() {
    this.fillUsers = this._users.sort((a,b) => ((a.firstName + " " + a.lastName).localeCompare(b.firstName + " " + b.lastName)));
    this.comms = this._communities;
    this.filteredCommunity = this._communities.sort((a,b) => (a.name.localeCompare(b.name)));
    this.profiles = this._candidateProfiles.sort((a,b) => (a.name.localeCompare(b.name)));
    this.processFootModal = this._processFooterModal;
    this.processStartModal = this._processModal;
    this.getUsers();
    this.getCandidates().subscribe(() => {}, err => this.facade.errorHandlerService.showErrorMessage(err));
    this.visible = this._visible;
    this.isNewCandidate = this.visible;
 }

  profileChanges(profileId) {
    this.candidateForm.controls['community'].reset();
    this.filteredCommunity = this.comms.filter(c => c.profileId === profileId);
  }

  getCandidates() {
    return this.facade.candidateService.get().pipe(
      tap(res => {
        this.candidates = res;
      })
    );
  }

  getUsers() {
    this.facade.userService.getUsers()
      .subscribe(res => {
        this.fillUsers = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  searchedCandidateModal(modalContent: TemplateRef<{}>): void {
    const modal: NzModalRef = this.modalService.create({
      nzTitle: 'Candidate found',
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
    let editedCandidate: Candidate = this.candidates.filter(Candidate => Candidate.id === id)[0];
    this.fillCandidateForm(editedCandidate);
    this.modalService.openModals[1].close(); // el 1 es un numero magico, despues habria que remplazarlo por un length
  }

  showDeleteConfirm(CandidateID: number): void {
    let CandidateDelete: Candidate = this.candidates.filter(c => c.id === CandidateID)[0];
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete ' + CandidateDelete.name + ' ' + CandidateDelete.lastName + ' ?',
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
    this.emptyCandidate = this.filteredCandidate.filter(candidate => candidate.id === candidateID)[0];
    this.detailsModal.showModal(modalContent, this.emptyCandidate.name + " " + this.emptyCandidate.lastName);
  }

  searchCandidate(searchString: string, modalContent: TemplateRef<{}>) {
    let candidate = this.candidates.filter(s => { return (replaceAccent(s.name).toLowerCase() + " " + replaceAccent(s.lastName).toLowerCase()).indexOf(replaceAccent(searchString).toLowerCase()) !== -1 });
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
      name: [''],
      firstName: [null, [Validators.required]],
      lastName: [null, [Validators.required]],
      email: [null,
        {
          validators: [Validators.email],
          asyncValidators: UniqueEmailValidator(this.facade.candidateService),
          updateOn: "blur"
        }
      ],
      phoneNumberPrefix: ['+54'],
      phoneNumber: [null, Validators.pattern(/^[0-9]+$/)],
      user: [null, [Validators.required]],
      contactDay: [null, [Validators.required]],
      community: [null, [Validators.required]],
      profile: [null, [Validators.required]],
      linkedInProfile: [null],
      isReferred: false,
      id: [null],
      knownFrom: [null],
      cv: [null],
      referredBy: [null]
    });
  }

  saveEdit(idCandidate: number) {
    let isCompleted: boolean = true;
    let editedCandidate: Candidate = this.candidates.filter(Candidate => Candidate.id === idCandidate)[0];
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
        status: editedCandidate.status,
        candidateSkills: editedCandidate.candidateSkills,
        preferredOfficeId: editedCandidate.preferredOfficeId,
        profile: editedCandidate.profile,
        community: editedCandidate.community,
        isReferred: editedCandidate.isReferred,
        cv: editedCandidate.cv,
        knownFrom: editedCandidate.knownFrom,
        referredBy: editedCandidate.referredBy,
        source: editedCandidate.source
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
        });
    }
    this.isEditCandidate = false;
    this.visible = false;
  }

  Recontact(idCandidate: number) {
    let editedCandidate: Candidate = this.candidates.filter(Candidate => Candidate.id === idCandidate)[0];
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
      status: CandidateStatusEnum.Recall,
      preferredOfficeId: editedCandidate.preferredOfficeId,
      candidateSkills: editedCandidate.candidateSkills,
      profile: editedCandidate.profile,
      community: editedCandidate.community,
      isReferred: editedCandidate.isReferred,
      cv: editedCandidate.cv,
      knownFrom: editedCandidate.knownFrom,
      referredBy: editedCandidate.referredBy,
      source: editedCandidate.source
    };
    this.facade.candidateService.update(idCandidate, editedCandidate)
      .subscribe(res => {
        this.getCandidates();
        this.facade.toastrService.success('Candidate was successfully edited !');
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
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
        user: this.candidateForm.controls['user'].value,
        contactDay: new Date(this.candidateForm.controls['contactDay'].value.toString()),
        linkedInProfile: null,
        englishLevel: EnglishLevelEnum.None,
        status: CandidateStatusEnum.New,
        preferredOfficeId: null,
        candidateSkills: [],
        isReferred: this.candidateForm.controls['isReferred'].value,
        community: new Community(this.candidateForm.controls['community'].value),
        profile: new CandidateProfile(this.candidateForm.controls['profile'].value),
        cv: null,
        knownFrom: null,
        referredBy: null,
        source: null
      }
      if (this.candidateForm.controls['phoneNumber'].value) {
        newCandidate.phoneNumber += this.candidateForm.controls['phoneNumber'].value.toString();
      }
      this.facade.candidateService.add(newCandidate)
        .subscribe(res => {
          this.facade.toastrService.success('Candidate was successfully created !');
          this.isNewCandidate = false;
          this.app.hideLoading();
          this.getCandidates()
          .subscribe(() => {
            this.startNewProcess(res.id);
          }, err => {
            this.facade.errorHandlerService.showErrorMessage(err);
          });
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
          this.app.hideLoading;
        });
    }
  }

  startNewProcess(candidateId: number) {
    this.facade.processService.getActiveProcessByCandidate(candidateId)
      .subscribe((res: Process[]) => {
        if (res.length > 0) {
          this.facade.modalService.confirm({
            nzTitle: 'There is already another process of ' + res[0].candidate.lastName + ', ' + res[0].candidate.name + '. Do you want to open a new one ?',
            nzContent: '',
            nzOkText: 'Yes',
            nzOkType: 'danger',
            nzCancelText: 'No',
            nzOnOk: () => {
              this.modalService.closeAll();
              let processCandidate: Candidate = this.candidates.filter(Candidate => Candidate.id === candidateId)[0];
              this.process.newProcessStart(this.processStartModal, this.processFooterModal, processCandidate);
            }
          });
        }
        else {
          this.modalService.closeAll();
          let processCandidate: Candidate = this.candidates.filter(Candidate => Candidate.id === candidateId)[0];
          this.process.newProcessStart(this.processStartModal, this.processFooterModal, processCandidate);
        }
      });
  }

}
