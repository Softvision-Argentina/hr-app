import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { customCvAndLinkedInValidator, customEmailAndPhoneNumberValidator } from '@app/shared/utils/forms.validators';
import { CandidateAddComponent } from '@old-architecture/candidates/add/candidate-add.component';
import { CandidateStatusEnum } from '@shared/enums/candidate-status.enum';
import { EnglishLevelEnum } from '@shared/enums/english-level.enum';
import { Candidate } from '@shared/models/candidate.model';
import { Community } from '@shared/models/community.model';
import { Cv } from '@shared/models/cv.model';
import { OpenPosition } from '@shared/models/open-position.model';
import { User } from '@shared/models/user.model';
import { BaseService } from '@shared/services/base.service';
import { FacadeService } from '@shared/services/facade.service';
import { UniqueEmailValidator } from '@shared/utils/email.validator';
import { NzModalService, NzUploadFile } from 'ng-zorro-antd';
import { ReferralsSandbox } from '../referrals/referral.sandbox';
import { ReferralsService } from '@shared/services/referrals.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-referrals-contact',
  templateUrl: './referrals-contact.component.html',
  styleUrls: ['./referrals-contact.component.scss']
})

export class ReferralsContactComponent implements OnInit {


  @ViewChild('dropdown') nameDropdown;
  @ViewChild(CandidateAddComponent) candidateAdd: CandidateAddComponent;

  @Input()
  private _visible: boolean = true;
  public get visibles(): boolean {
    return this._visible;
  }
  public set visibles(value: boolean) {
    this.visible = value;
  }

  referralId;

  isReferral: boolean;
  @Input() candidateSources = [];

  @Input()
  communities: Community[];
  filteredCommunity: Community[] = [];
  currentUser: User;
  @Input() position: OpenPosition = null;
  @Output() previousPosition = new EventEmitter<OpenPosition>();
  @Output() refreshTableAction = new EventEmitter<Candidate>();

  candidateForm: FormGroup = this.fb.group({
    firstName: [null, [Validators.required, Validators.pattern(/^[a-zA-Z\s]*$/)]],
    lastName: [null, [Validators.required, Validators.pattern(/^[a-zA-Z\s]*$/)]],
    email: [null,
      {
        validators: [Validators.email],
        updateOn: 'blur'
      }
    ],
    link: [null],
    phoneNumberPrefix: ['+54'],
    phoneNumber: [null, [Validators.pattern(/^\+?[1-9]\d{9,11}$/), Validators.minLength(10), Validators.maxLength(12)]],
    community: [null, [Validators.required]],
    file: [''],
    openPositionTitle: [null, { disabled: true }],
    source: null
  }, { validators: [customCvAndLinkedInValidator, customEmailAndPhoneNumberValidator] });

  visible = true;
  isNewCandidate = false;

  @Input()
  isEditReferral = false;

  referralToEdit: Candidate;

  @Input()
  referralSelected: any;

  searchValue = '';
  listOfSearchProcesses = [];

  filteredCandidate: Candidate[] = [];
  listOfDisplayData = [...this.filteredCandidate];


  emptyCandidate: Candidate;
  emptyUser: User;
  editingCandidateId: number = 0;

  fileList: NzUploadFile[] = [];

  checkedTerms = false;

  service = null;

  constructor(private fb: FormBuilder,
    private facade: FacadeService,
    private modalService: NzModalService,
    private b: BaseService<Cv>,
    private router: Router,
    private referralsSandbox: ReferralsSandbox,
    public _referralsService: ReferralsService) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    this._referralsService._isReferralSource.subscribe(referral => this.isReferral = referral);
    if (this.communities) {
      this.filteredCommunity = this.communities.slice().sort((a, b) => (a.name.localeCompare(b.name)));
    }
    this.visible = this._visible;
    this.isNewCandidate = this.visible;
    if (this.referralSelected) {
      this.referralToEdit = this.referralSelected.candidate;
      this.referralId = this.referralToEdit.id;
      this.candidateForm.get('email').setAsyncValidators(UniqueEmailValidator(this.facade.referralsService, this.referralId));
    } else {
      this.candidateForm.get('email').setAsyncValidators(UniqueEmailValidator(this.facade.referralsService));
    }
    this.fillReferralForm(this.referralToEdit);
  }

  fillReferralForm(candidate: Candidate) {
    this.candidateForm.controls['openPositionTitle'].setValue(!!this.position ? this.position.title : null);
    if (this.position?.id) {
      this.candidateForm.controls['community'].setValue(this.position.community.id);
    }
    if (this.isEditReferral) {
      this.candidateForm.controls['firstName'].setValue(candidate.name);
      this.candidateForm.controls['lastName'].setValue(candidate.lastName);
      this.candidateForm.controls['phoneNumberPrefix'].setValue(candidate.phoneNumber.substring(1, candidate.phoneNumber.indexOf(')')));
      this.candidateForm.controls['phoneNumber'].setValue(candidate.phoneNumber.split(')')[1]);
      this.candidateForm.controls['email'].setValue(candidate.emailAddress);
      this.candidateForm.controls['community'].setValue(candidate.community.id);
      this.candidateForm.controls['link'].setValue(candidate.linkedInProfile);
      this.candidateForm.controls['firstName'].disable();
      this.candidateForm.controls['lastName'].disable();
    }
  }

  saveEdit() {
    this.referralId = this.referralToEdit.id;
    let isCompleted;
    if (this.candidateForm.invalid) {
      this.checkForm();
      isCompleted = false;
    }
    else {
      isCompleted = true;
    }

    if (isCompleted) {
      const editedCandidate = {
        id: this.referralToEdit.id,
        name: this.candidateForm.controls['firstName'].value.toString(),
        lastName: this.candidateForm.controls['lastName'].value.toString(),
        phoneNumber: '(' + this.candidateForm.controls['phoneNumberPrefix'].value.toString() + ')',
        dni: 0,
        emailAddress: this.candidateForm.controls['email'].value ? this.candidateForm.controls['email'].value.toString() : null,
        user: null,
        contactDay: new Date(),
        linkedInProfile: this.candidateForm.controls['link'].value ? this.candidateForm.controls['link'].value : null,
        englishLevel: 0,
        status: 0,
        candidateSkills: null,
        preferredOfficeId: 1,
        profile: null,
        community: new Community(this.candidateForm.controls['community'].value),
        isReferred: true,
        cv: this.referralToEdit.cv,
        knownFrom: null,
        referredBy: this.currentUser.username,
        source: 'A friend / colleague'
      };
      if (this.candidateForm.controls['phoneNumber'].value) {
        editedCandidate.phoneNumber += this.candidateForm.controls['phoneNumber'].value.toString();
      }

      let referralForStore = { ...this.referralSelected, candidate: editedCandidate };
      this.referralsSandbox.edit(referralForStore, this.referralToEdit.id, this.candidateForm.get('file').value);
      const errorSubscription = this.referralsSandbox.referralsLoadingError$
        .subscribe((res) => {
          if (!res) {
            this.facade.toastrService.success('Candidate was successfully edited !');
            this.isNewCandidate = false;
            this.visible = false;
            this.previousPosition.emit(null);
            this.facade.appService.stopLoading();
            this.modalService.closeAll();
          } else {
            this.facade.errorHandlerService.showErrorMessage(null);
            this.facade.appService.stopLoading();
          }
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
          this.facade.appService.stopLoading();
        });

      errorSubscription.unsubscribe();
    }
  }

  createNewCandidate() {
    let isCompleted = true;

    if (this.candidateForm.invalid) {
      this.checkForm();
      isCompleted = false;
    }
    else {
      isCompleted = true;
    }

    if (isCompleted) {
      this.facade.appService.startLoading();

      const newCandidate: Candidate = {
        id: 0,
        name: this.candidateForm.controls['firstName'].value.toString(),
        lastName: this.candidateForm.controls['lastName'].value.toString(),
        phoneNumber: '(' + this.candidateForm.controls['phoneNumberPrefix'].value.toString() + ')',
        dni: 0,
        emailAddress: this.candidateForm.controls['email'].value ? this.candidateForm.controls['email'].value.toString() : null,
        user: this.currentUser,
        contactDay: new Date(),
        linkedInProfile: this.candidateForm.controls['link'].value ? this.candidateForm.controls['link'].value : null,
        englishLevel: EnglishLevelEnum.None,
        status: CandidateStatusEnum.New,
        preferredOfficeId: null,
        candidateSkills: [],
        isReferred: this.isReferral,
        community: new Community(this.candidateForm.controls['community'].value),
        profile: null,
        cv: null,
        knownFrom: null,
        referredBy: this.isReferral ? this.currentUser.username : null,
        openPositionTitle: this.position ? this.position.title : null,
        openPosition: this.position ? this.position : null,
        source: this.isReferral ? 'A friend / colleague' : this.candidateForm.controls["source"].value
      };

      if (this.candidateForm.controls['phoneNumber'].value) {
        newCandidate.phoneNumber += this.candidateForm.controls['phoneNumber'].value.toString();
      }


      this.referralsSandbox.add(newCandidate, this.candidateForm.get('file').value);
      const errorSubscription = this.referralsSandbox.referralsLoadingError$
        .subscribe((res) => {
          if (!res) {
            this.facade.toastrService.success('Candidate was successfully created !');
            this.isNewCandidate = false;
            this.visible = false;
            this.previousPosition.emit(null);
            this.facade.appService.stopLoading();
            this.modalService.closeAll();
          } else {
            this.facade.errorHandlerService.showErrorMessage(null);
            this.facade.appService.stopLoading();
          }
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
          this.facade.appService.stopLoading();
        });

      errorSubscription.unsubscribe();
    }
  }

  checkForm() {
    for (const i in this.candidateForm.controls) {
      this.candidateForm.controls[i].markAsDirty();
      if (this.candidateForm.controls[i] !== this.candidateForm.controls['email'] ||
        this.candidateForm.controls[i] !== this.candidateForm.controls['email']) {
        this.candidateForm.controls[i].updateValueAndValidity();
      }
      //this.candidateForm.controls[i].updateValueAndValidity();
    }
    //this.candidateForm.updateValueAndValidity();
  }

  clearDataAndCloseModal() {
    this.facade.modalService.openModals[0].destroy();
    this.previousPosition.emit(null);
    this._referralsService.resetIsReferral();
  }


  beforeUpload = (file: NzUploadFile): boolean => {
    let fileExtension = file.name.split('.')[1].toLowerCase(),
      fileSize = file.size;

    if (fileExtension === 'pdf' && fileSize < 6300000) {
      this.candidateForm.get('file').setValue(file);
      this.fileList = this.fileList.concat(file);
    } else if (fileExtension !== 'pdf') {
      this.facade.toastrService.error('File format must be PDF');
    } else if (fileSize > 6300000) {
      this.facade.toastrService.error('File size must be 6Mb');
    }

    return false;
  };
}
