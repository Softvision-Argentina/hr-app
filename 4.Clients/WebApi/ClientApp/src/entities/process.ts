import { Stage } from './stage';
import { Consultant } from './consultant';
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
    consultantOwner: Consultant;
    rejectionReason: string;
    declineReason: DeclineReason;
    actualSalary: number;
    wantedSalary: number;
    agreedSalary: number;
    englishLevel: EnglishLevelEnum;
    seniority: SeniorityEnum;
    hrStage: HrStage;
    technicalStage: TechnicalStage;
    clientStage: ClientStage;
    offerStage: OfferStage;
    startDate?: Date;
    endDate?: Date;
    candidateId?: number;
    consultantDelegateId?: number;
    consultantOwnerId?: number;
    consultantDelegate?: Consultant;
    declineReasonId?: number;
}
