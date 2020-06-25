import { SeniorityEnum } from "./enums/seniority.enum";
import { Community } from "./community";

export class OpenPosition {
  title: string;
  seniority: SeniorityEnum;
  studio: string;
  community: Community;
  priority: boolean;
}
