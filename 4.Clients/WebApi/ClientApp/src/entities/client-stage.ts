import { StageStatusEnum } from './enums/stage-status.enum';
import { Stage } from './stage';

export class ClientStage {
    id: number;
    feedback: string;
    status: StageStatusEnum;
    processId: number;
    interviewer: string;
    delegateName: string;
    date?: Date;
    consultantOwnerId?: number;
    consultantDelegateId?: number;
    rejectionReason?: string;
}
