import { StageStatusEnum } from './enums/stage-status.enum';
import { SeniorityEnum } from './enums/seniority.enum';

export class OfferStage {
    id: number;
    date?: Date;
    feedback: string;
    status: StageStatusEnum;
    userOwnerId?: number;
    userDelegateId?: number;
    processId: number;
    rejectionReason?: string;
    hireDate: Date;
    seniority: SeniorityEnum;
    backgroundCheckDone: boolean;
    backgroundCheckDoneDate: Date;
    preocupationalDone: boolean;
    preocupationalDoneDate: Date;
}
