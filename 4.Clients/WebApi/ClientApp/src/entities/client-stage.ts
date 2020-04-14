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
    userOwnerId?: number;
    userDelegateId?: number;
    rejectionReason?: string;
}
