import { Candidate } from './candidate';
import { referralCurrentStage } from './enums/referral-currentStage.enum';

export class ReferraListItem{
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
