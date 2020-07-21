import { StageStatusEnum } from "./enums/stage-status.enum";
import { ProcessCurrentStageEnum } from "./enums/process-current-stage";

export class ReaddressStatus {
  id: number;
  fromStatus: StageStatusEnum;
  toStatus: StageStatusEnum;
  readdressReasonId: number;
  feedback: string;
}