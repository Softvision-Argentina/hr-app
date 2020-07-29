import { StageStatusEnum } from '../enums/stage-status.enum';
import { Interview } from './interview.model';
import { ReaddressStatus } from './readdress-status.model';

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
