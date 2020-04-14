import { Stage } from './stage';
import { User } from './user';
import { Candidate } from './candidate';
import { ProcessStatusEnum } from './enums/process-status.enum';
import { SeniorityEnum } from './enums/seniority.enum';
import { HrStage } from './hr-stage';
import { TechnicalStage } from './technical-stage';
import { ClientStage } from './client-stage';
import { OfferStage } from './offer-stage';
import { EnglishLevelEnum } from './enums/english-level.enum';
import { ProcessCurrentStageEnum } from './enums/process-current-stage';
import { DeclineReason } from './declineReason';

export class Process {
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
    offerStage: OfferStage;
    createdDate: Date;
    startDate?: Date;
    endDate?: Date;
    candidateId?: number;
    userDelegateId?: number;
    userOwnerId?: number;
    userDelegate?: User;
    declineReasonId?: number;
}
