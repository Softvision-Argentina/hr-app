import { AfterViewChecked, Component, OnDestroy, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SeniorityEnum } from '@shared/enums/seniority.enum';
import { StageStatusEnum } from '@shared/enums/stage-status.enum';
import { Candidate } from '@shared/models/candidate.model';
import { Community } from '@shared/models/community.model';
import { DeclineReason } from '@shared/models/decline-reason.model';
import { OpenPosition } from '@shared/models/open-position.model';
import { Process } from '@shared/models/process.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { ReferralsService } from '@shared/services/referrals.service';
import { Globals } from '@shared/utils/globals';
import { SlickCarouselComponent } from 'ngx-slick-carousel';
import { Subscription } from 'rxjs';
import { AppComponent } from '@app/app.component';
import { UserDetailsComponent } from '@old-architecture/users/details/user-details.component';
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
    prevArrow: '<div class="slick-prev"><img style="transform: scaleX(-1);" src="assets/images/arrow_medium.svg"></div>',
    nextArrow: '<div class="slick-next"><img src="assets/images/arrow_medium.svg"></div>',
    accessibility: false
  };

  @ViewChild('slickJobDescription') slickJobD: SlickCarouselComponent;
  @ViewChild('dropdown') nameDropdown;
  @ViewChild('dropdownStatus') statusDropdown;
  @ViewChild('dropdownCurrentStage') currentStageDropdown;
  @ViewChild('newCandidate') newCandidate;
  @ViewChild('newReferralsButton') newReferralsButton: any;
  @ViewChild('newPositionButton') newPositionButton: any;

  searchValue = '';
  searchRecruiterValue = '';
  searchValueStatus = '';
  searchValueCurrentStage = '';
  currentUser: User;
  sortName = null;
  sortValue = null;
  isDetailsVisible = false;
  isEditable: boolean = false;
  users: User[] = [];

  profileSearch = 0;
  profileSearchName = 'ALL';
  communitySearch = 0;
  communitySearchName = 'ALL';
  statusList: any[];
  currentStageList: any[];
  tabIndex: number;

  isEdit = false;
  openFromEdit = false;
  currentComponent: string;
  lastComponent: string;
  times = 0;
  selectedSeniority: SeniorityEnum;
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
  jobDescriptionContent: string;
  openDesc: boolean = false;
  communities: Community[] = [];
  referralsListTabTitle: string;

  constructor(private facade: FacadeService, private route: ActivatedRoute, private formBuilder: FormBuilder,
    private userDetailsModal: UserDetailsComponent,
    private globals: Globals, private _referralsService: ReferralsService,
    private router: Router) {
    this.statusList = globals.processStatusList;
    this.currentStageList = globals.processCurrentStageList;
    this.tabIndex = this.route.snapshot.params['openpositions'] ? 1 : 0;
  }

  ngOnInit() {
    this.getOpenPositions();    
    this.getCommunities();
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
    this.displayOpenPositions = this.openPositions;


    setTimeout(() => {
      if (this.modalStart === true) {
        this.showContactCandidatesModal(this.newCandidate, null);
      }
    }, 700);

    this.currentUser.role === 'HRUser' || this.currentUser.role === 'HRManagement' || this.currentUser.role === 'Admin' || this.currentUser.role === 'Recruiter' ? this.referralsListTabTitle = 'REFERRALS' : this.referralsListTabTitle = 'MY REFERRALS';

    this.facade.appService.stopLoading();
  }

  ngAfterViewChecked() {
    if (this.slickJobD && this.openDesc) {
      setTimeout(() => {
        this.slickJobD.slickGoTo(this.openPositions.indexOf(this.currentPosition));
      }, 50)
      this.openDesc = false;
    }
  }

  isUserRole(roles: string[]): boolean {
    return this.facade.appService.isUserRole(roles);
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

  getCommunities() {
    const communitiesSubscription = this.facade.communityService.getData().subscribe(res => {
      this.communities = res;
      if(res)
        this.facade.appService.stopLoading();
    }, err => {
      this.facade.errorHandlerService.showErrorMessage(err);
      this.facade.appService.stopLoading();
    });
    this.referralsSubscriptions.add(communitiesSubscription);
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

  getStatus(status: number): string {
    return this.statusList.find(st => st.id === status).name;
  }

  getCurrentStage(cr: number): string {
    return this.currentStageList.find(st => st.id === cr).name;
  }

  resetRecruiter(): void {
    this.searchRecruiterValue = '';
  }

  searchOwnRecruiter(): void {
    this.searchRecruiterValue = this.currentUser.lastName + ' ' + this.currentUser.firstName;
    this.isOwnedProcesses = true;
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

  showContactCandidatesModal(modalContent: TemplateRef<{}>, position?: OpenPosition) {
    this.currentPosition = position;
    const modal = this.facade.modalService.create({
      nzTitle: 'Submit your referral',
      nzContent: modalContent,
      nzWrapClassName: 'recru-modal recru-modal--md recru-modal--title-lg',
      nzClosable: false,
      nzFooter: null,
      nzOnCancel: () => this.currentPosition = null
    });

  }

  showContactReferralModal(referral: Candidate) {
    this.currentPosition = {
      id: null,
      title: referral.openPositionTitle,
      seniority: null,
      studio: null,
      community: null,
      jobDescription: null,
      priority: null,
    };

    const modal = this.facade.modalService.create({
      nzTitle: 'Submit your referral',
      nzContent: ReferralsContactComponent,
      nzWrapClassName: 'recru-modal recru-modal--md recru-modal--title-lg',
      nzClosable: false,
      nzComponentParams: {
        referralToEdit: referral,
        isEditReferral: true,
        communities: this.communities,
        position: this.currentPosition
      },
      nzFooter: null,
      nzOnCancel: () => this.currentPosition = null
    });
  }

  deleteReferral(referral: Candidate) {
    this.facade.modalService.confirm({
      nzTitle: `Are you sure you want to delete ${referral.name} ${referral.lastName} ?`,
      nzContent: '',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.confirmDelete(referral.id)
    });
  }

  confirmDelete(referralId: number) {
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
      nzTitle: 'Add open position',
      nzContent: modalContent,
      nzWrapClassName: 'recru-modal recru-modal--md recru-modal--title-lg',
      nzClosable: false,
      nzFooter: null
    });
  }

  showEditPositionModal(positionToEdit: OpenPosition) {
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit open position',
      nzContent: PositionAddComponent,
      nzWrapClassName: 'recru-modal recru-modal--md recru-modal--title-lg',
      nzClosable: false,
      nzComponentParams: {
        positionToEdit: positionToEdit,
        isEditPosition: true,
        communities: this.communities
      },
      nzFooter: null
    });
  }

  updateSeniority($event) {
    this.selectedSeniority = $event;
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

  priorityChange(positionToEdit: OpenPosition) {
    positionToEdit.priority = !positionToEdit.priority;
    this.facade.openPositionService.update(positionToEdit.id, positionToEdit)
      .subscribe(res => {
      }, err => {
        this.facade.errorHandlerService.showErrorMessage(err);
      });
  }

  applyFromDescription(position: OpenPosition) {
    this.facade.modalService.openModals[0].destroy();
    this.showContactCandidatesModal(this.newCandidate, position);
  }

  showJobDescription(position: OpenPosition, modalContent: TemplateRef<{}>) {
    this.jobDescriptionContent = position.jobDescription;
    this.currentPosition = position;
    this.openDesc = true;
    const modal = this.facade.modalService.create({
      nzContent: modalContent,
      nzTitle: 'Open position',
      nzWrapClassName: 'recru-modal recru-modal--md job-description-modal',
      nzClosable: true,
      nzFooter: null           
    });    
  }

  closeJobDModal() {
    this.facade.modalService.openModals[0].destroy();
    this.openDesc = false;
  }

  ngOnDestroy() {
    this.referralsSubscriptions.unsubscribe();
  }

  refreshReferralsTable(candidate: Candidate) {
    this.facade.referralsService.addNew(candidate);
  }
}