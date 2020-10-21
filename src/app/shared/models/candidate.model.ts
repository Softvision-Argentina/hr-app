import { CandidateSkill } from './candidate-skill.model';
import { CandidateStatusEnum } from '../enums/candidate-status.enum';
import { EnglishLevelEnum } from '../enums/english-level.enum';
import { User } from './user.model';
import { Community } from './community.model';
import { CandidateProfile } from './candidate-profile.model';
import { OpenPosition } from './open-position.model';

export interface Candidate {
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
  openPosition?: OpenPosition;
  openPositionTitle?: string;
  source: string;
}
