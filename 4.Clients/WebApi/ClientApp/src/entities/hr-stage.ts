import { StageStatusEnum } from './enums/stage-status.enum';
import { EnglishLevelEnum } from './enums/english-level.enum';
import { Stage } from 'src/entities/stage';
import { RejectionReasonsHrEnum } from './enums/rejection-reasons-hr.enum';

export class HrStage {
    id: number;
    date: Date;
    feedback: string;
    status: StageStatusEnum;
    processId: number;
    actualSalary: number;
    wantedSalary: number;
    englishLevel: EnglishLevelEnum;
    rejectionReasonsHr?: RejectionReasonsHrEnum;
    userOwnerId?: number;
    userDelegateId?: number;
    rejectionReason?: string;
}
