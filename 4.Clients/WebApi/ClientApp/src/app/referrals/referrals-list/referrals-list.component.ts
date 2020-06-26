import { Component, OnInit, OnDestroy } from '@angular/core';
import { FacadeService } from 'src/app/services/facade.service';
import { Process } from 'src/entities/process';
import { Candidate } from 'src/entities/candidate';
import { forkJoin, Subscription } from 'rxjs';
import { User } from 'src/entities/user';
import { Globals } from '../../app-globals/globals';
import { referralCurrentStage } from 'src/entities/enums/referral-currentStage.enum';
import { ReferraListItem } from 'src/entities/referral-list-item';
import { ProcessCurrentStageEnum } from 'src/entities/enums/process-current-stage';
import { ProcessStatusEnum } from 'src/entities/enums/process-status.enum';
@Component({
  selector: 'app-referrals-list',
  templateUrl: './referrals-list.component.html',
  styleUrls: ['./referrals-list.component.scss']
})
export class ReferralsListComponent implements OnInit, OnDestroy {
  referralsList: ReferraListItem[] = [];
  processes: Process[] = [];
  candidates: Candidate[] = [];
  currentUser: User;
  referralListStatus: any[];
  currentProcessStatus: any[];
  processStatusList: {id: number, name: string, value: string}[] = [];
  processesSubscription: Subscription;
  candidateSubscription: Subscription;
  constructor(private facade: FacadeService, private globals: Globals) {
    this.referralListStatus = globals.referralCurrentStage;
    this.processStatusList = globals.stageStatusList;
  }

  ngOnInit() {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    this.init();
  }

  init() {
    forkJoin(
      this.facade.candidateService.get(),
      this.facade.processService.get()
    )
    .subscribe(([candidates, processes ]: [Candidate[], Process[]]) => {
      this.candidates = candidates;
      this.processes = processes;
      this.candidateSubscription = this.facade.referralsService.candidateAdded
      .subscribe( () => {
        this.facade.candidateService.get()
        .subscribe(cand =>{
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
    if (!!this.candidates && !!this.processes) {
      const referredCandidates: Candidate[] = this.candidates.filter(candidate => {
        if (candidate.referredBy && candidate.referredBy.length > 0) {
          const fullName = this.currentUser.firstName + ' ' + this.currentUser.lastName;
          if (candidate.referredBy === fullName) {
            return candidate;
          }
        }
      });
      referredCandidates.map(candidate => {
        const relatedProcess = this.processes.find(process => process.candidateId === candidate.id);
        const exists = this.referralsList.find(item => item.candidate.id === candidate.id);
        if (!exists) {
          // Should check if conditional is necessary
          if (relatedProcess) {
            this.referralsList = [
              ...this.referralsList,
              {candidate: {...candidate}, currentStatus: this.getCurrentStatus(relatedProcess, this.getCurrentStage(relatedProcess))}
            ];
          } else {
              this.referralsList = [
                ...this.referralsList,
                {candidate: {...candidate}, currentStatus: this.getCurrentStatus(null, this.getCurrentStage(null))}
              ];
          }
        }
      });
    }
  }

  getCurrentStage(currentProcess: Process) {
    if (currentProcess) {
      switch (currentProcess.currentStage) {
        case ProcessCurrentStageEnum.HrStage:
          return{
            stage: ProcessCurrentStageEnum.HrStage,
            message: 'Hr Stage: '
          };
        case ProcessCurrentStageEnum.TechnicalStage:
          return{
            stage: ProcessCurrentStageEnum.TechnicalStage,
            message: 'Technical Stage: '
          }
        case ProcessCurrentStageEnum.ClientStage:
          return{
            stage: ProcessCurrentStageEnum.ClientStage,
            message: 'Client Stage: '
          };
        case ProcessCurrentStageEnum.PreOfferStage:
          return{
            stage: ProcessCurrentStageEnum.PreOfferStage,
            message: 'PreOffer Stage: '
          };
        case ProcessCurrentStageEnum.OfferStage:
          return{
            stage: ProcessCurrentStageEnum.OfferStage,
            message: 'Offer Stage: '
          };
        case ProcessCurrentStageEnum.Finished:
          return{
            stage: ProcessCurrentStageEnum.Finished,
            status: referralCurrentStage.contracted,
            //???? should say this message?
            message: 'Finished: Congratulations! Your candidate will be part of our team. HR will let you know next steps if a bonus is applied to this referral.'
          };
        case ProcessCurrentStageEnum.Pipeline:
          return{
            stage: ProcessCurrentStageEnum.Pipeline,
            status: referralCurrentStage.pipeline,
            //????? should say this message?
            message: 'Pipeline: Will be belonging to our database for current or future searchings.'
          }
        default:
          return{
            stage:"brokn",
            message:"ritoto"
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
    if(currentProcess) {
      const processStatus = this.processStatusList.find(st => st.id === currentProcess.status);
      if (currentProcess.currentStage === ProcessCurrentStageEnum.OfferStage) {
        if (currentProcess.status === ProcessStatusEnum.Hired) {
          return{
            ...currentStatus,
            status: referralCurrentStage.contracted,
            message: 'Finished: Congratulations! Your candidate will be part of our team. HR will let you know next steps if a bonus is applied to this referral.'
          };
        } else if (currentProcess.status === ProcessStatusEnum.Declined || currentProcess.status === ProcessStatusEnum.Rejected) {
          return{
            ...currentStatus,
            status: referralCurrentStage.pipeline,
            message: 'Pipeline: Will be belonging to our database for current or future searchings'
          };
        }
      } else if(currentProcess.currentStage !== ProcessCurrentStageEnum.Finished && currentProcess.currentStage !== ProcessCurrentStageEnum.Pipeline ) {
        return{
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


  getTextColor(referralItem: ReferraListItem) {
    // should refactor this.
    const color =[
      {'color': 'black'},
      {'color': '#C800A1'}, // Magenta
      {'color': '#32a852'}, // green
      {'color': '#348ceb'}, // blue


    ];
    switch (referralItem.currentStatus.status) {
      case referralCurrentStage.new:
        return color[0];
        break;
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


  ngOnDestroy() {
    this.processesSubscription.unsubscribe();
    this.candidateSubscription.unsubscribe();
  }
}
