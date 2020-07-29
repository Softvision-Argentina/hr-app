import { CandidateSkill } from './candidate-skill.model';

export interface Skill {
    id: number;
    name: string;
    description: string;
    type: number;
    candidateSkills: CandidateSkill[];
}
