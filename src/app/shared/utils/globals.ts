import { Injectable } from '@angular/core';
import { CandidateStatusEnum } from '@app/shared/enums/candidate-status.enum';
import { DaysOffStatusEnum } from '@shared/enums/daysoff-status.enum';
import { DaysOffTypeEnum } from '@shared/enums/daysoff-type.enum';
import { EnglishLevelEnum } from '@shared/enums/english-level.enum';
import { HealthInsuranceEnum } from '@shared/enums/health-insurance.enum';
import { OfferStatusEnum } from '@shared/enums/offer-status.enum';
import { ProcessCurrentStageEnum } from '@shared/enums/process-current-stage.enum';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { referralCurrentStage } from '@shared/enums/referral-currentStage.enum';
import { RejectionReasonsHrEnum } from '@shared/enums/rejection-reasons-hr.enum';
import { SeniorityEnum } from '@shared/enums/seniority.enum';
import { StageStatusEnum } from '@shared/enums/stage-status.enum';
import { SeniorityForOpenPositionsEnum } from '@shared/enums/seniority-for-open-positions.enum';

@Injectable()
export class Globals {
  seniorityList: any[] = [
    { id: SeniorityEnum.NA, name: 'N/A' }, { id: SeniorityEnum.Junior1, name: 'Junior 1' },
    { id: SeniorityEnum.Junior2, name: 'Junior 2' },
    { id: SeniorityEnum.SemiSenior1, name: 'Semi-Senior 1' }, { id: SeniorityEnum.SemiSenior2, name: 'Semi-Senior 2' },
    { id: SeniorityEnum.SemiSenior3, name: 'Semi-Senior 3' },
    { id: SeniorityEnum.Senior1, name: 'Senior 1' }, { id: SeniorityEnum.Senior2, name: 'Senior 2' },
    { id: SeniorityEnum.Senior3, name: 'Senior 3' }
  ];

  seniorityListForAddPosition: any[] = [
    { id: SeniorityForOpenPositionsEnum.NA, name: 'N/A' },
    { id: SeniorityForOpenPositionsEnum.Junior, name: 'Junior' },
    { id: SeniorityForOpenPositionsEnum.MidLevel, name: 'Mid Level' },
    { id: SeniorityForOpenPositionsEnum.Senior, name: 'Senior' }
  ];

  processStatusList: any[] = [
    { id: ProcessStatusEnum.NA, name: 'N/A' },
    { id: ProcessStatusEnum.InProgress, name: 'In Progress' },
    { id: ProcessStatusEnum.Accepted, name: 'Accepted' },
    { id: ProcessStatusEnum.Rejected, name: 'Rejected' },
    { id: ProcessStatusEnum.Declined, name: 'Declined' },
    { id: ProcessStatusEnum.Hired, name: 'Hired' },
    { id: ProcessStatusEnum.Recall, name: 'Recall' },
    { id: ProcessStatusEnum.Pipeline, name: 'Pipeline' },
    { id: ProcessStatusEnum.PendingReply, name: 'Pending Reply' },
    { id: ProcessStatusEnum.Eliminated, name: 'Eliminated' }

  ];

  processCurrentStageList: any[] = [
    { id: ProcessCurrentStageEnum.NA, name: 'N/A' },
    { id: ProcessCurrentStageEnum.HrStage, name: 'Hr Stage' },
    { id: ProcessCurrentStageEnum.TechnicalStage, name: 'Technical Stage' },
    { id: ProcessCurrentStageEnum.ClientStage, name: 'Client Stage' },
    { id: ProcessCurrentStageEnum.PreOfferStage, name: 'Pre Offer Stage' },
    { id: ProcessCurrentStageEnum.OfferStage, name: 'Offer Stage' },
    { id: ProcessCurrentStageEnum.Finished, name: 'Finished' },
    { id: ProcessCurrentStageEnum.Pipeline, name: 'Pipeline' }
  ];

  stageStatusList: any[] = [
    { id: StageStatusEnum.NA, name: 'N/A' },
    { id: StageStatusEnum.InProgress, name: 'In Progress', value: 'In Progress' },
    { id: StageStatusEnum.Accepted, name: 'Accepted', value: 'Accepted' },
    { id: StageStatusEnum.Declined, name: 'Declined', value: 'Declined' },
    { id: StageStatusEnum.Rejected, name: 'Rejected', value: 'Rejected' },
    { id: StageStatusEnum.Hired, name: 'Hired', value: 'Hired' },
    { id: StageStatusEnum.Recall, name: 'Recall', value: 'Recall' },
    { id: StageStatusEnum.Pipeline, name: 'Pipeline', value: 'Pipeline' }
  ];

  candidateStatusList: any[] = [
    { id: CandidateStatusEnum.New, name: 'New' },
    { id: CandidateStatusEnum.InProgress, name: 'In Progress' },
    { id: CandidateStatusEnum.Recall, name: 'Recall' },
    { id: CandidateStatusEnum.Hired, name: 'Hired' },
    { id: CandidateStatusEnum.Rejected, name: 'Rejected' },
    { id: CandidateStatusEnum.Accepted, name: 'Accepted' },
    { id: CandidateStatusEnum.Declined, name: 'Declined' },
    { id: CandidateStatusEnum.PendingReply, name: 'Pending Reply' },
    { id: CandidateStatusEnum.Pipeline, name: 'Pipeline' },
    { id: CandidateStatusEnum.Eliminated, name: 'Eliminated' }
  ];

  profileList: any[] = [
    { id: 0, name: 'ALL' },
    { id: 1, name: 'DEV' },
    { id: 2, name: 'FullStack' },
    { id: 3, name: 'UX' },
    { id: 4, name: 'QA' },
    { id: 5, name: 'HR' }
  ];

  englishLevelList: any[] = [
    { id: EnglishLevelEnum.None, name: 'N/A' }, { id: EnglishLevelEnum.LowIntermediate, name: 'Low Intermediate' },
    { id: EnglishLevelEnum.HighIntermediate, name: 'High Intermediate' }, { id: EnglishLevelEnum.Advanced, name: 'Advanced' },
  ];

  daysOffTypeList: any[] = [
    { id: DaysOffTypeEnum.Holidays, name: 'Holidays' }, { id: DaysOffTypeEnum.PTO, name: 'PTO' },
    { id: DaysOffTypeEnum.StudyDays, name: 'Study Days' }, { id: DaysOffTypeEnum.Training, name: 'Training' }
  ];

  daysOffStatusList: any[] = [
    { id: DaysOffStatusEnum.InReview, name: 'In Review' },
    { id: DaysOffStatusEnum.Accepted, name: 'Accepted' }
  ];

  rejectionReasonsHRList: any[] = [
    { id: RejectionReasonsHrEnum.SalaryExpectations, name: 'Salary Expectations' },
    { id: RejectionReasonsHrEnum.Skills, name: 'Skills' },
    { id: RejectionReasonsHrEnum.EnglishLevel, name: 'English Level' },
    { id: RejectionReasonsHrEnum.Residence, name: 'Residence' },
    { id: RejectionReasonsHrEnum.Other, name: 'Other' }
  ];

  hrStageStatusList: any[] = [
    { id: StageStatusEnum.NA, name: 'N/A' },
    { id: StageStatusEnum.InProgress, name: 'In Progress', value: 'In Progress' },
    { id: StageStatusEnum.Accepted, name: 'Accepted', value: 'Accepted' },
    { id: StageStatusEnum.Declined, name: 'Declined', value: 'Declined' },
    { id: StageStatusEnum.Rejected, name: 'Rejected', value: 'Rejected' },
    { id: StageStatusEnum.Recall, name: 'Recall', value: 'Recall' },
    { id: StageStatusEnum.Pipeline, name: 'Pipeline', value: 'Pipeline' },
    { id: StageStatusEnum.PendingReply, name: 'Pending Reply', value: 'Pending Reply' }
  ];

  preOfferStatusList: any[] = [
    { id: StageStatusEnum.NA, name: 'N/A' },
    { id: StageStatusEnum.PendingReply, name: 'Pending Reply', value: 'Pending Reply' },
    { id: StageStatusEnum.Accepted, name: 'Accepted', value: 'Accepted' },
    { id: StageStatusEnum.Declined, name: 'Declined', value: 'Declined' },
    { id: OfferStatusEnum.Rejected, name: 'Rejected' },
  ];

  offerStatusList: any[] = [
    { id: OfferStatusEnum.NA, name: 'N/A' },
    { id: OfferStatusEnum.InProgress, name: 'In Progress' },
    { id: OfferStatusEnum.Rejected, name: 'Rejected' },
    { id: OfferStatusEnum.Declined, name: 'Declined' },
    { id: OfferStatusEnum.Hired, name: 'Hired' }
  ];

  healthInsuranceList: any[] = [
    { id: HealthInsuranceEnum.NA, name: 'N/A'},
    { id: HealthInsuranceEnum.OSDE210, name: 'OSDE 21O'},
    { id: HealthInsuranceEnum.OSDE310, name: 'OSDE 310'},
    { id: HealthInsuranceEnum.OSDE410, name: 'OSDE 41O'},
    { id: HealthInsuranceEnum.ContinuesWithCurrentone, name: 'Continues With Current One'},
    { id: HealthInsuranceEnum.Other, name: 'Other'}
  ];

  referralCurrentStage: any[] =[
    {id: referralCurrentStage.new, name: 'New' },
    {id: referralCurrentStage.pipeline, name: 'Pipeline' },
    {id: referralCurrentStage.onProcess, name: 'On process' },
    {id: referralCurrentStage.contracted, name: 'Contracted' }
  ]
}
