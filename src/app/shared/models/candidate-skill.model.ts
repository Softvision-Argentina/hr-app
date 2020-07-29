import { Candidate } from './candidate.model';
import { Skill } from './skill.model';

export interface CandidateSkill {
    candidateId: number;
    candidate: Candidate;
    skillId: number;
    skill: Skill;
    rate: number;
    comment: string;
}
