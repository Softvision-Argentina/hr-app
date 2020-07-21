import { SeniorityEnum } from "./enums/seniority.enum";
import { Community } from "./community";

export class OpenPosition {
  id: number;
  title: string;
  seniority: SeniorityEnum;
  studio: string;
  community: Community;
  jobDescription: string;
  priority: boolean;
}
