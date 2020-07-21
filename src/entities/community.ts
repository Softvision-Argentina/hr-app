import { CandidateProfile } from './Candidate-Profile';

export class Community {
  constructor(id?: number) {
    this.id = id;
  }
    id: number;
    name: string;
    description: string;
    profileId: number;
    profile: CandidateProfile;
  }
