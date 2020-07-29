import { SeniorityEnum } from "../enums/seniority.enum";
import { Community } from "./community.model";

export interface OpenPosition {
  id: number;
  title: string;
  seniority: SeniorityEnum;
  studio: string;
  community: Community;
  jobDescription: string;
  priority: boolean;
}
