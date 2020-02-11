import { StageStatusEnum } from './enums/stage-status.enum';
import { Stage } from './stage';

export class ClientStage {
    id: number;
    date: Date;
    feedback: string;
    status: StageStatusEnum;
    consultantOwnerId: number;
    consultantDelegateId: number;
    processId: number;
    rejectionReason?: string;
    interviewer: string;
    delegateName : string;
}
