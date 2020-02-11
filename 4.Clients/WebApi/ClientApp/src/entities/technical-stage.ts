import { StageStatusEnum } from './enums/stage-status.enum';
import { SeniorityEnum } from './enums/seniority.enum';
import { Stage } from './stage';

export class TechnicalStage {
    id: number;
    date: Date;
    feedback: string;
    status: StageStatusEnum;
    consultantOwnerId: number;
    consultantDelegateId: number;
    processId: number;
    rejectionReason?: string;
    seniority: SeniorityEnum;
    alternativeSeniority: SeniorityEnum;
    client: string;
}
