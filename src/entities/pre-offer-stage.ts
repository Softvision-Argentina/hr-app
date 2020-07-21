import { StageStatusEnum } from './enums/stage-status.enum';
import { SeniorityEnum } from './enums/seniority.enum';
import { HealthInsuranceEnum } from './enums/health-insurance.enum';
import { ReaddressStatus } from './ReaddressStatus';

export class preOfferStage {
  id: number;
  date?: Date;
  dni: number;
  feedback: string;
  status: StageStatusEnum;
  userOwnerId?: number;
  userDelegateId?: number;
  processId: number;
  rejectionReason?: string;
  remunerationOffer: number;
  vacationDays: number;
  healthInsurance: HealthInsuranceEnum
  notes: string;
  firstday: Date;
  bonus: string;
  hireDate: Date;
  seniority: SeniorityEnum;
  backgroundCheckDone: boolean;
  backgroundCheckDoneDate: Date;
  preocupationalDone: boolean;
  preocupationalDoneDate: Date;
  readdressStatus: ReaddressStatus;
}
