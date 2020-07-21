import { StageStatusEnum } from './enums/stage-status.enum';
import { Interview } from 'src/entities/interview';
import { ReaddressStatus } from './ReaddressStatus';

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
    interviews?: Interview[];
    readdressStatus: ReaddressStatus;
}
