import { Community } from './community';

export class CandidateProfile {
    constructor(id?: number) {
        this.id = id;
    }
    id: number;
    name: string;
    description: string;
    communityItems: Community[];
}
