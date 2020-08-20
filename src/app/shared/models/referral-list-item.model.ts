import { Candidate } from './candidate.model';
import { referralCurrentStage } from '../enums/referral-currentStage.enum';
import { ProcessCurrentStageEnum } from '../enums/process-current-stage.enum';
import { Globals } from '@shared/utils/globals';
import { ProcessStatusEnum } from '../enums/process-status.enum';
export class ReferralListItem {
    candidate: Candidate;
    private processId: number;
    stage: number;
    message: string;
    status: number;
    private processStatus;
    private statusMessage = {
      [ProcessStatusEnum.NA] : 'N/A',
      [ProcessStatusEnum.InProgress]: 'In Progress',
      [ProcessStatusEnum.Accepted]: 'Accepted',
      [ProcessStatusEnum.Declined]: 'Declined',
      [ProcessStatusEnum.Hired]: 'Hired',
      [ProcessStatusEnum.Recall]: 'Recall',
      [ProcessStatusEnum.Pipeline]: 'Pipeline',
      [ProcessStatusEnum.PendingReply]: 'Pending Reply'
    };
    private stageMessage = {
      [ProcessCurrentStageEnum.HrStage]: 'Hr Stage: ',
      [ProcessCurrentStageEnum.TechnicalStage]: 'Technical Stage: ',
      [ProcessCurrentStageEnum.ClientStage]: 'Client Stage: ',
      [ProcessCurrentStageEnum.PreOfferStage]: 'Preoffer Stage: ',
      [ProcessCurrentStageEnum.OfferStage]: 'Offer Stage: ',
      [ProcessCurrentStageEnum.Finished]: 'Finished: Congratulations! Your candidate will be part of our team. HR will let you know next steps if a bonus is applied to this referral.',
      [ProcessCurrentStageEnum.Pipeline]: 'Pipeline: Will be belonging to our database for current or future searchings.'
    };
    constructor(
      candidate: Candidate,
      processId: number,
      processStage: number,
      processStatus: number,
    ){
        this.candidate = candidate;
        this.processId = processId;
        this.stage = processStage;
        this.processStatus = processStatus;
        this.getCurrentStatus();
    }

    private getCurrentStage() {
        if (this.stage) {
            if (!this.stageMessage[this.stage]){
              this.message = 'Something went wrong';
            } else {
              this.message = this.stageMessage[this.stage];
            }
        } else {
          this.status = referralCurrentStage.new;
          this.message = 'New: There\'s not an ongoing process for this referral yet';
        }
    }

    private getCurrentStatus() {
      this.getCurrentStage();
      if (this.status !== referralCurrentStage.new){
        if (this.stage === ProcessCurrentStageEnum.OfferStage){
          if (this.processStatus === ProcessStatusEnum.Hired) {
            this.status = referralCurrentStage.contracted,
            this.message = 'Finished: Congratulations! Your candidate will be part of our team. HR will let you know next steps if a bonus is applied to this referral.';
          } else if (this.processStatus === ProcessStatusEnum.Declined || this.processStatus === ProcessStatusEnum.Rejected) {
            this.status = referralCurrentStage.pipeline;
            this.message = 'Pipeline: Will be belonging to our database for current or future searchings';
          }
        }
        else if ( this.stage !== ProcessCurrentStageEnum.Finished && this.stage !== ProcessCurrentStageEnum.Pipeline) {
          this.status = referralCurrentStage.onProcess;
          this.message = this.message + this.getStatusMessage();
        }
      }
    }

    private getStatusMessage(){
      if (!this.statusMessage[this.processStatus]){
        return 'N/A';
      } else {
        return this.statusMessage[this.processStatus];
      }
    }
}