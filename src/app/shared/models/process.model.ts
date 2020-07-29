import { User } from './user.model';
import { Candidate } from './candidate.model';
import { ProcessStatusEnum } from '../enums/process-status.enum';
import { SeniorityEnum } from '../enums/seniority.enum';
import { HrStage } from './hr-stage.model';
import { TechnicalStage } from './technical-stage.model';
import { ClientStage } from './client-stage.model';
import { PreOfferStage } from './pre-offer-stage.model'
import { OfferStage } from './offer-stage.model';
import { EnglishLevelEnum } from '../enums/english-level.enum';
import { ProcessCurrentStageEnum } from '../enums/process-current-stage.enum';
import { DeclineReason } from './decline-reason.model';

export interface Process {
    id: number;
    status: ProcessStatusEnum;
    currentStage: ProcessCurrentStageEnum;
    candidate: Candidate;
    userOwner: User;
    rejectionReason: string;
    declineReason: DeclineReason;
    actualSalary: number;
    wantedSalary: number;
    englishLevel: EnglishLevelEnum;
    seniority: SeniorityEnum;
    hrStage: HrStage;
    technicalStage: TechnicalStage;
    clientStage: ClientStage;
    offerStage: OfferStage
    preOfferStage: PreOfferStage;
    createdDate: Date;
    startDate?: Date;
    endDate?: Date;
    candidateId?: number;
    userDelegateId?: number;
    userOwnerId?: number;
    userDelegate?: User;
    declineReasonId?: number;
}
