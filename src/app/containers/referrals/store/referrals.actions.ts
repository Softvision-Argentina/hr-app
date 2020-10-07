import { createAction, props } from '@ngrx/store';
import { Candidate } from '@shared/models/candidate.model';

export const referralsActions = {
    add: createAction('[Referrals] add', props<{ referral: any ,file: any}>()),
    addSuccess: createAction('[Referrals] addSuccess', props<{ referral: Candidate, referralId: number }>()),
    addFailed: createAction('[Referrals] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[Referral List] load'),
    loadSuccess: createAction('[Referrals] loadSuccess', props<{ referrals: any[] }>()),
    loadFailed: createAction('[Referrals] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[Referrals] edit', props<{ referral: any, referralId: number,file: any}>()),
    editSuccess: createAction('[Referrals] editSuccess', props<{ referral: any }>()),
    editFailed: createAction('[Referrals] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[Referrals] remove', props<{ referralId: number }>()),
    removeSuccess: createAction('[Referrals] removeSuccess', props<{ referralId: number }>()),
    removeFailed: createAction('[Referrals] removeFailed', props<{ errorMsg: any }>()),
}