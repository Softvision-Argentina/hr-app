import { StageStatusEnum } from './enums/stage-status.enum';
import { SeniorityEnum } from './enums/seniority.enum';
import { Stage } from './stage';

export class TechnicalStage {
    id: number;
    feedback: string;
    status: StageStatusEnum;
    processId: number;
    seniority: SeniorityEnum;
    alternativeSeniority: SeniorityEnum;
    client: string;
    consultantOwnerId?: number;
    consultantDelegateId?: number;
    rejectionReason?: string;
    date?: Date;
}
