import { Injectable } from '@angular/core';
import { ProcessStatusEnum } from '../../entities/enums/process-status.enum';
import { StageStatusEnum } from '../../entities/enums/stage-status.enum';
import { SeniorityEnum } from '../../entities/enums/seniority.enum';
import { CandidateStatusEnum } from 'src/entities/enums/candidate-status.enum';
import { DaysOffStatusEnum } from '../../entities/enums/daysoff-status.enum';
import { DaysOffTypeEnum } from '../../entities/enums/daysoff-type.enum';
import { EnglishLevelEnum } from '../../entities/enums/english-level.enum';
import { RejectionReasonsHrEnum } from 'src/entities/enums/rejection-reasons-hr.enum';
import { ProcessCurrentStageEnum } from 'src/entities/enums/process-current-stage';
import { OfferStatusEnum } from 'src/entities/enums/offer-status.enum';

@Injectable()
export class Globals {
  seniorityList: any[] = [
    { id: SeniorityEnum.NA, name: 'NA' }, { id: SeniorityEnum.Junior1, name: 'Junior 1' },
    { id: SeniorityEnum.Junior2, name: 'Junior 2' }, { id: SeniorityEnum.Junior3, name: 'Junior 3' },
    { id: SeniorityEnum.SemiSenior1, name: 'Semi-Senior 1' }, { id: SeniorityEnum.SemiSenior2, name: 'Semi-Senior 2' },
    { id: SeniorityEnum.SemiSenior3, name: 'Semi-Senior 3' },
    { id: SeniorityEnum.Senior1, name: 'Senior 1' }, { id: SeniorityEnum.Senior2, name: 'Senior 2' },
    { id: SeniorityEnum.Senior3, name: 'Senior 3' }
  ];

  processStatusList: any[] = [
    { id: ProcessStatusEnum.NA, name: 'N/A' },
    { id: ProcessStatusEnum.InProgress, name: 'In Progress' }, { id: ProcessStatusEnum.Recall, name: 'Recall' },
    { id: ProcessStatusEnum.Declined, name: 'Declined' },
    { id: ProcessStatusEnum.Hired, name: 'Hired' }, { id: ProcessStatusEnum.Rejected, name: 'Rejected' },
    { id: ProcessStatusEnum.OfferAccepted, name: 'Offer Accepted' },
  ];

  processCurrentStageList: any[] = [
    { id: ProcessCurrentStageEnum.NA, name: 'N/A' },
    { id: ProcessCurrentStageEnum.HrStage, name: 'Hr Stage' },
    { id: ProcessCurrentStageEnum.TechnicalStage, name: 'Technical Stage' },
    { id: ProcessCurrentStageEnum.ClientStage, name: 'Client Stage' },
    { id: ProcessCurrentStageEnum.OfferStage, name: 'Offer Stage' },
    { id: ProcessCurrentStageEnum.Finished, name: 'Finished' }
  ];

  stageStatusList: any[] = [
    { id: StageStatusEnum.NA, name: 'N/A', value: 'NA' },
    { id: StageStatusEnum.InProgress, name: 'In Progress', value: 'InProgress' },
    { id: StageStatusEnum.Accepted, name: 'Accepted', value: 'Accepted' },
    { id: StageStatusEnum.Declined, name: 'Declined', value: 'Declined' },
    { id: StageStatusEnum.Rejected, name: 'Rejected', value: 'Rejected' },
    { id: StageStatusEnum.Hired, name: 'Hired', value: 'Hired' }
  ];

  candidateStatusList: any[] = [
    { id: CandidateStatusEnum.New, name: 'New' }, { id: CandidateStatusEnum.InProgress, name: 'In Progress' },
    { id: CandidateStatusEnum.Recall, name: 'Recall' }, { id: CandidateStatusEnum.Hired, name: 'Hired' },
    { id: CandidateStatusEnum.Rejected, name: 'Rejected' }
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
    { id: EnglishLevelEnum.None, name: 'None' }, { id: EnglishLevelEnum.LowIntermediate, name: 'Low Intermediate' },
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

  offerStatusList: any[] = [
    { id: OfferStatusEnum.Declined, name: 'Declined' },
    { id: OfferStatusEnum.Accepted, name: 'Accepted' },
    { id: OfferStatusEnum.Pending, name: 'Pending' }
  ];
}
