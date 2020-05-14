import { CandidateSkill } from './candidateSkill';
import { CandidateStatusEnum } from './enums/candidate-status.enum';
import { EnglishLevelEnum } from './enums/english-level.enum';
import { User } from './user';
import { Community } from './community';
import { CandidateProfile } from './Candidate-Profile';

export class Candidate {
  id: number;
  name: string;
  lastName: string;
  emailAddress: string;
  phoneNumber: string;
  englishLevel: EnglishLevelEnum;
  status: CandidateStatusEnum;
  candidateSkills: CandidateSkill[];
  user: User;
  preferredOfficeId?: number;
  contactDay: Date;
  profile: CandidateProfile;
  community: Community;
  isReferred: boolean;
  linkedInProfile?: string;
  referredBy: string;
  knownFrom: string;
  cv: string;
  dni?: number;
}
