import { StageStatusEnum } from './enums/stage-status.enum';
import { SeniorityEnum } from './enums/seniority.enum';
import { Stage } from './stage';
import { Offer } from './offer';

export class OfferStage {
    id: number;
    date?: Date;
    feedback: string;
    status: StageStatusEnum;
    consultantOwnerId?: number;
    consultantDelegateId?: number;
    processId: number;
    rejectionReason?: string;
    offerDate: Date;
    hireDate: Date;
    seniority: SeniorityEnum;    
    backgroundCheckDone: boolean;
    backgroundCheckDoneDate: Date;
    preocupationalDone: boolean;
    preocupationalDoneDate: Date;
    offers : Offer[];
}
