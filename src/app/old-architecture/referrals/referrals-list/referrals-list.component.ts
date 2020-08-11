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
  referralsSubscription: Subscription = new Subscription();

  @Input() communities;
  @Output() editEvent = new EventEmitter();
  @Output() deleteEvent = new EventEmitter();
  listOfColumns: ColumnItem[];
  candidateInfo: Candidate;

  constructor(private facade: FacadeService, private globals: Globals, private _referralsService: ReferralsService, private router: Router) {
    this.referralListStatus = globals.referralCurrentStage;
    this.processStatusList = globals.stageStatusList;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this._referralsService._candidateInfoSource.subscribe(info => this.candidateInfo = info);
    this.getSearchInfo();
    this.init();
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

  init() {
    forkJoin(
      this.facade.candidateService.get(),
      this.facade.processService.get()
    )
      .subscribe(([candidates, processes]: [Candidate[], Process[]]) => {
        this.candidates = candidates;
        this.processes = processes;
        this.candidateSubscription = this.facade.referralsService.candidateAdded
          .subscribe(() => {
            this.facade.candidateService.get()
              .subscribe(cand => {
                this.candidates = cand;
                this.getReferralsList();
              });
          });
        this.processesSubscription = this.facade.processService.getData()
          .subscribe(proc => {
            this.processes = proc;
            this.getReferralsList();
          });
        this.getReferralsList();
      });
  }

  getReferralsList() {
    this.referralsList = [];
    if (!!this.candidates && !!this.processes) {
      const referredCandidates: Candidate[] = this.candidates.filter(candidate => {
        if (candidate.referredBy && candidate.referredBy.length > 0) {
          const fullName = this.currentUser.username;
          if (this.currentUser.role === 'HRUser' || this.currentUser.role === 'HRManagement' || this.currentUser.role === 'Admin' || this.currentUser.role === 'Recruiter') {
            return candidate;
          } else {
            if (candidate.referredBy === fullName) {
              return candidate;
            }
          }

        }
      });
      referredCandidates.map(candidate => {
        const relatedProcess = this.processes.find(process => process.candidateId === candidate.id);
        if (relatedProcess) {
          this.referralsList = [
            ...this.referralsList,
            { candidate: { ...candidate }, currentStatus: this.getCurrentStatus(relatedProcess, this.getCurrentStage(relatedProcess)) }
          ];
        } else {
          this.referralsList = [
            ...this.referralsList,
            { candidate: { ...candidate }, currentStatus: this.getCurrentStatus(null, this.getCurrentStage(null)) }
          ];
        }
      });
    }
    this.completeReferralsList = this.referralsList;
  }

  getCurrentStage(currentProcess: Process) {
    if (currentProcess) {
      switch (currentProcess.currentStage) {
        case ProcessCurrentStageEnum.HrStage:
          return {
            stage: ProcessCurrentStageEnum.HrStage,
            message: 'Hr Stage: '
          };
        case ProcessCurrentStageEnum.TechnicalStage:
          return {
            stage: ProcessCurrentStageEnum.TechnicalStage,
            message: 'Technical Stage: '
          }
        case ProcessCurrentStageEnum.ClientStage:
          return {
            stage: ProcessCurrentStageEnum.ClientStage,
            message: 'Client Stage: '
          };
        case ProcessCurrentStageEnum.PreOfferStage:
          return {
            stage: ProcessCurrentStageEnum.PreOfferStage,
            message: 'PreOffer Stage: '
          };
        case ProcessCurrentStageEnum.OfferStage:
          return {
            stage: ProcessCurrentStageEnum.OfferStage,
            message: 'Offer Stage: '
          };
        case ProcessCurrentStageEnum.Finished:
          return {
            stage: ProcessCurrentStageEnum.Finished,
            status: referralCurrentStage.contracted,
            //???? should say this message?
            message: 'Finished: Congratulations! Your candidate will be part of our team. HR will let you know next steps if a bonus is applied to this referral.'
          };
        case ProcessCurrentStageEnum.Pipeline:
          return {
            stage: ProcessCurrentStageEnum.Pipeline,
            status: referralCurrentStage.pipeline,
            //????? should say this message?
            message: 'Pipeline: Will be belonging to our database for current or future searchings.'
          }
        default:
          return {
            stage: "brokn",
            message: "ritoto"
          };
          break;
      }
    } else {
      return {
        stage: null,
        status: referralCurrentStage.new,
        message: 'New: There\'s not an ongoing process for this referral yet'
      }
    }
  }

  getCurrentStatus(currentProcess: Process, currentStatus: any) {
    if (currentProcess) {
      const processStatus = this.processStatusList.find(st => st.id === currentProcess.status);
      if (currentProcess.currentStage === ProcessCurrentStageEnum.OfferStage) {
        if (currentProcess.status === ProcessStatusEnum.Hired) {
          return {
            ...currentStatus,
            status: referralCurrentStage.contracted,
            message: 'Finished: Congratulations! Your candidate will be part of our team. HR will let you know next steps if a bonus is applied to this referral.'
          };
        } else if (currentProcess.status === ProcessStatusEnum.Declined || currentProcess.status === ProcessStatusEnum.Rejected) {
          return {
            ...currentStatus,
            status: referralCurrentStage.pipeline,
            message: 'Pipeline: Will be belonging to our database for current or future searchings'
          };
        }
      } else if (currentProcess.currentStage !== ProcessCurrentStageEnum.Finished && currentProcess.currentStage !== ProcessCurrentStageEnum.Pipeline) {
        return {
          ...currentStatus,
          status: referralCurrentStage.onProcess,
          message: currentStatus.message + processStatus.name,
        };
      } else {
        return currentStatus;
      }
    } else {
      return currentStatus;
    }
  }


  getTextColor(referralItem: ReferralListItem) {
    // should refactor this.
    const color = [
      { 'color': 'black' },
      { 'color': '#C800A1' }, // Magenta
      { 'color': '#32a852' }, // green
      { 'color': '#348ceb' }, // blue
    ];
    switch (referralItem.currentStatus.status) {
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
      this._referralsService.sendCandidateInfo(candidate);
      this.router.navigateByUrl('/processes');
  }

  ngOnDestroy() {
    this.processesSubscription.unsubscribe();
    this.candidateSubscription.unsubscribe();
    this.referralsSubscription.unsubscribe();
  }

  getSearchInfo() {
    this.searchSub = this.facade.searchbarService.searchChanged.subscribe(data => {

      this.referralsList = this.completeReferralsList.filter(referral => {
        const referralFullName = referral.candidate.name + referral.candidate.lastName;
        const value = data.toString().toUpperCase();
        return referralFullName.toString().toUpperCase().indexOf(value) !== -1;
      });
    });
    this.referralsSubscription.add(this.searchSub);
  }
}
