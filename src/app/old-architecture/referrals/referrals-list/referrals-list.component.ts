import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { ProcessCurrentStageEnum } from '@shared/enums/process-current-stage.enum';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { referralCurrentStage } from '@shared/enums/referral-currentStage.enum';
import { Candidate } from '@shared/models/candidate.model';
import { ColumnItem } from '@shared/models/column-item.model';
import { Process } from '@shared/models/process.model';
import { ReferralListItem } from '@shared/models/referral-list-item.model';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';
import { ReferralsService } from '@shared/services/referrals.service';
import { Globals } from '@shared/utils/globals';
import { forkJoin, Subscription } from 'rxjs';
import {CandidateInfoService} from '@shared/services/candidate-info.service';
@Component({
  selector: 'app-referrals-list',
  templateUrl: './referrals-list.component.html',
  styleUrls: ['./referrals-list.component.scss']
})
export class ReferralsListComponent implements OnInit, OnChanges, OnDestroy {
  referralsList: ReferralListItem[] = [];
  completeReferralsList: ReferralListItem[] = [];
  processes: Process[] = [];
  candidates: Candidate[] = [];
  currentUser: User;
  referralListStatus: any[];
  currentProcessStatus: any[];
  processStatusList: { id: number, name: string, value: string }[] = [];
  processesSubscription: Subscription;
  candidateSubscription: Subscription;
  searchSub: Subscription;
  referralsSubscriptions: Subscription[] = [];

  @Input() communities;
  @Output() editEvent = new EventEmitter();
  @Output() deleteEvent = new EventEmitter();
  listOfColumns: ColumnItem[];
  candidateInfo: Candidate;

  constructor(
    private facade: FacadeService,
    private globals: Globals,
    private _candidateInfoService : CandidateInfoService,
    private router: Router) {
    this.referralListStatus = globals.referralCurrentStage;
    this.processStatusList = globals.stageStatusList;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.referralsSubscriptions.push(this._candidateInfoService._candidateInfoSource
      .subscribe(info => this.candidateInfo = info)
    );
    this.getSearchInfo();
    this.getReferrals();
    this.onReferralsListChange();
  }

  ngOnChanges() {
    if (this.communities) {
      this.communities = this.communities.filter(community => community.name != 'N/A');

      this.listOfColumns = [
        {
          name: 'Candidate'
        },
        {
          name: 'Source'
        },
        {
          name: 'Referral Date',
          sortOrder: null,
          sortFn: (a: any, b: any) => a.candidate.id - b.candidate.id
        },
        {
          name: 'Community',
          listOfFilter: this.communities.map((value, index) => {
            return { text: value.name, value: value.name };
          }),
          sortOrder: null,
          sortFn: (a: any, b: any) => a.candidate.community.name.localeCompare(b.candidate.community.name),
          filterFn: (communityNameList: string[], item: any) => communityNameList.some(name => item.candidate.community.name.indexOf(name) !== -1)
        },
        {
          name: 'Current Stage',
          sortOrder: null,
          sortFn: (a: any, b: any) => a.currentStatus.message.localeCompare(b.currentStatus.message)
        },
        {
          name: 'Position Title',
          sortOrder: null,
          sortFn: (a: any, b: any) => b.candidate.openPosition.priority.localeCompare(a.currentStatus.message)
        },
        {
          name: 'Actions'
        }
      ]

    }
  }

  resetFilters(): void {
    this.listOfColumns.forEach(item => {
      switch (item.name) {
        case 'Community':
          item.listOfFilter = this.communities.map((value, index) => { return { text: value.name, value: value.name } })
          break;
      }
    });
  }

  trackByName(_: number, item: ColumnItem): string {
    return item.name;
  }

  getReferrals() {
    const referralSubs = this.facade.referralsService.get()
    .subscribe(res => {
      res.forEach(referralItem => {
        const newReferralItem = new ReferralListItem(referralItem.candidate, referralItem.processId, referralItem.processCurrentStage, referralItem.processStatus);
        this.referralsList = [
          ...this.referralsList,
          newReferralItem
        ];
        this.completeReferralsList = [
          ...this.completeReferralsList,
          newReferralItem
        ]
      });
    });
    this.referralsSubscriptions.push(referralSubs);
  }

  onReferralsListChange(){
    const newCandidateSubs = this.facade.referralsService.candidateAdded
    .subscribe(candidate =>{
      const candidateId = this.referralsList.findIndex(referralItem => referralItem.candidate.id === candidate.id);
      if (candidateId === -1){
        const newReferralItem = new ReferralListItem(candidate, 0, 0, 0);
        this.referralsList = [
          ...this.referralsList,
          newReferralItem
        ];
        this.completeReferralsList = [
          ...this.completeReferralsList,
          newReferralItem
        ]
      } else{
        this.referralsList[candidateId].candidate = candidate;
      }
    });
    this.referralsSubscriptions.push(newCandidateSubs);

    const deleteCandidateSubs = this.facade.referralsService.candidateDelete
    .subscribe(candidateId =>{
      this.referralsList = this.referralsList.filter(referralItem => referralItem.candidate.id !== candidateId);
    });
    this.referralsSubscriptions.push(deleteCandidateSubs);
  }




  getTextColor(referralItem: ReferralListItem) {
    // should refactor this.
    const color = [
      { 'color': 'black' },
      { 'color': '#C800A1' }, // Magenta
      { 'color': '#32a852' }, // green
      { 'color': '#348ceb' }, // blue
    ];
    switch (referralItem.status) {
      case referralCurrentStage.new:
        return color[0];
      case referralCurrentStage.onProcess:
        return color[1];
      case referralCurrentStage.contracted:
        return color[2];
      case referralCurrentStage.pipeline:
        return color[3];
      default:
        break;
    }
  }

  fontWeightForNewReferral(status: number) {
    if (status === 0) {
      return { 'font-weight': 'bold' };
    }
  }



  showContactReferralModal(referral: Candidate) {
    this.editEvent.emit(referral);
  }

  openCV(cv) {
    window.open(cv);
  }

  checkHrRole() {
    return this.currentUser.role === 'HRManagement' || this.currentUser.role === 'HRUser' || this.currentUser.role === 'Admin' || this.currentUser.role === 'Recruiter' ? true : false;
  }

  checkCV(cv) {
    return cv === null ? false : true;
  }

  playButtonTitle(status) {
    return status === 0 ? 'Start Process' : 'Process has been created for this referral';
  }

  goToProcesses(candidate) {
      this._candidateInfoService.sendCandidateInfo(candidate);
      this.router.navigateByUrl('/processes');
  }

  ngOnDestroy() {
    this.referralsSubscriptions.forEach(sub => {
      sub.unsubscribe();
    });
  }

  getSearchInfo() {
    this.searchSub = this.facade.searchbarService.searchChanged.subscribe(data => {

      this.referralsList = this.completeReferralsList.filter(referral => {
        const referralFullName = referral.candidate.name + referral.candidate.lastName;
        const value = data.toString().toUpperCase();
        return referralFullName.toString().toUpperCase().indexOf(value) !== -1;
      });
    });
    this.referralsSubscriptions.push(this.searchSub);
  }
}
