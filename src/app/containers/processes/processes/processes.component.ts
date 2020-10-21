import { AfterViewChecked, Component, ElementRef, OnDestroy, OnInit, TemplateRef, ViewChild, HostListener } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AppComponent } from '@app/app.component';
import { CandidateAddComponent } from '@old-architecture/candidates/add/candidate-add.component';
import { CandidateDetailsComponent } from '@old-architecture/candidates/details/candidate-details.component';
import { ClientStageComponent } from '@old-architecture/stages/client-stage/client-stage.component';
import { HireStageComponent } from '@old-architecture/stages/hire-stage/hire-stage.component';
import { HrStageComponent } from '@old-architecture/stages/hr-stage/hr-stage.component';
import { OfferStageComponent } from '@old-architecture/stages/offer-stage/offer-stage.component';
import { PreOfferStageComponent } from '@old-architecture/stages/pre-offer-stage/pre-offer-stage.component';
import { TechnicalStageComponent } from '@old-architecture/stages/technical-stage/technical-stage.component';
import { UserDetailsComponent } from '@old-architecture/users/details/user-details.component';
import { CandidateStatusEnum } from '@shared/enums/candidate-status.enum';
import { EnglishLevelEnum } from '@shared/enums/english-level.enum';
import { HealthInsuranceEnum } from '@shared/enums/health-insurance.enum';
import { preOfferStatusEnum } from '@shared/enums/pre-offer-status.enum';
import { ProcessCurrentStageEnum } from '@shared/enums/process-current-stage.enum';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { SeniorityEnum } from '@shared/enums/seniority.enum';
import { StageStatusEnum } from '@shared/enums/stage-status.enum';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { Candidate } from '@shared/models/candidate.model';
import { Community } from '@shared/models/community.model';
import { DeclineReason } from '@shared/models/decline-reason.model';
import { Office } from '@shared/models/office.model';
import { PreOffer } from '@shared/models/pre-offer.model';
import { Process } from '@shared/models/process.model';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
import { ReaddressReason } from '@shared/models/readdress-reason.model';
import { ReaddressStatus } from '@shared/models/readdress-status.model';
import { Stage } from '@shared/models/stage.model';
import { User } from '@shared/models/user.model';
import { CandidateInfoService } from '@shared/services/candidate-info.service';
import { FacadeService } from '@shared/services/facade.service';
import { ReferralsService } from '@shared/services/referrals.service';
import { Globals } from '@shared/utils/globals';
import { replaceAccent } from '@shared/utils/replace-accent.util';
import { SlickCarouselComponent } from 'ngx-slick-carousel';
import { Subject, Subscription } from 'rxjs';
import { ProcessSandbox } from '@app/containers/processes/processes.sandbox';
import { ProcessTableView } from '@shared/models/process-tableView.model';

@Component({
  selector: 'app-processes',
  templateUrl: './processes.component.html',
  styleUrls: ['./processes.component.scss'],
})

export class ProcessesComponent implements OnInit, AfterViewChecked, OnDestroy {
  slideConfig = {
    slidesToShow: 1,
    adaptiveHeight: true,
    arrows: true,
    infinite: false,
    draggable: false,
    accessibility: false
  };

  @ViewChild('slickModal', { static: false }) slickModal: SlickCarouselComponent;
  @ViewChild('dropdown', { static: false }) nameDropdown;
  @ViewChild('dropdownStatus', { static: false }) statusDropdown;
  @ViewChild('dropdownCurrentStage', { static: false }) currentStageDropdown;
  @ViewChild('processCarousel', { static: false }) processCarousel;
  @ViewChild(CandidateAddComponent) candidateAdd: CandidateAddComponent;
  @ViewChild(HrStageComponent) hrStage: HrStageComponent;
  @ViewChild(TechnicalStageComponent) technicalStage: TechnicalStageComponent;
  @ViewChild(ClientStageComponent) clientStage: ClientStageComponent;
  @ViewChild(PreOfferStageComponent) preOfferStage: PreOfferStageComponent;
  @ViewChild(OfferStageComponent) offerStage: OfferStageComponent;
  @ViewChild(HireStageComponent) hireStage: HireStageComponent;
  @ViewChild('processStart') processStart: ElementRef;
  @ViewChild('startModalButtons') startModalButtons: ElementRef;

  filteredProcesses: ProcessTableView[] = [];
  filteredOwnProcesses: ProcessTableView[] = [];
  filteredDeletedProcesses: ProcessTableView[];

  searchValue = '';
  searchRecruiterValue = '';
  searchValueStatus = '';
  searchValueCurrentStage = '';
  listOfSearchProcesses = [];
  listOfDisplayData: ProcessTableView[];
  listOfDisplayOwnData: ProcessTableView[];
  listOfDeletedDisplayData: ProcessTableView[];
  currentUser: User;
  sortName = null;
  sortValue = null;
  rejectProcessForm: FormGroup;
  declineProcessForm: FormGroup;
  isDetailsVisible = false;
  emptyProcess: Process;
  availableCandidates: Candidate[] = [];
  candidatesFullList: Candidate[] = [];
  users: User[] = [];
  profileSearch = 0;
  profileSearchName = 'ALL';
  communitySearchName = 'ALL';
  profileList: any[];
  statusList: any[];
  displayedOwnStatusList: any[];
  displayedStatusList: any[];
  currentStageList: any[];
  displayedOwnCurrentStageList: any[];
  displayedCurrentStageList: any[];
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
  displayedCommunities: Community[] = [];
  displayedOwnCommunities: Community[] = [];
  profiles: CandidateProfile[] = [];
  displayedProfiles: CandidateProfile[] = [];
  displayedOwnProfiles: CandidateProfile[] = [];
  stepIndex = 0;
  declineReasons: DeclineReason[] = [];
  isDeclineReasonOther = false;
  isOwnedProcesses = false;
  forms: FormGroup[] = [];
  processId: number;
  displayNavAndSideMenu: boolean;
  successMessage: string = null;
  searchSub: Subscription = new Subscription();
  processesSubscription: Subscription = new Subscription();
  saveEventSubject: Subject<number> = new Subject<number>();

  readdressReasonList: ReaddressReason[] = [];
  readdressReasonTypeList: ReaddressReasonType[] = [];
  readdressStatus: ReaddressStatus = new ReaddressStatus();

  currentStage: ProcessCurrentStageEnum;
  candidateInfo: Candidate;

  preOfferData: { tentativeStartDate: Date, bonus: number, grossSalary: number, vacationDays: number, healthInsurance: HealthInsuranceEnum, notes: String } = null;

  constructor(
    private facade: FacadeService,
    public processesSandbox: ProcessSandbox,
    private formBuilder: FormBuilder,
    private candidateDetailsModal: CandidateDetailsComponent,
    private app: AppComponent,
    private userDetailsModal: UserDetailsComponent,
    private router: Router,
    private _referralsService: ReferralsService,
    private _candidateInfoService: CandidateInfoService,
    private globals: Globals) {
    this.profileList = globals.profileList;
    this.statusList = globals.processStatusList;
    this.currentStageList = globals.processCurrentStageList;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this._referralsService._displayNavAndSideMenuSource.subscribe(
      instruction => this.displayNavAndSideMenu = instruction
    );

    const CandidateSub = this._candidateInfoService._candidateInfoSource.subscribe(info => {
      this.candidateInfo = info;
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
    });
    this.processesSubscription.add(CandidateSub);

    this._referralsService.displayNavAndSideMenu(true);
    this.facade.appService.removeBgImage();
    this.processesSandbox.loadCommunities();
    this.processesSandbox.loadProcess();
    this.processesSandbox.loadDeletedProcess();
    this.getDeletedProcesses();
    this.getCandidates();
    this.getUsers();
    this.getOffices();
    this.getProfiles();
    this.getDeclineReasons();
    this.getSearchInfo();
    this.getReaddressReasonList();
    this.getReaddressReasonTypeList();
    this.isLoading();
    this.isSuccessful();
    this.getError();
    this.getProcesses();
    this.rejectProcessForm = this.formBuilder.group({
      rejectionReasonDescription: [null]
    });
    this.declineProcessForm = this.formBuilder.group({
      declineReasonDescription: [null, [Validators.required]],
      declineReasonName: [null, [Validators.required]]
    });

    setTimeout(() => {
      if (this.candidateInfo) {
        this.newProcessStart(this.processStart, this.startModalButtons);
      }
    }, 1000);

    this.facade.appService.stopLoading();
  }

  ngAfterViewChecked() {
    if (this.slickModal && this.openFromEdit) {
      setTimeout(() => {
        this.slickModal.slickGoTo(this.stepIndex);
      }, 500);
      this.openFromEdit = false;
    }
  }

  isUserRole(roles: string[]): boolean {
    return this.facade.appService.isUserRole(roles);
  }

  getCandidates() {
    const candidatesSubscription = this.facade.candidateService.getData()
      .subscribe(res => {
        if (!!res) {
          this.availableCandidates = res.filter(x => x.status === CandidateStatusEnum.New || x.status === CandidateStatusEnum.Recall);
          this.candidatesFullList = res;
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
    this.processesSubscription.add(candidatesSubscription);
  }

  isLoading() {
    const isLoadSub = this.processesSandbox.isLoading$.subscribe(isLoading => {
      if (isLoading) {
        this.facade.appService.startLoading();
      }
    });
    this.processesSubscription.add(isLoadSub);
  }

  isSuccessful() {
    const successfulSub = this.processesSandbox.isSuccessful$.subscribe(isSuccessful => {
      if (isSuccessful) {
        if (this.successMessage) {
          this.facade.toastrService.success(this.successMessage);
          this._candidateInfoService.sendCandidateInfo(null);
        }
        this.facade.appService.stopLoading();
        this.successMessage = null;
        this.facade.modalService.closeAll();
      }
    });
    this.processesSubscription.add(successfulSub);
  }

  getError() {
    const errorSub = this.processesSandbox.getError$.subscribe(error => {
      if (error) {
        this.facade.toastrService.error(error);
      }
      this.facade.appService.stopLoading();
    });
    this.processesSubscription.add(errorSub);
  }

  getProcesses() {
    const processesSubscription = this.processesSandbox.processes$.subscribe(processes => {
      if (processes && processes.length > 0) {
        this.listOfDisplayData = this.getAllProcesses(processes);
        this.listOfDisplayOwnData = this.getOwnProcesses(processes);
        this.filteredProcesses = this.getAllProcesses(processes);
        this.filteredOwnProcesses = this.getOwnProcesses(processes);
      }
    });
    this.processesSubscription.add(processesSubscription);
  }

  getDeletedProcesses() {
    const deletedProcessesSubscription = this.processesSandbox.deletedProcesses$.subscribe(processes => {
      if (processes && processes.length > 0) {
        this.listOfDeletedDisplayData = processes;
        this.filteredDeletedProcesses = processes;
      }
    });
    this.processesSubscription.add(deletedProcessesSubscription);
  }

  getAllProcesses(processes: ProcessTableView[]) {
    if (this.currentUser.role === 'CommunityManager') {
      return processes.filter(process => process.candidate.community.id === this.currentUser.community.id);
    } else {
      return processes;
    }
  }

  getOwnProcesses(processes: ProcessTableView[]) {
    return processes.filter(process => {
      if (process.userOwner !== null && typeof process.userOwner !== 'undefined') {
        if (process.userOwner.id === this.currentUser.id) {
          return process;
        }
      }
    });
  }

  getUsers() {
    const userSubscription = this.facade.userService.getData()
      .subscribe(res => {
        this.users = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });

    this.processesSubscription.add(userSubscription);
  }

  getOffices() {
    const officeSubscription = this.facade.OfficeService.getData()
      .subscribe(res => {
        this.offices = res;
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
    this.processesSubscription.add(officeSubscription);
  }

  getCommunity(community: number): string {
    return this.communities.find(x => x.id === community).name;
  }

  getProfiles() {
    this.facade.candidateProfileService.get()
      .subscribe(res => {
        this.profiles = res;
        this.profiles.sort((a, b) => (a.name.localeCompare(b.name)));
        for (let i = 0; i < this.profiles.length; i++) {
          if (this.profiles[i].name === 'N/A') {
            const NA = this.profiles.splice(i, 1);
            this.profiles.unshift(NA[0]);
          }
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getDeclineReasons() {
    this.facade.declineReasonService.get('Named')
      .subscribe(res => {
        this.declineReasons = res.sort((a, b) => (a.name.localeCompare(b.name)));
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  getSearchInfo() {
    this.searchSub = this.facade.searchbarService.searchChanged.subscribe(data => {

      this.listOfDisplayData = this.filteredProcesses;
      this.listOfDisplayOwnData = this.filteredOwnProcesses;

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
      this.listOfDisplayOwnData = ownProcesses;
    });
    this.processesSubscription.add(this.searchSub);
  }

  showApproveProcessConfirm(process: Process): void {

    const processText = process.candidate.name.concat(' ').concat(process.candidate.lastName);
    const title = 'Are you sure you want to approve the process for ' +
      processText + '? This will approve all stages associated with the process';
    this.facade.modalService.confirm({
      nzTitle: title,
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.approveProcess(process.id)
    });
  }

  approveProcess(processId: number) {
    this.successMessage = 'Process approved';
    this.processesSandbox.approveProcess(processId);
  }

  showStepsModal(process: Process): void {
    this.emptyProcess = process;
    this.isDetailsVisible = true;
  }

  rejectProcess(process: Process, modalContent: TemplateRef<{}>) {
    this.rejectProcessForm.reset();
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal',
      nzTitle: 'Are you sure you want to reject the process for ' + process.candidate.name + ' ' + process.candidate.lastName + '?',
      nzContent: modalContent,
      nzFooter: [
        {
          label: 'Cancel',
          onClick: () => modal.destroy()
        },
        {
          label: 'Submit',
          type: 'danger',
          onClick: () => {
            this.facade.appService.startLoading();
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
              const rejectionReason = this.rejectProcessForm.get('rejectionReasonDescription').value;
              this.successMessage = 'Process and associated candidate were rejected';
              this.processesSandbox.rejectProcess(process.id, rejectionReason);
            }
          }
        }
      ]
    });
  }

  reactivateProcess(processID: number) {
    const process: ProcessTableView = this.listOfDeletedDisplayData.find(p => p.id === processID);
    const processText = process.candidate.name.concat(' ').concat(process.candidate.lastName);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to reactivate the process for ' + processText + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.processesSandbox.reactivate(processID);
      }
    });
  }

  reset(): void {
    this.searchValue = '';
    this.search();
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

  getFullProcess(modalContent: TemplateRef<{}>, footer: TemplateRef<{}>, processId: number) {
    this.processesSandbox.getProcessById(processId)
      .subscribe(process => {
        this.showProcessStart(modalContent, footer, process);
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  showProcessStart(modalContent: TemplateRef<{}>, footer: TemplateRef<{}>, process: Process): void {
    this.emptyProcess = process;
    this.facade.appService.startLoading();
    this.preOfferData = null;
    if (this.emptyProcess.id > -1) {
      this.isEdit = true;
      this.openFromEdit = true;
      if (this.currentUser.role === 'Admin' || this.currentUser.role === 'Recruiter') {
        this.emptyProcess.currentStage === ProcessCurrentStageEnum.Finished ? this.stepIndex = ProcessCurrentStageEnum.OfferStage : this.stepIndex = this.emptyProcess.currentStage;
      } else {
        this.stepIndex = 0;
      }
      this.getPreOfferData(this.emptyProcess.id);
    } else {
      this.emptyProcess = undefined;
    }
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--lg',
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzFooter: footer
    });
    this.facade.appService.stopLoading();
  }

  newProcessStart(modalContent: any, footer: any, candidate?: Candidate): void {
    this.facade.appService.startLoading();
    if (!candidate) {
      const newCandidate: Candidate = {
        id: null,
        name: '',
        lastName: '',
        dni: 0,
        emailAddress: '',
        phoneNumber: null,
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
        source: null
      };
      this.currentCandidate = newCandidate;
    } else {
      this.currentCandidate = candidate;
    }
    this.createEmptyProcess(this.currentCandidate);
    const modal = this.facade.modalService.create({
      nzWrapClassName: 'recru-modal recru-modal--lg',
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzFooter: footer,
      nzMaskClosable: false
    });
    this.preOfferData = null;
    this.facade.appService.stopLoading();
  }

  showCandidateDetailsModal(candidateID: number, modalContent: TemplateRef<{}>): void {
    this.emptyCandidate = this.candidatesFullList.filter(candidate => candidate.id === candidateID)[0];
    this.candidateDetailsModal.showModal(modalContent, this.emptyCandidate.name + ' ' + this.emptyCandidate.lastName);
  }

  showUserDetailsModal(userID: number, modalContent: TemplateRef<{}>): void {
    this.emptyUser = this.users.filter(user => user.id === userID)[0];
    this.userDetailsModal.showModal(modalContent, this.emptyUser.firstName);
  }

  showDeleteConfirm(processDelete: Process): void {
    this.successMessage = 'Process was deleted';
    const processText = processDelete.candidate.name.concat(' ').concat(processDelete.candidate.lastName);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure you want to delete the process for ' + processText + ' ?',
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => {
        this.processesSandbox.deleteProcess(processDelete.id);
      }
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
        if (form.controls[i] !== form.controls['email']) {
          form.controls[i].markAsDirty();
          form.controls[i].updateValueAndValidity();
        }
      }
    }
    return form;
  }

  validateForms(): boolean {
    this.getForms();
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
        return 'offerButton';
      default:
        return 'candidateButton';
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
      if (this.candidateInfo) {
        newCandidate.id = this.candidateInfo.id;
        newCandidate.isReferred = this.candidateInfo.isReferred;
        newCandidate.knownFrom = this.candidateInfo.knownFrom;
        newCandidate.referredBy = this.candidateInfo.referredBy;
        newCandidate.cv = this.candidateInfo.cv;
        newCandidate.source = this.candidateInfo.source;
        newCandidate.user = this.candidateInfo.user;
      }
      newCandidate.candidateSkills = [...this.technicalStage.getFormDataSkills()];
      newProcess = this.getProcessFormData();
      // temp fix, we should check if englishlevel should be in Candidate table and HrStage table
      newCandidate.englishLevel = newProcess.hrStage.englishLevel;
      newProcess.userOwnerId = this.currentUser.id;
      newProcess.candidate = newCandidate;
      newProcess.offerStage.userOwnerId = this.currentUser.id;
      if (!this.isEdit) {
        if (!newCandidate.id) {
          this.successMessage = 'The process was successfully saved';
          this.facade.candidateService.add(newCandidate).subscribe(res => {
            newProcess.candidate.id = res.id;
            this.processesSandbox.addProcess(newProcess);
          }, err => {
            this.facade.appService.stopLoading();
            this.facade.errorHandlerService.showErrorMessage(err);
          });
        } else {
          // Save only new process (new referral)
          this.successMessage = 'Process was successfully saved';
          this._candidateInfoService.sendCandidateInfo(null);
          this.processesSandbox.addProcess(newProcess);
        }
      } else {
        // Update process
        this.successMessage = 'Process was successfully updated';
        this.facade.candidateService.update(newCandidate.id, newCandidate)
          .subscribe(() => {
            this.processesSandbox.updateProcess(newProcess);
          }, err => {
            this.facade.appService.stopLoading();
            this.facade.errorHandlerService.showErrorMessage(err);
          });
      }
    }
  }

  getProcessFormData(): Process {
    let process: Process;
    if (this.candidateInfo) {
      process = {
        id: !this.isEdit ? 0 : this.emptyProcess.id,
        startDate: new Date(),
        endDate: null,
        status: !this.isEdit ? ProcessStatusEnum.InProgress : ProcessStatusEnum[CandidateStatusEnum[this.emptyProcess.candidate.status]],
        currentStage: ProcessCurrentStageEnum.NA,
        candidateId: this.candidateInfo.id,
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
        offerStage: null,
        createdDate: new Date()
      };
    } else {
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
        preOfferStage: null,
        offerStage: null,
        createdDate: new Date()
      };
    }

    process.hrStage = this.hrStage.getFormData(process.id);
    process.technicalStage = this.technicalStage.getFormData(process.id);
    process.clientStage = this.clientStage.getFormData(process.id);
    process.preOfferStage = this.preOfferStage.getFormData(process.id);
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
    if (stages[3].status === StageStatusEnum.Accepted) { processStatus = ProcessStatusEnum.Accepted; }

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
      nzWrapClassName: 'recru-modal recru-modal--lg',
      nzTitle: null,
      nzContent: modalContent,
      nzClosable: false,
      nzFooter: [
        {
          label: 'Cancel',
          onClick: () => {
            modal.destroy();
            this.refreshTable();
          }
        }]
    });
  }

  refreshTable() {
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
      candidate,
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
        userOwnerId: null,
        userDelegateId: null,
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
        userOwnerId: null,
        userDelegateId: null,
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
        userOwnerId: null,
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
        backgroundCheckDone: null,
        backgroundCheckDoneDate: null,
        preocupationalDone: null,
        preocupationalDoneDate: null,
        readdressStatus: null
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
        remunerationOffer: 0,
        vacationDays: 0,
        healthInsurance: HealthInsuranceEnum.NA,
        notes: '',
        firstday: new Date(),
        bonus: '',
        backgroundCheckDone: false,
        backgroundCheckDoneDate: new Date(),
        preocupationalDone: false,
        preocupationalDoneDate: new Date()
      },
    };
  }

  isDeclined(process: Process): Boolean {
    return (process.hrStage.status === StageStatusEnum.Declined ||
      process.technicalStage.status === StageStatusEnum.Declined ||
      process.clientStage.status === StageStatusEnum.Declined ||
      process.offerStage.status === StageStatusEnum.Declined) ? true : false;
  }

  declineReasonNameChanged() {
    if (this.declineProcessForm.controls.declineReasonName.value === -1) {
      this.isDeclineReasonOther = true;
      this.declineProcessForm.controls.declineReasonDescription.enable();
    } else {
      this.isDeclineReasonOther = false;
      this.declineProcessForm.controls.declineReasonDescription.disable();
    }
  }

  onHrStageSlideClick(currentTargetId) {
    this.setCurrentStage(ProcessCurrentStageEnum.HrStage);
    this.slickModal.slickGoTo(0);
    this.wishedStage(0, currentTargetId);
  }

  onTechnicalStageSlideClick(currentTargetId) {
    this.setCurrentStage(ProcessCurrentStageEnum.TechnicalStage);
    this.slickModal.slickGoTo(1);
    this.wishedStage(1, currentTargetId);
  }

  onClientStageSlideClick(currentTargetId) {
    this.setCurrentStage(ProcessCurrentStageEnum.ClientStage);
    this.slickModal.slickGoTo(2);
    this.wishedStage(2, currentTargetId);
  }

  onPreOfferStageSlideClick(currentTargetId) {
    this.setCurrentStage(ProcessCurrentStageEnum.PreOfferStage);
    this.slickModal.slickGoTo(3);
    this.wishedStage(3, currentTargetId);
  }

  onOfferStageSlideClick(currentTargetId) {
    this.setCurrentStage(ProcessCurrentStageEnum.OfferStage);
    this.slickModal.slickGoTo(4);
    this.wishedStage(4, currentTargetId);
  }

  setCurrentStage(value) {
    this.currentStage = value;
  }

  getReaddressReasonList() {
    this.facade.readdressReasonService.getData()
      .subscribe(res => {
        this.readdressReasonList = res;
      }, err => {
        console.log(err);
      });
  }

  getReaddressReasonTypeList() {
    this.facade.readdressReasonTypeService.getData()
      .subscribe(res => {
        this.readdressReasonTypeList = res;
      }, err => {
        console.log(err);
      });
  }

  clearDataAndCloseModal() {
    let emptyCandidate: Candidate;
    this._candidateInfoService.sendCandidateInfo(emptyCandidate);
    this.closeModal();
  }

  getPreOfferData(processId: number) {
    let preOffers: PreOffer[] = null;
    this.facade.preOfferService.getByProcessId(processId)
      .subscribe(res => {
        preOffers = res;

        if (preOffers && (preOffers.length > 0) && (this.emptyProcess.preOfferStage.status == StageStatusEnum.Accepted)) {
          const mainAcceptedPreOffer = preOffers.find(_ => _.status == preOfferStatusEnum.Accepted);
          if (mainAcceptedPreOffer) {
            this.preOfferData = {
              tentativeStartDate: mainAcceptedPreOffer.tentativeStartDate,
              bonus: mainAcceptedPreOffer.bonus,
              grossSalary: mainAcceptedPreOffer.salary,
              vacationDays: mainAcceptedPreOffer.vacationDays,
              healthInsurance: mainAcceptedPreOffer.healthInsurance,
              notes: mainAcceptedPreOffer.notes
            };
          }
        }
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  @HostListener('document:keydown.escape', ['$event']) onKeydownHandler(event: KeyboardEvent) {
    if (event.keyCode === 27) {
      let emptyCandidate: Candidate;
      this._candidateInfoService.sendCandidateInfo(emptyCandidate);
    }
  }

  ngOnDestroy() {
    this.processesSubscription.unsubscribe();
  }
}
