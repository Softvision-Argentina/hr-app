import { Component, OnInit, ViewChild, TemplateRef, AfterViewChecked, OnDestroy } from '@angular/core';
import { Process } from 'src/entities/process';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FacadeService } from 'src/app/services/facade.service';
import { Candidate } from 'src/entities/candidate';
import { UserDetailsComponent } from 'src/app/users/details/user-details.component';
import { AppComponent } from 'src/app/app.component';
import { Stage } from 'src/entities/stage';
import { CandidateAddComponent } from 'src/app/candidates/add/candidate-add.component';
import { HrStageComponent } from 'src/app/stages/hr-stage/hr-stage.component';
import { ClientStageComponent } from 'src/app/stages/client-stage/client-stage.component';
import { PreOfferStageComponent } from 'src/app/stages/pre-offer-stage/pre-offer-stage.component';
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
import { SlickCarouselComponent } from 'ngx-slick-carousel';
import { DeclineReason } from 'src/entities/declineReason';
import { Subscription } from 'rxjs';
import { HealthInsuranceEnum } from 'src/entities/enums/health-insurance.enum';
import { ReferralsService } from 'src/app/services/referrals.service';
import { Router, ActivatedRoute } from '@angular/router';
import { OpenPosition } from 'src/entities/open-position';
import { nullSafeIsEquivalent } from '@angular/compiler/src/output/output_ast';
import { PositionAddComponent } from '../position-add/position-add.component';
import { ReferralsContactComponent } from '../referrals-contact/referrals-contact.component';

@Component({
  selector: 'app-referrals',
  templateUrl: './referrals.component.html',
  styleUrls: ['./referrals.component.scss'],
  providers: [UserDetailsComponent, AppComponent]
})

export class ReferralsComponent implements OnInit, AfterViewChecked, OnDestroy {
  slideConfig = {
    slidesToShow: 1,
    adaptiveHeight: true,
    arrows: true,
    infinite: false,
    draggable: false,
    prevArrow: '<button style="left: -45px!important;" class="slick-prev"><img style="transform: scaleX(-1);" src="../../assets/images/arrow_medium.svg"></button>',
    nextArrow: '<button class="slick-next"><img src="../../assets/images/arrow_medium.svg"></button>'
    
  };

  @ViewChild('slickJobDescription') slickJobD: SlickCarouselComponent;  
  @ViewChild('dropdown') nameDropdown;
  @ViewChild('dropdownStatus') statusDropdown;
  @ViewChild('dropdownCurrentStage') currentStageDropdown;
  @ViewChild('newCandidate') newCandidate;

  @ViewChild('processCarousel') processCarousel;
  @ViewChild(CandidateAddComponent) candidateAdd: CandidateAddComponent;
  @ViewChild(HrStageComponent) hrStage: HrStageComponent;
  @ViewChild(TechnicalStageComponent) technicalStage: TechnicalStageComponent;
  @ViewChild(ClientStageComponent) clientStage: ClientStageComponent;
  @ViewChild(PreOfferStageComponent) preOfferStage: PreOfferStageComponent;
  @ViewChild(OfferStageComponent) offerStage: OfferStageComponent;
  @ViewChild(HireStageComponent) hireStage: HireStageComponent;
  @ViewChild('newReferralsButton') newReferralsButton: any;
  @ViewChild('newPositionButton') newPositionButton: any;

  filteredProcesses: Process[] = [];
  searchValue = '';
  searchRecruiterValue = '';
  searchValueStatus = '';
  searchValueCurrentStage = '';
  listOfSearchProcesses = [];
  listOfDisplayData = [...this.filteredProcesses];
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
  candidateReferred: Candidate[] = [];

  users: User[] = [];

  profileSearch = 0;
  profileSearchName = 'ALL';
  communitySearch = 0;
  communitySearchName = 'ALL';
  profileList: any[];
  statusList: any[];
  currentStageList: any[];
  emptyCandidate: Candidate;
  emptyUser: User;
  currentCandidate: Candidate;
  tabIndex: number;

  isEdit = false;
  openFromEdit = false;
  currentComponent: string;
  lastComponent: string;
  times = 0;
  selectedSeniority: SeniorityEnum;
  offices: Office[] = [];
  communities: Community[] = [];
  profiles: CandidateProfile[] = [];
  openPositions: OpenPosition[];
  displayOpenPositions: any[];
  stepIndex = 0;
  isDeclineReasonOther = false;
  isOwnedProcesses = false;
  notis: Notification[] = [];
  notisCount: number;
  id: number;
  forms: FormGroup[] = [];
  visible: boolean;
  modalStart: boolean;
  displayNavAndSideMenu: boolean;
  selectedIndex = 0;
  declineReasons: DeclineReason[] = [];
  referralsSubscriptions: Subscription = new Subscription();
  currentPosition: OpenPosition = null;
  jobDescriptionContent : string;
  openDesc : boolean = false;
  referralsListTabTitle: string;

  constructor(private facade: FacadeService, private route: ActivatedRoute, private formBuilder: FormBuilder,
    private userDetailsModal: UserDetailsComponent,
    private globals: Globals, private _referralsService: ReferralsService,
    private router: Router) {
    this.profileList = globals.profileList;
    this.statusList = globals.processStatusList;
    this.currentStageList = globals.processCurrentStageList;
    this.tabIndex = this.route.snapshot.params['openpositions'] ? 1 : 0;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));

    this._referralsService._startReferralsModalSource.subscribe(
      startModalInstruction => this.modalStart = startModalInstruction
    );

    this._referralsService._displayNavAndSideMenuSource.subscribe(
      instruction => this.displayNavAndSideMenu = instruction
    );

    this._referralsService.displayNavAndSideMenu(true);

    if (this.currentUser.role === 'Employee') {
      if (this.modalStart === true) {
        this.selectedIndex = 0;
      } else {
        this.selectedIndex = 1;
      }
    }

    this.facade.appService.removeBgImage();
    this.getProcesses();
    this.getCandidates();
    this.getUsers();
    this.getOffices();
    this.getCommunities();
    this.getProfiles();
    this.getOpenPositions();
    this.displayOpenPositions = this.openPositions;

    this.rejectProcessForm = this.formBuilder.group({
      rejectionReasonDescription: [null]
    });

    this.declineProcessForm = this.formBuilder.group({
      declineReasonDescription: [null, [Validators.required]],
      declineReasonName: [null, [Validators.required]]
    });
    this.getNotifications();

    setTimeout(() => {
      if (this.modalStart === true) {
        this.showContactCandidatesModal(this.newCandidate, null);
      }
    }, 700);

    this.currentUser.role === 'HRUser' || this.currentUser.role === 'HRManagement' || this.currentUser.role === 'Admin' || this.currentUser.role === 'Recruiter' ? this.referralsListTabTitle = 'REFERRALS' : this.referralsListTabTitle = 'MY REFERRALS';

    this.facade.appService.stopLoading();
  }

  ngAfterViewChecked() {
    if (this.slickJobD && this.openDesc){
      setTimeout(() => {
        this.slickJobD.slickGoTo(this.openPositions.indexOf(this.currentPosition));
      }, 50)
      this.openDesc = false;
    }
  }

  isUserRole(roles: string[]): boolean {
    return this.facade.appService.isUserRole(roles);
  }

  getCandidates() {
    const candidateSubscription = this.facade.candidateService.getData().subscribe(res => {
      if (!!res) {
        this.availableCandidates = res.filter(x => x.status === CandidateStatusEnum.New || x.status === CandidateStatusEnum.Recall);
        this.candidatesFullList = res.filter(x => x.isReferred === true);
        this.candidateReferred = res.filter(x => x.referredBy === this.currentUser.lastName + ' ' + this.currentUser.firstName);

        this.facade.referralsService.updateList(this.candidateReferred);
        this.facade.referralsService.referrals.subscribe((referralList) => {
          this.candidateReferred = referralList;
        });
      }
    }, err => {
      console.log(err);
    });
  }

  getNotifications() {
    this.facade.NotificationSevice.getNotifications()
      .subscribe(res => {
        this.notis = res;
        this.notisCount = res.length;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  readNotification(id: number) {
    this.facade.NotificationSevice.readNotifications(id)
      .subscribe(err => {
        console.log(err);
        this.visible = false;
        this.notisCount = this.notisCount - 1;
        this.getNotifications();
      });
  }

  change(value: boolean): void {
    console.log(value);
  }

  clickMe(): void {
    this.visible = false;
  }

  changeNumber(value: boolean): void {
    this.visible = false;
  }

  getUsers() {
    this.facade.userService.get()
      .subscribe(res => {
        this.users = res;
      }, err => {
        console.log(err);
      });
  }

  getOffices() {
    const officesSubscription = this.facade.OfficeService.getData().subscribe(res => {
      this.offices = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.referralsSubscriptions.add(officesSubscription);
  }

  getCommunity(community: number): string {
    return this.communities.find(x => x.id === community).name;
  }

  getCommunities() {
    const communitiesSubscription = this.facade.communityService.getData().subscribe(res => {
      this.communities = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.referralsSubscriptions.add(communitiesSubscription);
  }

  getProfiles() {
    const profileSubscription = this.facade.candidateProfileService.getData().subscribe(res => {
      this.profiles = res;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.referralsSubscriptions.add(profileSubscription);
  }

  getOpenPositions() {
    const openPositionsSubscription = this.facade.openPositionService.getData().subscribe(res => {
      this.openPositions = res;
      this.displayOpenPositions = this.openPositions;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.referralsSubscriptions.add(openPositionsSubscription);
  }

  getProfile(profile: number): string {
    return this.profiles.filter(x => x.id === profile)[0].name;
  }

  getProcesses() {
    const processesSubscription = this.facade.processService.getData().subscribe(res => {
      this.filteredProcesses = res;
      this.listOfDisplayData = res;
      if (!!res) {
        const newProc: Process = res[res.length - 1];
        if (newProc && newProc.candidate) {
          this.candidatesFullList.push(newProc.candidate);
        }
      }
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.referralsSubscriptions.add(processesSubscription);
  }

  getProcessesByUser() {
    this.facade.processService.getData()
      .subscribe(res => {
        if (!!res) {
          this.filteredProcesses = res;
          this.listOfDisplayData = res;
          const newProc: Process = res[res.length - 1];
          if (newProc && newProc.candidate) {
            this.candidatesFullList.push(newProc.candidate);
          }
        }

      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getStatus(status: number): string {
    return this.statusList.find(st => st.id === status).name;
  }

  getCurrentStage(cr: number): string {
    return this.currentStageList.find(st => st.id === cr).name;
  }

  showApproveProcessConfirm(processID: number): void {
    const procesToApprove: Process = this.filteredProcesses.find(p => p.id === processID);
    const processText = procesToApprove.candidate.name.concat(' ').concat(procesToApprove.candidate.lastName);

    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to approve the process for ' + processText + '? This will approve all stages associated with the process',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.approveProcess(processID)
    });
  }

  approveProcess(processID: number) {
    this.facade.appService.startLoading();
    this.facade.processService.approve(processID)
      .subscribe(res => {
        this.getProcesses();
        this.getCandidates();
        this.facade.appService.stopLoading();
        this.facade.toastrService.success('Process approved!');
      }, err => {
        this.facade.appService.stopLoading();
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  showStepsModal(process: Process): void {
    this.emptyProcess = process;
    this.isDetailsVisible = true;
  }

  reset(): void {
    this.searchValue = '';
    this.search();
  }

  resetRecruiter(): void {
    this.searchRecruiterValue = '';
  }

  search(): void {
    const filterFunc = (item) => {
      return (this.listOfSearchProcesses.length ? this.listOfSearchProcesses.some(p => (item.candidate.name.toString() + ' ' + item.candidate.lastName.toString()).indexOf(p) !== -1) : true) &&
        (replaceAccent(item.candidate.name.toString() + ' ' + item.candidate.lastName.toString()).toUpperCase().indexOf(replaceAccent(this.searchValue).toUpperCase()) !== -1);
    };
    const data = this.filteredProcesses.filter(item => filterFunc(item));
    this.listOfDisplayData = data.sort((a, b) => (this.sortValue === 'ascend') ? (a[this.sortName] > b[this.sortName] ? 1 : -1) : (b[this.sortName] > a[this.sortName] ? 1 : -1));
    this.communitySearchName = 'ALL';
    this.profileSearchName = 'ALL';
    this.nameDropdown.nzVisible = false;
  }

  searchOwnRecruiter(): void {
    this.searchRecruiterValue = this.currentUser.lastName + ' ' + this.currentUser.firstName;
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

  sort(sortName: string, value: boolean): void {
    this.sortName = sortName;
    this.sortValue = value;
    this.search();
  }

  showProcessStart(modalContent: TemplateRef<{}>, footer: TemplateRef<{}>, processId: number): void {
    this.facade.appService.startLoading();
    if (processId > -1) {
      this.emptyProcess = this.filteredProcesses.filter(p => p.id === processId)[0];
      this.isEdit = true;
      this.openFromEdit = true;
      this.emptyProcess.currentStage === ProcessCurrentStageEnum.Finished ? this.stepIndex = ProcessCurrentStageEnum.OfferStage : this.stepIndex = this.emptyProcess.currentStage;
    }
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzWidth: '90%',
      nzFooter: footer,

    });
    this.facade.appService.stopLoading();
  }

  newProcessStart(modalContent: TemplateRef<{}>, footer: TemplateRef<{}>, candidate: Candidate): void {
    this.facade.appService.startLoading();
    this.createEmptyProcess(candidate);

    this.currentCandidate = candidate;

    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom',
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzWidth: '90%',
      nzFooter: footer
    });
    this.facade.appService.stopLoading();
  }

  // NOT BEING USED
  setCandidateNewStatus(processStatusId: string, candidateId: number): Candidate {
    const processStatus: string = this.statusList[processStatusId].name;
    const candidate: Candidate = this.candidatesFullList.filter(x => x.id === candidateId)[0];
    if (processStatus !== 'NotStarted' && processStatus !== 'Wait') {
      switch (processStatus) {
        case 'Rejected':
          candidate.status = CandidateStatusEnum.Rejected;
          break;
        case 'Finish':
          candidate.status = CandidateStatusEnum.Hired;
          break;
        case 'Process':
          candidate.status = CandidateStatusEnum.InProgress;
          break;
      }
    }
    return candidate;
  }

  showUserDetailsModal(userID: number, modalContent: TemplateRef<{}>): void {
    this.emptyUser = this.users.filter(user => user.id === userID)[0];
    this.userDetailsModal.showModal(modalContent, this.emptyUser.firstName + ' ' + this.emptyUser.lastName);
  }

  deletePositionConfirm(positionId: number): void {
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete the position?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.openPositionService.delete(positionId)
        .subscribe(res => {
          this.getOpenPositions();
          this.facade.toastrService.success('Position was deleted !');
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
      if (form.invalid) {
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
    this.getForms();
    const slide: number = this.onCheck();
    if (slide > -1) {
      this.processCarousel.goTo(slide);
      const elementName: string = slide === 0 ? 'candidateButton' : slide === 1 ? 'hrButton' : slide === 2 ? 'technicalButton'
        : slide === 3 ? 'clientButton' : slide === 4 ? 'preOfferButton' : slide === 5 ? 'offerButton' : slide === 6 ? 'hireButton' : 'none';
      this.checkSlideIndex(elementName);
      return false;
    } else {
      return true;
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

  getForms() {
    this.forms = [];
    this.forms.push(this.candidateAdd.candidateForm);
    this.forms.push(this.hrStage.hrForm);
    this.forms.push(this.technicalStage.technicalForm);
    this.forms.push(this.clientStage.clientForm);
    this.forms.push(this.preOfferStage.preOfferForm);
    this.forms.push(this.offerStage.offerForm);
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


  saveProcess() {
    if (this.validateForms()) {
      this.facade.appService.startLoading();
      let newCandidate: Candidate;
      let newProcess: Process;
      newCandidate = this.candidateAdd.getFormData();
      newCandidate.candidateSkills = this.technicalStage.getFormDataSkills();
      newProcess = this.getProcessFormData();
      newProcess.userOwnerId = newCandidate.user.id;
      newProcess.candidate = newCandidate;

      if (!this.isEdit) {
        this.facade.processService.add(newProcess)
          .subscribe(res => {
            this.getProcesses();
            this.facade.appService.stopLoading();
            this.facade.toastrService.success('The process was successfully saved !');
            this.createEmptyProcess(newCandidate);
            this.closeModal();
          }, err => {
            this.facade.appService.stopLoading();
            this.facade.toastrService.error(err.message);
          });
      } else {
        this.facade.processService.getByID(newProcess.id)
          .subscribe(res => {
            this.facade.processService.update(newProcess.id, newProcess)
              .subscribe(() => {
                this.getProcesses();
                this.getCandidates();
                this.facade.appService.stopLoading();
                this.facade.toastrService.success('The process was successfully saved !');
                this.createEmptyProcess(newCandidate);
                this.closeModal();
              }, err => {
                this.facade.appService.stopLoading();
                this.facade.toastrService.error(err.message);
              });
          }, err => {
            this.facade.appService.stopLoading();
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
      createdDate: new Date(),
      endDate: null,
      status: !this.isEdit ? ProcessStatusEnum.InProgress : ProcessStatusEnum[CandidateStatusEnum[this.emptyProcess.candidate.status]],
      currentStage: ProcessCurrentStageEnum.NA,
      candidateId: !this.isEdit ? 0 : this.emptyProcess.candidate.id,
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
      preOfferStage: null,
      offerStage: null
    };

    process.hrStage = this.hrStage.getFormData(process.id);
    process.technicalStage = this.technicalStage.getFormData(process.id);
    process.clientStage = this.clientStage.getFormData(process.id);
    process.preOfferStage = this.preOfferStage.getFormData(process.id);
    process.offerStage = this.offerStage.getFormData(process.id);

    // Seniority is now handled globally between technical stage and offer stage. The process uses the last updated value.
    process.seniority = (this.selectedSeniority ||
      process.technicalStage.seniority ||
      process.technicalStage.alternativeSeniority ||
      process.offerStage.seniority);

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
    if (stages[3].status === StageStatusEnum.Accepted) { processStatus = ProcessStatusEnum.Accepted; }

    if (stages[3].status === StageStatusEnum.Accepted && stages[4].status === StageStatusEnum.InProgress ||
      stages[3].status === StageStatusEnum.Accepted && stages[4].status === StageStatusEnum.Declined) {
      processStatus = ProcessStatusEnum.Declined;
    }

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

  showContactCandidatesModal(modalContent: TemplateRef<{}>, position: OpenPosition) {
    this.currentPosition = position;
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'modal-custom referralForm',
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzWidth: '50%',      
      nzFooter: null,
      nzOnCancel: () => this.currentPosition = null
    });

  }


  showContactReferralModal(referral: Candidate) {
    const modal = this.facade.modalService.create({
      nzTitle: null,
      nzContent: ReferralsContactComponent,
      nzClosable: false,
      nzComponentParams: {
        referralToEdit: referral,
        isEditReferral: true,
        communities: this.communities
      },
      nzWidth: '50%',
      nzFooter: null
    });
  }

  deleteReferral(referral: Candidate ){
    this.facade.modalService.confirm({
      nzTitle: `Are you sure you want to delete ${referral.name} ${referral.lastName} ?`,
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.confirmDelete(referral.id)
    });
  }

  confirmDelete(referralId: number){
    this.facade.appService.startLoading();
    this.facade.referralsService.delete(referralId)
    .subscribe(() => {
      this.facade.appService.stopLoading();
      this.facade.toastrService.success('Referral was successfully deleted');
    }, err => {
      this.facade.appService.stopLoading();
      this.facade.errorHandlerService.showErrorMessage(err, 'Referral cannot be deleted');
    });
  }

  showOpenPositionModal(modalContent: TemplateRef<{}>) {
    const modal = this.facade.modalService.create({
      nzTitle: '<h1 class="title"> <strong> Add open position</strong></h1>',
      nzContent: modalContent,
      nzClosable: false,
      nzWidth: '90%',
      nzFooter: null
    });
  }

  showEditPositionModal(positionToEdit: OpenPosition) {
    const modal = this.facade.modalService.create({
      nzTitle: '<h1 class="title"> <strong> Edit open position</strong></h1>',
      nzContent: PositionAddComponent,
      nzClosable: false,
      nzComponentParams: {
        positionToEdit: positionToEdit,
        isEditPosition: true,
        communities: this.communities
      },
      nzWidth: '90%',
      nzFooter: null
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
      createdDate: new Date(),
      endDate: null,
      status: ProcessStatusEnum.NA,
      currentStage: ProcessCurrentStageEnum.NA,
      candidateId: candidate.id,
      candidate: candidate,
      userOwnerId: candidate.user.id,
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
      hrStage: {
        id: 0,
        date: new Date(),
        status: StageStatusEnum.InProgress,
        feedback: '',
        userOwnerId: candidate.user.id,
        userDelegateId: candidate.user.id,
        processId: 0,
        actualSalary: 0,
        wantedSalary: 0,
        additionalInformation: '',
        englishLevel: EnglishLevelEnum.None,
        rejectionReasonsHr: null,
        sentEmail: false,
        readdressStatus: null
      },
      technicalStage: {
        id: 0,
        date: new Date(),
        status: StageStatusEnum.NA,
        feedback: '',
        englishLevel: EnglishLevelEnum.None,
        userOwnerId: candidate.user.id,
        userDelegateId: candidate.user.id,
        processId: 0,
        seniority: SeniorityEnum.NA,
        alternativeSeniority: SeniorityEnum.NA,
        client: '',
        sentEmail: false,
        readdressStatus: null
      },
      clientStage: {
        id: 0,
        date: new Date(),
        status: StageStatusEnum.NA,
        feedback: '',
        userOwnerId: candidate.user.id,
        userDelegateId: candidate.user.id,
        processId: 0,
        interviewer: '',
        delegateName: '',
        readdressStatus: null
      },
      preOfferStage: {
        id: 0,
        date: new Date(),
        dni: 0,
        status: StageStatusEnum.NA,
        feedback: '',
        userOwnerId: candidate.user.id,
        userDelegateId: null,
        processId: 0,
        seniority: SeniorityEnum.NA,
        remunerationOffer: 0,
        vacationDays: 0,
        healthInsurance: HealthInsuranceEnum.NA,
        notes: '',
        firstday: new Date(),
        bonus: '',
        hireDate: new Date(),
        backgroundCheckDone: false,
        backgroundCheckDoneDate: new Date(),
        preocupationalDone: false,
        preocupationalDoneDate: new Date(),
        readdressStatus: null
      },
      offerStage: {
        id: 0,
        date: new Date(),
        status: StageStatusEnum.NA,
        feedback: '',
        userOwnerId: candidate.user.id,
        userDelegateId: null,
        processId: 0,
        seniority: SeniorityEnum.NA,
        remunerationOffer: 0,
        vacationDays: 0,
        healthInsurance: HealthInsuranceEnum.NA,
        notes: '',
        firstday: new Date(),
        bonus: '',
        hireDate: new Date(),
        backgroundCheckDone: false,
        backgroundCheckDoneDate: new Date(),
        preocupationalDone: false,
        preocupationalDoneDate: new Date()
      },
    };
  }

  isDeclined(process: Process): Boolean {
    if (process.hrStage.status === StageStatusEnum.Declined ||
      process.technicalStage.status === StageStatusEnum.Declined ||
      process.clientStage.status === StageStatusEnum.Declined ||
      process.offerStage.status === StageStatusEnum.Declined) {
      return true;
    }
    return false;
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

  checkHrRole() {
    return this.currentUser.role === 'HRManagement' || this.currentUser.role === 'HRUser' ? true : false;
  }

  resetPosition() {
    this.currentPosition = null;
  }

  extraContent(): any {
    if (this.isUserRole(['Admin', 'HRManagement', 'HRUser', 'Recruiter'])) {
      if (this.tabIndex == 1) {
        return this.newPositionButton;
      } else {
        return null;
      }
    } else {
      if (this.tabIndex == 0) {
        return this.newReferralsButton;
      } else {
        return null;
      }
    }
  }

  priorityChange(positionToEdit: OpenPosition){        
    positionToEdit.priority = !positionToEdit.priority;    
    this.facade.openPositionService.update(positionToEdit.id, positionToEdit)
    .subscribe(res => {
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });    
  }

  applyFromDescription(position: OpenPosition) {
    this.facade.modalService.openModals[0].destroy();    
    this.currentPosition = position;    
    this.showContactCandidatesModal(this.newCandidate);
  }
  
  showJobDescription(position: OpenPosition, modalContent: TemplateRef<{}> ){            
    this.jobDescriptionContent = position.jobDescription;
    this.currentPosition = position;
    this.openDesc = true;
    const modal = this.facade.modalService.create({      
      nzContent: modalContent,
      nzClosable: false,
      nzWidth: '60%',
      nzFooter: null           
    });    
  }
  
  closeJobDModal(){
    this.facade.modalService.openModals[0].destroy();
    this.openDesc = false;
  }
  
  ngOnDestroy() {
    this.referralsSubscriptions.unsubscribe();
  }
}