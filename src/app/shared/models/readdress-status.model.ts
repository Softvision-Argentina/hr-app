import { StageStatusEnum } from "../enums/stage-status.enum";

export class ReaddressStatus {
  id: number;
  fromStatus: StageStatusEnum;
  toStatus: StageStatusEnum;
  readdressReasonId: number;
  feedback: string;
}