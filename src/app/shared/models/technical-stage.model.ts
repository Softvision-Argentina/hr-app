import { StageStatusEnum } from '../enums/stage-status.enum';
import { SeniorityEnum } from '../enums/seniority.enum';
import { EnglishLevelEnum } from '../enums/english-level.enum';
import { ReaddressStatus } from './readdress-status.model';

export class TechnicalStage {
    id: number;
    feedback: string;
    englishLevel: EnglishLevelEnum;
    status: StageStatusEnum;
    processId: number;
    seniority: SeniorityEnum;
    alternativeSeniority: SeniorityEnum;
    client: string;
    userOwnerId?: number;
    userDelegateId?: number;
    rejectionReason?: string;
    date?: Date;
    sentEmail: boolean;
    readdressStatus: ReaddressStatus;
}
