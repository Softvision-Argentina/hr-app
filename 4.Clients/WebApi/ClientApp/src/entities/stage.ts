import { StageStatusEnum } from './enums/stage-status.enum';

export class Stage {
    id: number;
    date: Date;
    feedback: string;
    status: StageStatusEnum;
    userOwnerId: number;
    userDelegateId: number;
    processId: number;
    rejectionReason?: string;
}
