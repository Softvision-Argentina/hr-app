import { Candidate } from './candidate.model';
import { Community } from './community.model';
import { Profile } from './profile.model';
import { User } from './user.model';

export interface ProcessTableView{
    id: number;
    status: number;
    currentState: 1;
    candidate: Candidate;
    community: Community;
    profile: Profile;
    userOwner: User;
    seniority: number;
}