import { Candidate } from './candidate';
import { Skill } from './skill';

export class CandidateSkill {
    candidateId: number;
    candidate: Candidate;
    skillId: number;
    skill: Skill;
    rate: number;
    comment: string;
}
