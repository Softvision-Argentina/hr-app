import { CandidateSkill } from "./candidateSkill";
import { CandidateStatusEnum } from "./enums/candidate-status.enum";
import { EnglishLevelEnum } from './enums/english-level.enum';
import { Office } from "./office";
import { Consultant } from "./consultant";
import { Community } from "./community";
import { CandidateProfile } from "./Candidate-Profile";

export class Cv {
  id: number;
  url: string;
  file: string;
}
