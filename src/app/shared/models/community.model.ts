import { CandidateProfile } from './candidate-profile.model';

export class Community {
  constructor(id?: number) {
    this.id = id;
  }

  id: number;
  name?: string;
  description?: string;
  profileId?: number;
  profile?: CandidateProfile;
  profiles?: CandidateProfile[];
}
