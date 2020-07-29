import { Candidate } from './candidate.model';
import { referralCurrentStage } from '../enums/referral-currentStage.enum';

export interface ReferralListItem {
    candidate: Candidate;
    currentStatus: {
        stage: {
            id: number,
            name: string,
        }
        status: referralCurrentStage,
        message: string
    };
}
