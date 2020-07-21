import { CandidateSkill } from './candidateSkill';

export class Skill {
    id: number;
    name: string;
    description: string;
    type: number;

    candidateSkills: CandidateSkill[];
}
