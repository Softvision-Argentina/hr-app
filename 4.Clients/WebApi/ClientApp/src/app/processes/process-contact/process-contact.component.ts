import { Component, OnInit, ViewChild, Input, TemplateRef } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { trimValidator } from 'src/app/directives/trim.validator';
import { Candidate } from 'src/entities/candidate';
import { Consultant } from 'src/entities/consultant';
import { AppComponent } from 'src/app/app.component';
import { User } from 'src/entities/user';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { CandidateDetailsComponent } from 'src/app/candidates/details/candidate-details.component';
import { ProcessesComponent } from 'src/app/processes/processes/processes.component';
import { CandidateAddComponent } from 'src/app/candidates/add/candidate-add.component';
import { CandidateStatusEnum } from '../../../entities/enums/candidate-status.enum';
import { EnglishLevelEnum } from '../../../entities/enums/english-level.enum';
import { Community } from 'src/entities/community';
import { CandidateProfile } from 'src/entities/Candidate-Profile';
import { replaceAccent } from 'src/app/helpers/string-helpers';
import { Process } from 'src/entities/process';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-process-contact',
  templateUrl: './process-contact.component.html',
  styleUrls: ['./process-contact.component.css']
})

export class ProcessContactComponent implements OnInit {


  @ViewChild('dropdown') nameDropdown;
  @ViewChild(CandidateAddComponent) candidateAdd: CandidateAddComponent;

  @Input()
  private _consultants: Consultant[];
  public get consultants(): Consultant[] {
    return this._consultants;
  }
  public set consultants(value: Consultant[]) {
    this.recruiters = value;
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
  recruiters: Consultant[] = [];
  comms: Community[] = [];
  filteredCommunity: Community[] = [];
  profiles: CandidateProfile[] = [];
  currentUser: User;
  currentConsultant: any;
  candidateForm: FormGroup = this.fb.group({
    name: ['', [trimValidator]],
    firstName: [null, [Validators.required, trimValidator]],
    lastName: [null, [Validators.required, trimValidator]],
    email: [null, [Validators.email]],
    phoneNumberPrefix: ['+54'],
    phoneNumber: [null, trimValidator],
    recruiter: [null, [Validators.required]],
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
  visible = false;
  isNewCandidate = false;
  isEditCandidate = false;
  candidates: Candidate[] = [];
  searchValue = '';
  listOfSearchProcesses = [];
  filteredCandidate: Candidate[] = [];
  listOfDisplayData = [...this.filteredCandidate];
  emptyCandidate: Candidate;
  emptyConsultant: Consultant;
  editingCandidateId = 0;

  constructor(
    private fb: FormBuilder,
    private facade: FacadeService,
    private app: AppComponent,
    private detailsModal: CandidateDetailsComponent,
    private modalService: NzModalService,
    private process: ProcessesComponent) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this.recruiters = this._consultants;
    this.comms = this._communities;
    this.filteredCommunity = this._communities;
    this.profiles = this._candidateProfiles;
    this.processFootModal = this._processFooterModal;
    this.processStartModal = this._processModal;
    this.getConsultants();
    this.getCandidates().subscribe(() => {}, err => this.facade.errorHandlerService.showErrorMessage(err));
    this.visible = this._visible;
    this.isNewCandidate = this.visible;
    this.facade.consultantService.GetByEmail(this.currentUser.email)
      .subscribe(res => {
        this.currentConsultant = res.body;
        // tslint:disable-next-line: no-unused-expression
        this.currentConsultant !== null ? this.candidateForm.controls['recruiter'].setValue(this.currentConsultant.id) : null;
      });
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

  getConsultants() {
    this.facade.consultantService.get()
      .subscribe(res => {
        this.consultants = res;
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

    this.candidateForm.controls['recruiter'].setValue(this.recruiters.filter(r => r.emailAddress.toLowerCase() === this.currentUser.email.toLowerCase())[0].id);
    this.candidateForm.controls['contactDay'].setValue(new Date());
  }

  edit(id: number): void {
    this.resetForm();
    this.isEditCandidate = true;
    this.visible = true;
    this.isNewCandidate = false;
    this.editingCandidateId = id;
    const editedCandidate: Candidate = this.candidates.filter(candidate => candidate.id === id)[0];
    this.fillCandidateForm(editedCandidate);
    this.modalService.openModals[1].close(); // el 1 es un numero magico, despues habria que remplazarlo por un length
  }

  showDeleteConfirm(CandidateID: number): void {
    const CandidateDelete: Candidate = this.candidates.filter(candidate => candidate.id === CandidateID)[0];
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
    this.emptyCandidate = this.filteredCandidate.filter(candidate => candidate.id === candidateID)[0];
    this.detailsModal.showModal(modalContent, this.emptyCandidate.name + ' ' + this.emptyCandidate.lastName);
  }

  searchCandidate(searchString: string, modalContent: TemplateRef<{}>) {

    const candidate = this.candidates.filter(s => (replaceAccent(s.name).toLowerCase() + ' ' + replaceAccent(s.lastName).toLowerCase()).indexOf(replaceAccent(searchString).toLowerCase()) !== -1);
    this.filteredCandidate = candidate;
    this.searchedCandidateModal(modalContent);
  }

  fillCandidateForm(candidate: Candidate) {
    this.candidateForm.controls['firstName'].setValue(candidate.name);
    this.candidateForm.controls['lastName'].setValue(candidate.lastName);
    this.candidateForm.controls['phoneNumberPrefix'].setValue(candidate.phoneNumber.substring(1, candidate.phoneNumber.indexOf(')')));
    this.candidateForm.controls['phoneNumber'].setValue(candidate.phoneNumber.split(')')[1]);
    this.candidateForm.controls['email'].setValue(candidate.emailAddress);
    this.candidateForm.controls['recruiter'].setValue(candidate.recruiter);
    this.candidateForm.controls['id'].setValue(candidate.id);
    this.candidateForm.controls['contactDay'].setValue(new Date(candidate.contactDay));
    this.candidateForm.controls['profile'].setValue(candidate.profile.id);
    this.candidateForm.controls['community'].setValue(candidate.community.id);
    this.candidateForm.controls['isReferred'].setValue(candidate.isReferred);
    this.candidateForm.controls['referredBy'].setValue(candidate.referredBy);
    this.candidateForm.controls['cv'].setValue(candidate.cv);
    this.candidateForm.controls['knownFrom'].setValue(candidate.knownFrom);
  }

  resetForm() {
    this.candidateForm = this.fb.group({
      name: ['', [trimValidator]],
      firstName: [null, [Validators.required, trimValidator]],
      lastName: [null, [Validators.required, trimValidator]],
      email: [null, [Validators.email]],
      phoneNumberPrefix: ['+54'],
      phoneNumber: [null],
      recruiter: [null, [Validators.required]],
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
    const isCompleted = true;
    let editedCandidate: Candidate = this.candidates.filter(candidate => candidate.id === idCandidate)[0];
    if (isCompleted) {
      editedCandidate = {
        id: idCandidate,
        name: this.candidateForm.controls['firstName'].value.toString(),
        lastName: this.candidateForm.controls['lastName'].value.toString(),
        phoneNumber: '(' + this.candidateForm.controls['phoneNumberPrefix'].value.toString() + ')',
        dni: editedCandidate.dni,
        emailAddress: this.candidateForm.controls['email'].value ? this.candidateForm.controls['email'].value.toString() : null,
        recruiter: this.candidateForm.controls['recruiter'].value,
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
      };
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
    let editedCandidate: Candidate = this.candidates.filter(candidate => candidate.id === idCandidate)[0];
    editedCandidate = {
      id: idCandidate,
      name: editedCandidate.name,
      lastName: editedCandidate.lastName,
      phoneNumber: editedCandidate.phoneNumber,
      dni: editedCandidate.dni,
      emailAddress: editedCandidate.emailAddress,
      recruiter: this.recruiters.filter(r => r.emailAddress.toLowerCase() === this.currentUser.email.toLowerCase())[0],
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
    // tslint:disable-next-line: no-unused-expression
    this.app.showLoading;
    let isCompleted = true;

    for (const i in this.candidateForm.controls) {
      if (this.candidateForm.controls[i]) {
        this.candidateForm.controls[i].markAsDirty();
        this.candidateForm.controls[i].updateValueAndValidity();
        if (!this.candidateForm.controls[i].valid) { isCompleted = false; }
      }
    }

    if (isCompleted) {
      const newCandidate: Candidate = {
        id: 0,
        name: this.candidateForm.controls['firstName'].value.toString(),
        lastName: this.candidateForm.controls['lastName'].value.toString(),
        phoneNumber: '(' + this.candidateForm.controls['phoneNumberPrefix'].value.toString() + ')',
        dni: 0,
        emailAddress: this.candidateForm.controls['email'].value ? this.candidateForm.controls['email'].value.toString() : null,
        recruiter: new Consultant(this.candidateForm.controls['recruiter'].value, null, null),
        contactDay: new Date(this.candidateForm.controls['contactDay'].value.toString()),
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
        referredBy: null
      };
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
          this.app.hideLoading();
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
              const processCandidate: Candidate = this.candidates.filter(candidate => candidate.id === candidateId)[0];
              this.process.newProcessStart(this.processStartModal, this.processFooterModal, processCandidate);
            }
          });
        } else {
          this.modalService.closeAll();
          const processCandidate: Candidate = this.candidates.filter(candidate => candidate.id === candidateId)[0];
          this.process.newProcessStart(this.processStartModal, this.processFooterModal, processCandidate);
        }
      });
  }

}
