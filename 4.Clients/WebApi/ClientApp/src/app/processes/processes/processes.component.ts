import { Component, OnInit, ViewChild, TemplateRef, AfterViewChecked, OnDestroy } from '@angular/core';
import { Process } from 'src/entities/process';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FacadeService } from 'src/app/services/facade.service';
import { Candidate } from 'src/entities/candidate';
import { CandidateDetailsComponent } from 'src/app/candidates/details/candidate-details.component';
import { UserDetailsComponent } from 'src/app/users/details/user-details.component';
import { AppComponent } from 'src/app/app.component';
import { Stage } from 'src/entities/stage';
import { CandidateAddComponent } from 'src/app/candidates/add/candidate-add.component';
import { HrStageComponent } from 'src/app/stages/hr-stage/hr-stage.component';
import { ClientStageComponent } from 'src/app/stages/client-stage/client-stage.component';
import { OfferStageComponent } from 'src/app/stages/offer-stage/offer-stage.component';
import { HireStageComponent } from 'src/app/stages/hire-stage/hire-stage.component';
import { TechnicalStageComponent } from 'src/app/stages/technical-stage/technical-stage.component';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
import { SeniorityEnum } from '../../../entities/enums/seniority.enum';
import { Globals } from '../../app-globals/globals';
import { CandidateStatusEnum } from '../../../entities/enums/candidate-status.enum';
import { StageStatusEnum } from '../../../entities/enums/stage-status.enum';
import { EnglishLevelEnum } from '../../../entities/enums/english-level.enum';
import { Office } from 'src/entities/office';
import { Community } from 'src/entities/community';
import { CandidateProfile } from 'src/entities/Candidate-Profile';
import { replaceAccent } from 'src/app/helpers/string-helpers';
import { ProcessCurrentStageEnum } from 'src/entities/enums/process-current-stage';
import { User } from 'src/entities/user';
import { SlickComponent } from 'ngx-slick';
import { DeclineReason } from 'src/entities/declineReason';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-processes',
  templateUrl: './processes.component.html',
  styleUrls: ['./processes.component.css'],
  providers: [CandidateDetailsComponent, UserDetailsComponent, AppComponent]
})

export class ProcessesComponent implements OnInit, AfterViewChecked, OnDestroy {
  slideConfig = {
    slidesToShow: 1,
    adaptiveHeight: true,
    arrows: true,
    infinite: false,
    draggable: false
  };

  @ViewChild('slickModal') slickModal: SlickComponent;
  @ViewChild('dropdown') nameDropdown;
  @ViewChild('dropdownStatus') statusDropdown;
  @ViewChild('dropdownCurrentStage') currentStageDropdown;
  @ViewChild('processCarousel') processCarousel;
  @ViewChild(CandidateAddComponent) candidateAdd: CandidateAddComponent;
  @ViewChild(HrStageComponent) hrStage: HrStageComponent;
  @ViewChild(TechnicalStageComponent) technicalStage: TechnicalStageComponent;
  @ViewChild(ClientStageComponent) clientStage: ClientStageComponent;
  @ViewChild(OfferStageComponent) offerStage: OfferStageComponent;
  @ViewChild(HireStageComponent) hireStage: HireStageComponent;

  filteredProcesses: Process[] = [];
  searchValue = '';
  searchRecruiterValue = '';
  searchValueStatus = '';
  searchValueCurrentStage = '';
  listOfSearchProcesses = [];
  listOfDisplayData = [...this.filteredProcesses];
  listOfDisplayOwnData = [...this.filteredProcesses];
  currentUser: User;
  sortName = null;
  sortValue = null;
  processForm: FormGroup;
  rejectProcessForm: FormGroup;
  declineProcessForm: FormGroup;
  isDetailsVisible = false;
  emptyProcess: Process;
  availableCandidates: Candidate[] = [];
  candidatesFullList: Candidate[] = [];
  users: User[] = [];
  profileSearch = 0;
  profileSearchName = 'ALL';
  communitySearch = 0;
  communitySearchName = 'ALL';
  profileList: any[];
  statusList: any[];
  currentStageList: any[];
  emptyCandidate: Candidate;
  emptyUser: any;
  currentCandidate: Candidate;
  isEdit = false;
  openFromEdit = false;
  currentComponent: string;
  lastComponent: string;
  times = 0;
  selectedSeniority: SeniorityEnum;
  offices: Office[] = [];
  communities: Community[] = [];
  profiles: CandidateProfile[] = [];
  stepIndex = 0;
  declineReasons: DeclineReason[] = [];
  isDeclineReasonOther = false;
  isOwnedProcesses = false;
  forms: FormGroup[] = [];
  isLoading = false;
  searchSub: Subscription;
  constructor(private facade: FacadeService, private formBuilder: FormBuilder, private app: AppComponent,
    private candidateDetailsModal: CandidateDetailsComponent, private userDetailsModal: UserDetailsComponent,
    private globals: Globals, private _appComponent: AppComponent, ) {
    this.profileList = globals.profileList;
    this.statusList = globals.processStatusList;
    this.currentStageList = globals.processCurrentStageList;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.app.removeBgImage();
    this.getUserProcesses();
    this.getCandidates();
    this.getUsers();
    this.getOffices();
    this.getCommunities();
    this.getProfiles();
    this.getDeclineReasons();
    this.getSearchInfo();
    this.rejectProcessForm = this.formBuilder.group({
      rejectionReasonDescription: [null, [Validators.required]]
    });
    this.declineProcessForm = this.formBuilder.group({
      declineReasonDescription: [null, [Validators.required]],
      declineReasonName: [null, [Validators.required]]
    });
    this.app.hideLoading();
  }

  ngAfterViewChecked() {
    if (this.slickModal && this.openFromEdit) {
      this.slickModal.slickGoTo(this.stepIndex);
      this.openFromEdit = false;
    }
  }

  isUserRole(roles: string[]): boolean {
    return this._appComponent.isUserRole(roles);
  }

  getCandidates() {
    this.facade.candidateService.get()
      .subscribe(res => {
        this.availableCandidates = res.filter(x => x.status === CandidateStatusEnum.New || x.status === CandidateStatusEnum.Recall);
        this.candidatesFullList = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getUsers() {
    this.facade.userService.get()
      .subscribe(res => {
        this.users = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getOffices() {
    this.facade.OfficeService.get()
      .subscribe(res => {
        this.offices = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getCommunity(community: number): string {
    return this.communities.find(x => x.id === community).name;
  }

  getCommunities() {
    this.facade.communityService.get()
      .subscribe(res => {
        this.communities = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getProfiles() {
    this.facade.candidateProfileService.get()
      .subscribe(res => {
        this.profiles = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getDeclineReasons() {
    this.facade.declineReasonService.get('Named')
      .subscribe(res => {
        this.declineReasons = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getProfile(profile: number): string {
    return this.profiles.filter(x => x.id === profile)[0].name;
  }

  getProcesses() {
    this.facade.processService.getProcessByUserRole(this.currentUser)
      .subscribe(res => {
        this.filteredProcesses = res;
        this.listOfDisplayData = res;
        const newProc: Process = res[res.length - 1];
        if (newProc && newProc.candidate) {
          this.candidatesFullList.push(newProc.candidate);
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getUserProcesses() {
    this.facade.processService.get()
      .subscribe(res => {
        const result = [];
        for (let i = 0; i < res.length; i++) {
          if (res[i].candidate.user !== null && typeof res[i].candidate.user !== 'undefined') {
            const sessionConsultant = res[i].candidate.user.firstName + ' ' + res[i].candidate.user.lastName;
            if (sessionConsultant === this.currentUser.firstName + ' ' + this.currentUser.lastName) {
              result.push(res[i]);
            }
          }
        }
        this.listOfDisplayOwnData = result;
        const newProc: Process = result[result.length - 1];

        if (newProc && newProc.candidate) {
          this.candidatesFullList.push(newProc.candidate);
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }
  getSearchInfo() {
    this.searchSub = this.facade.searchbarService.searchChanged.subscribe(data => {

      this.listOfDisplayData = this.filteredProcesses;
      this.listOfDisplayOwnData = this.filteredProcesses;

      const allProcesses = this.listOfDisplayData.filter(process => {
        const fullName = process.candidate.name + process.candidate.lastName;
        const value = data.toString().toUpperCase();
        return fullName.toString().toUpperCase().indexOf(value) !== -1;
      });
      const ownProcesses = this.listOfDisplayOwnData.filter(process => {
        const fullName = process.candidate.name + process.candidate.lastName;
        const value = data.toString().toUpperCase();
        return fullName.toString().toUpperCase().indexOf(value) !== -1;
      });

      this.listOfDisplayData = allProcesses;
      this.listOfDisplayOwnData = ownProcesses.filter(res =>{
          if (res.candidate.user !== null && typeof res.candidate.user !== 'undefined') {
            const sessionConsultant = res.candidate.user.firstName + ' ' + res.candidate.user.lastName;
            if (sessionConsultant === this.currentUser.firstName + ' ' + this.currentUser.lastName) {
              return res;
            }
          }
      });
    });
    
  }
  showApproveProcessConfirm(processID: number): void {
    const procesToApprove: Process = this.filteredProcesses.find(p => p.id === processID);
    const processText = procesToApprove.candidate.name.concat(' ').concat(procesToApprove.candidate.lastName);
    const title = 'Are you sure you want to approve the process for ' +
      processText + '? This will approve all stages associated with the process';
    this.facade.modalService.confirm({
      nzTitle: title,
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.approveProcess(processID)
    });
  }

  approveProcess(processID: number) {
    this.app.showLoading();
    this.facade.processService.approve(processID)
      .subscribe(res => {
        this.getProcesses();
        this.getCandidates();
        this.app.hideLoading();
        this.facade.toastrService.success('Process approved!');
      }, err => {
        this.app.hideLoading();
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  showStepsModal(process: Process): void {
    this.emptyProcess = process;
    this.isDetailsVisible = true;
  }

  rejectProcess(processID: number, modalContent: TemplateRef<{}>) {
    this.rejectProcessForm.reset();
    const process: Process = this.filteredProcesses.filter(p => p.id === processID)[0];
    const modal = this.facade.modalService.create({
      nzTitle: 'Are you sure you want to reject the process for ' + process.candidate.name + ' ' + process.candidate.lastName + '?',
      nzContent: modalContent,
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Submit',
          type: 'danger',
          onClick: () => {
            this.app.showLoading();
            let isCompleted = true;
            for (const i in this.rejectProcessForm.controls) {
              if (this.rejectProcessForm.controls.hasOwnProperty(i)) {
                this.rejectProcessForm.controls[i].markAsDirty();
                this.rejectProcessForm.controls[i].updateValueAndValidity();
                if (!this.rejectProcessForm.controls[i].valid) {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {
              const rejectionReason = this.rejectProcessForm.controls['rejectionReasonDescription'].value.toString();
              this.facade.processService.reject(processID, rejectionReason)
                .subscribe(res => {
                  this.getCandidates();
                  this.getProcesses();
                  this.app.hideLoading();
                  modal.destroy();
                  this.facade.toastrService.success('Process and associated candidate were rejected');
                }, err => {
                  this.app.hideLoading();
                  this.facade.toastrService.error(err.message);
                });
            }
            this.app.hideLoading();
          }
        }
      ]
    });
  }

  /**Opens modal for entering a process declination reason, which updates process upon pressing OK.*/
  openDeclineModal(process: Process, modalContent: TemplateRef<{}>) {
    this.declineProcessForm.reset();
    const modal = this.facade.modalService.create({
      nzTitle: 'Are you sure you want to decline the process for ' + process.candidate.name + ' ' + process.candidate.lastName + '?',
      nzContent: modalContent,
      // added this because it was showing behind the process edit modal, might have been caused by an unrelated issue though
      nzZIndex: 5,
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => modal.destroy()
        },
        {
          label: 'Submit',
          type: 'primary',
          onClick: () => {
            this.app.showLoading();
            let isCompleted = true;
            for (const i in this.declineProcessForm.controls) {
              if (this.declineProcessForm.controls.hasOwnProperty(i)) {
                this.declineProcessForm.controls[i].markAsDirty();
                this.declineProcessForm.controls[i].updateValueAndValidity();
                if (!this.declineProcessForm.controls[i].valid && this.declineProcessForm.controls[i].enabled) {
                  isCompleted = false;
                }
              }
            }
            if (isCompleted) {
              const declineReason: DeclineReason = {
                id: this.declineProcessForm.controls['declineReasonName'].value,
                name: '',
                description: this.declineProcessForm.controls['declineReasonDescription'].enabled ?
                  this.declineProcessForm.controls['declineReasonDescription'].value.toString() : ''
              };
              process.declineReason = declineReason;
              this.facade.processService.update(process.id, process)
                .subscribe(res => {
                  this.app.hideLoading();
                  modal.destroy();
                  this.facade.toastrService.success('Process and associated candidate were declined');
                }, err => {
                  this.app.hideLoading();
                  this.facade.toastrService.error(err.message);
                });
            }
            this.app.hideLoading();
          }
        }
      ]
    });
    return modal;
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  resetRecruiter(): void {
    this.searchRecruiterValue = '';
    this.searchRecruiter();
  }

  resetStatus(): void {
    this.searchValueStatus = '';
    this.searchStatus();
  }

  resetCurrentStage(): void {
    this.searchValueCurrentStage = '';
    this.searchCurrentStage();
  }

  triggerSearch(val) {
    this.searchValue = val;
    this.search();
  }

  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchProcesses.length ? this.listOfSearchProcesses.some(p => (item.candidate.name.toString() + ' ' + item.candidate.lastName.toString()).indexOf(p) !== -1) : true) && (replaceAccent(item.candidate.name.toString() + ' ' + item.candidate.lastName.toString()).toUpperCase().indexOf(replaceAccent(this.searchValue).toUpperCase()) !== -1);
    };
    const data = this.filteredProcesses.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.communitySearchName = 'ALL';
    this.profileSearchName = 'ALL';
    this.nameDropdown.nzVisible = false;
  }

  triggerSearchRecruiter(val) {
    this.searchRecruiterValue = val;
    this.searchRecruiter();
  }

  searchRecruiter(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchProcesses.length ? this.listOfSearchProcesses.some(p => (item.candidate.user.firstName.toString() + ' ' + item.candidate.user.lastName.toString()).indexOf(p) !== -1) : true) &&
        (replaceAccent(item.candidate.user.firstName.toString() + ' ' + item.candidate.user.lastName.toString()).toUpperCase().indexOf(replaceAccent(this.searchRecruiterValue).toUpperCase()) !== -1);
    };
    const data = this.filteredProcesses.filter(item => filterFunc(item));
    this.listOfDisplayOwnData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.communitySearchName = 'ALL';
    this.profileSearchName = 'ALL';
  }

  searchOwnRecruiter(): void {
    this.searchRecruiterValue = this.currentUser.firstName + ' ' + this.currentUser.lastName;
    this.searchRecruiter();
    this.isOwnedProcesses = true;
  }

  searchAllProcess() {
    this.getProcesses();
    this.isOwnedProcesses = false;
  }

  showOwnProcessesFirst(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchProcesses.length ? this.listOfSearchProcesses.some(p => (item.candidate.user.firstName.toString() + ' ' + item.candidate.user.lastName.toString()).indexOf(p) !== -1) : true) &&
        (replaceAccent(item.candidate.user.firstName.toString() + ' ' + item.candidate.user.lastName.toString()).toUpperCase().indexOf(replaceAccent(this.searchRecruiterValue).toUpperCase()) !== -1);
    };
    const data = this.filteredProcesses.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.communitySearchName = 'ALL';
    this.profileSearchName = 'ALL';
    this.nameDropdown.nzVisible = false;
  }

  triggerSearchStatus(id) {
    this.searchValueStatus = id;
    this.searchStatus();
  }

  searchStatus(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchProcesses.length ? this.listOfSearchProcesses.some(p => item.status.indexOf(p) !== -1) : true) &&
        (item.status === this.searchValueStatus);
    };
    const data = this.searchValueStatus !== '' ? this.filteredProcesses.filter(item => filterFunc(item)) : this.filteredProcesses;
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.searchValueStatus = '';
  }

  triggerSearchCurrentStage(id) {
    this.searchValueCurrentStage = id;
    this.searchCurrentStage();
  }

  searchCurrentStage(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchProcesses.length ? this.listOfSearchProcesses.some(p => item.currentStage.indexOf(p) !== -1) : true) &&
        (item.currentStage === this.searchValueCurrentStage);
    };
    const data = this.searchValueCurrentStage !== '' ? this.filteredProcesses.filter(item => filterFunc(item)) : this.filteredProcesses;
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.searchValueCurrentStage = '';
  }

  searchProfile(searchedProfile: number) {
    this.profileSearch = searchedProfile;
    if (this.profileSearch === 0) {
      this.listOfDisplayData = this.filteredProcesses;
      this.profileSearchName = 'ALL';
    } else {
      this.profileSearchName = (this.profiles.find(p => p.id === this.profileSearch)).name;
      this.listOfDisplayData = this.filteredProcesses.filter(p => p.candidate.profile.id === searchedProfile);
      this.communitySearchName = 'ALL';
    }
  }

  searchCommunity(searchedCommunity: number) {
    this.communitySearch = searchedCommunity;
    if (this.communitySearch === 0) {
      this.listOfDisplayData = this.filteredProcesses;
      this.communitySearchName = 'ALL';
    } else {
      this.communitySearchName = (this.communities.filter(p => p.id === this.communitySearch))[0].name;
      this.communitySearchName = (this.communities.filter(p => p.id === this.communitySearch))[0].name;
      this.listOfDisplayData = this.filteredProcesses.filter(p => p.candidate.community.id === searchedCommunity);
      this.profileSearchName = 'ALL';
    }
  }

  sort(sortName: string, value: boolean): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  showProcessStart(modalContent: TemplateRef<{}>, footer: TemplateRef<{}>, processId: number): void {
    this.app.showLoading();
    if (processId > -1) {
      this.emptyProcess = this.filteredProcesses.filter(p => p.id === processId)[0];
      this.isEdit = true;
      this.openFromEdit = true;
      if (this.currentUser.role === 'Admin' || this.currentUser.role === 'Recruiter') {
        this.emptyProcess.currentStage === ProcessCurrentStageEnum.Finished ? this.stepIndex = ProcessCurrentStageEnum.OfferStage : this.stepIndex = this.emptyProcess.currentStage;
      } else {
        this.stepIndex = 0;
      }
    } else {
      this.emptyProcess = undefined;
    }
    const modal = this.facade.modalService.create({
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzWidth: '90%',
      nzFooter: footer
    });
    this.app.hideLoading();
  }

  newProcessStart(modalContent: TemplateRef<{}>, footer: TemplateRef<{}>, candidate?: Candidate): void {
    this.app.showLoading();
    if (!candidate) {
      const newCandidate: Candidate = {
        id: null,
        name: '',
        lastName: '',
        dni: 0,
        emailAddress: '',
        phoneNumber: null,
        additionalInformation: '',
        englishLevel: null,
        status: null,
        candidateSkills: [],
        user: null,
        preferredOfficeId: null,
        contactDay: null,
        profile: null,
        community: null,
        isReferred: false,
        linkedInProfile: null,
        referredBy: null,
        knownFrom: null,
        cv: null,
      };
      this.currentCandidate = newCandidate;
    } else {
      this.currentCandidate = candidate;
    }
    this.createEmptyProcess(this.currentCandidate);
    const modal = this.facade.modalService.create({
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzWidth: '90%',
      nzFooter: footer
    });
    this.app.hideLoading();
  }

  showCandidateDetailsModal(candidateID: number, modalContent: TemplateRef<{}>): void {
    this.emptyCandidate = this.candidatesFullList.filter(candidate => candidate.id === candidateID)[0];
    this.candidateDetailsModal.showModal(modalContent, this.emptyCandidate.name + ' ' + this.emptyCandidate.lastName);
  }

  showUserDetailsModal(userID: number, modalContent: TemplateRef<{}>): void {
    this.emptyUser = this.users.filter(user => user.id == userID)[0];
    this.userDetailsModal.showModal(modalContent, this.emptyUser.firstName);
  }

  showDeleteConfirm(processID: number): void {
    const procesDelete: Process = this.filteredProcesses.find(p => p.id === processID);
    const processText = procesDelete.candidate.name.concat(' ').concat(procesDelete.candidate.lastName);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure delete the process for ' + processText + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.processService.delete(processID)
        .subscribe(res => {
          this.getProcesses();
          this.facade.toastrService.success('Process was deleted !');
        }, err => {
          this.facade.toastrService.error(err.message);
        })
    });
  }

  onCheck(): number {
    let i = 0;
    let carouselSlide = -1;
    this.forms.forEach(form => {
      form = this.checkForm(form);
      if (carouselSlide === -1 && form.invalid) {
        carouselSlide = i;
      }
      i++;
    });
    return carouselSlide;
  }

  checkForm(form: FormGroup): FormGroup {
    for (const i in form.controls) {
      if (form.controls[i]) {
        form.controls[i].markAsDirty();
        form.controls[i].updateValueAndValidity();
      }
    }
    return form;
  }

  validateForms(): boolean {
    this.getForms(this.stepIndex);
    const formNumber: number = this.onCheck();
    if (formNumber > -1) {
      this.checkSlideIndex(this.getInvalidFormSlide(formNumber));
      return false;
    } else {
      return true;
    }
  }
  getInvalidFormSlide(slide: number) {
    switch (slide) {
      case 0:
        this.slickModal.slickGoTo(0);
        this.stepIndex = 0;
        return 'candidateButton';
      case 1:
        this.slickModal.slickGoTo(0);
        this.stepIndex = 0;
        return 'candidateButton';
      case 2:
        this.slickModal.slickGoTo(1);
        this.stepIndex = 1;
        return 'technicalButton';
      case 3:
        this.slickModal.slickGoTo(2);
        this.stepIndex = 2;
        return 'clientButton';
      case 4:
        this.slickModal.slickGoTo(3);
        this.stepIndex = 3;
        return 'clientButton';
      default:
        return 'hireButton';
    }
  }

  wishedStage(choosenStage: number, elementName: string) {
    this.stepIndex = choosenStage;
    this.checkSlideIndex(elementName);
  }

  closeModal() {
    this.facade.modalService.openModals[0].destroy();
    this.isEdit = false;
    this.stepIndex = 0;
  }

  getForms(slide: number) {
    const allForms = [];
    const currentSlideForm = slide + 2;
    allForms.push(this.candidateAdd.candidateForm);
    allForms.push(this.hrStage.hrForm);
    allForms.push(this.technicalStage.technicalForm);
    allForms.push(this.clientStage.clientForm);
    allForms.push(this.offerStage.offerForm);
    this.forms = [...allForms.slice(0, currentSlideForm)];
  }

  checkSlideIndex(elementName: string) {
    this.currentComponent = elementName;
    if (this.times === 0) {
      this.lastComponent = this.currentComponent;
    }
    document.getElementById('candidateButton').style.borderWidth = '0px';
    document.getElementById('candidateButton').style.borderColor = 'none';

    document.getElementById(this.lastComponent).style.borderWidth = '0px';
    document.getElementById(this.lastComponent).style.borderColor = 'none';
    document.getElementById(this.currentComponent).style.borderWidth = '3px';
    document.getElementById(this.currentComponent).style.borderColor = 'red';
    document.getElementById(this.currentComponent).style.borderRadius = '6px';

    this.times++;
    this.lastComponent = this.currentComponent;
  }

  saveProcess(declineProcessModal: TemplateRef<{}>) {
    if (this.validateForms()) {
      this.app.showLoading();
      let newCandidate: Candidate;
      let newProcess: Process;
      this.isLoading = true;
      newCandidate = this.candidateAdd.getFormData();
      newCandidate.candidateSkills = this.technicalStage.getFormDataSkills();
      newProcess = this.getProcessFormData();
      newProcess.userOwnerId = newCandidate.user.id;
      newProcess.candidate = newCandidate;
      newProcess = this.generateProcess(newProcess, newCandidate);
      if (!this.isEdit) {
        if (!newCandidate.id) {
          this.facade.candidateService.add(newCandidate).subscribe(res => {
            newProcess.candidate.id = res.id;
            this.facade.processService.add(newProcess)
              .subscribe(() => {
                this.isLoading = false;
                this.getProcesses();
                this.app.hideLoading();
                this.facade.toastrService.success('The process was successfully saved !');
                this.createEmptyProcess(newCandidate);
                this.closeModal();
              }, err => {
                this.isLoading = false;
                this.app.hideLoading();
                this.facade.toastrService.error(err);
              });
          }, err => {
            this.isLoading = false;
            this.facade.errorHandlerService.showErrorMessage(err);
          });
        } else {
          this.facade.processService.add(newProcess)
            .subscribe(res => {
              newCandidate.status = CandidateStatusEnum.InProgress;
              this.isLoading = false;
              this.getProcesses();
              this.app.hideLoading();
              this.facade.toastrService.success('The process was successfully saved !');
              this.createEmptyProcess(newCandidate);
              this.closeModal();
            }, err => {
              this.isLoading = false;
              this.app.hideLoading();
              this.facade.toastrService.error(err);
            });
        }
      } else {
        this.facade.processService.getByID(newProcess.id)
          .subscribe(res => {
            if (res.status !== ProcessStatusEnum.Declined && this.isDeclined(newProcess)) {
              // Used for verifying whether user pressed OK or Cancel on decline modal.
              const declineReason = newProcess.declineReason;
              this.openDeclineModal(newProcess, declineProcessModal).afterClose
                .subscribe(sel => {
                  if (declineReason !== newProcess.declineReason) {
                    this.getCandidates();
                    this.getProcesses();
                    this.app.hideLoading();
                    this.facade.toastrService.success('The process was successfully saved!');
                    this.createEmptyProcess(newCandidate);
                    this.closeModal();
                  }
                });
            } else {
              this.facade.processService.update(newProcess.id, newProcess)
                .subscribe(() => {
                  this.getProcesses();
                  this.getCandidates();
                  this.app.hideLoading();
                  this.facade.toastrService.success('The process was successfully saved !');
                  this.createEmptyProcess(newCandidate);
                  this.closeModal();
                }, err => {
                  this.app.hideLoading();
                  this.facade.toastrService.error(err.message);
                });
            }
          }, err => {
            this.app.hideLoading();
            this.facade.toastrService.error(err.message);
          });
      }
    }
  }

  getProcessFormData(): Process {
    let process: Process;
    process = {
      id: !this.isEdit ? 0 : this.emptyProcess.id,
      startDate: new Date(),
      endDate: null,
      status: !this.isEdit ? ProcessStatusEnum.InProgress : ProcessStatusEnum[CandidateStatusEnum[this.emptyProcess.candidate.status]],
      currentStage: ProcessCurrentStageEnum.NA,
      candidateId: !this.isEdit ? 0 : null,
      candidate: null,
      userOwnerId: 0,
      userOwner: null,
      userDelegate: null,
      userDelegateId: null,
      rejectionReason: null,
      declineReason: null,
      actualSalary: 0,
      wantedSalary: 0,
      englishLevel: EnglishLevelEnum.None,
      seniority: 0,
      hrStage: null,
      technicalStage: null,
      clientStage: null,
      offerStage: null,
      createdDate: new Date()
    };

    process.hrStage = this.hrStage.getFormData(process.id);
    process.technicalStage = this.technicalStage.getFormData(process.id);
    process.clientStage = this.clientStage.getFormData(process.id);
    process.offerStage = this.offerStage.getFormData(process.id);
    // Seniority is now handled global between technical stage and offer stage. The process uses the last updated value.
    process.seniority = this.selectedSeniority ? this.selectedSeniority :
      (process.technicalStage.seniority ? process.technicalStage.seniority :
        process.technicalStage.alternativeSeniority ? process.technicalStage.alternativeSeniority :
          (process.offerStage.seniority));
    process.englishLevel = process.englishLevel;
    return process;
  }

  getProcessStatus(stages: Stage[]): number {
    let processStatus = 0;
    stages.forEach(stage => {
      if (stage.status === StageStatusEnum.InProgress) { processStatus = ProcessStatusEnum.InProgress; }
      if (stage.status === StageStatusEnum.Accepted) { processStatus = ProcessStatusEnum.InProgress; }
      if (stage.status === StageStatusEnum.Declined) { processStatus = ProcessStatusEnum.Declined; }
    });
    if (stages[3].status === StageStatusEnum.Accepted) { processStatus = ProcessStatusEnum.OfferAccepted; }

    if (stages[3].status === StageStatusEnum.Accepted && stages[4].status === StageStatusEnum.InProgress ||
      stages[3].status === StageStatusEnum.Accepted && stages[4].status === StageStatusEnum.Declined) { processStatus = ProcessStatusEnum.Declined; }

    if (stages[4].status === StageStatusEnum.Accepted) { processStatus = ProcessStatusEnum.Hired; }

    return processStatus;
  }

  getStatusColor(status: number): string {
    const statusName = this.statusList.filter(s => s.id === status)[0].name;
    switch (statusName) {
      case 'Hired': return 'success';
      case 'Rejected': return 'error';
      case 'Declined': return 'error';
      case 'In Progress': return 'processing';
      case 'Recall': return 'warning';
      default: return 'default';
    }
  }

  showContactCandidatesModal(modalContent: TemplateRef<{}>) {
    const modal = this.facade.modalService.create({
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzWidth: '90%',
      nzFooter: [
        {
          label: 'Cancel',
          shape: 'default',
          onClick: () => {
            modal.destroy();
            this.refreshTable();
          }
        }]
    });
  }

  refreshTable() {
    this.getProcesses();
    this.getCandidates();
  }

  updateSeniority($event) {
    this.selectedSeniority = $event;
  }

  onStepIndexChange(index: number): void {
    this.stepIndex = index;
  }

  createEmptyProcess(candidate: Candidate) {
    this.emptyProcess = {
      id: 0,
      startDate: new Date(),
      endDate: null,
      status: ProcessStatusEnum.NA,
      currentStage: ProcessCurrentStageEnum.NA,
      candidateId: candidate.id,
      candidate: candidate,
      userOwnerId: null,
      userOwner: candidate.user,
      userDelegateId: null,
      userDelegate: null,
      rejectionReason: null,
      declineReasonId: null,
      declineReason: null,
      actualSalary: 0,
      wantedSalary: 0,
      englishLevel: EnglishLevelEnum.None,
      seniority: 0,
      createdDate: new Date(),
      hrStage: {
        id: 0,
        date: new Date(),
        status: StageStatusEnum.InProgress,
        feedback: '',
        userOwnerId: null,
        userDelegateId: null,
        processId: 0,
        actualSalary: null,
        wantedSalary: null,
        englishLevel: EnglishLevelEnum.None,
        rejectionReasonsHr: null
      },
      technicalStage: {
        id: 0,
        date: new Date(),
        status: StageStatusEnum.NA,
        feedback: '',
        userOwnerId: null,
        userDelegateId: null,
        processId: 0,
        seniority: SeniorityEnum.NA,
        alternativeSeniority: SeniorityEnum.NA,
        client: ''
      },
      clientStage: {
        id: 0,
        date: new Date(),
        status: StageStatusEnum.NA,
        feedback: '',
        userOwnerId: null,
        userDelegateId: null,
        processId: 0,
        interviewer: '',
        delegateName: ''
      },
      offerStage: {
        id: 0,
        date: new Date(),
        status: StageStatusEnum.NA,
        feedback: '',
        userOwnerId: null,
        userDelegateId: null,
        processId: 0,
        seniority: SeniorityEnum.NA,
        hireDate: new Date(),
        backgroundCheckDone: false,
        backgroundCheckDoneDate: new Date(),
        preocupationalDone: false,
        preocupationalDoneDate: new Date()
      },
    };
  }
  generateProcess(process: Process, candidate: Candidate) {
    const userId = candidate.user.id;
    process.candidateId = candidate.id;
    process.candidate = candidate;
    process.userOwnerId = userId;
    process.userOwner = candidate.user;
    process.hrStage.userOwnerId = userId;
    process.hrStage.userDelegateId = userId;
    process.technicalStage.userOwnerId = userId;
    process.technicalStage.userDelegateId = userId;
    process.clientStage.userOwnerId = userId;
    process.clientStage.userDelegateId = userId;
    process.offerStage.userOwnerId = userId;
    return process;
  }

  isDeclined(process: Process): Boolean {
    return (process.hrStage.status === StageStatusEnum.Declined ||
      process.technicalStage.status === StageStatusEnum.Declined ||
      process.clientStage.status === StageStatusEnum.Declined ||
      process.offerStage.status === StageStatusEnum.Declined) ? true : false;
  }

  declineReasonNameChanged() {
    if (this.declineProcessForm.controls['declineReasonName'].value === -1) {
      this.isDeclineReasonOther = true;
      this.declineProcessForm.controls['declineReasonDescription'].enable();
    } else {
      this.isDeclineReasonOther = false;
      this.declineProcessForm.controls['declineReasonDescription'].disable();
    }
  }

  ngOnDestroy() {
    this.searchSub.unsubscribe();
  }
}
