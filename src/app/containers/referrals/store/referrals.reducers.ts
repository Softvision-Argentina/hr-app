import { createReducer, on, createSelector, createFeatureSelector } from '@ngrx/store';
import { Candidate } from '@shared/models/candidate.model';
import { referralsActions } from './referrals.actions';
import { ReferralListItem } from '@shared/models/referral-list-item.model';

export const key = 'referral';

export interface ReferralsState {
    referrals: any[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}
export const initialState: ReferralsState = {
    referrals: [],
    errorMsg: null,
    loading: false,
    failed: false
};

const selectAllReferrals = createFeatureSelector<ReferralsState>('referrals');

export const getReferralsState = createSelector(
    selectAllReferrals,
    (state: ReferralsState) => state.referrals
);

export const getReferralsBiggerThan = createSelector(
    selectAllReferrals,
    (state: ReferralsState, props) => state.referrals.filter(task => task > props.bigger)
);

export const getReferralsLoading = createSelector(
    selectAllReferrals,
    (state: ReferralsState) => state.loading
);

export const getReferralsError = createSelector(
    selectAllReferrals,
    (state: ReferralsState) => state.failed
);

export const _referralsReducer = createReducer(initialState,
    on(
        referralsActions.add,
        referralsActions.edit,
        (state, { referral }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        referralsActions.addSuccess,
        (state, { referral, referralId }) => {

            const referralForStore = { ...referral, id: referralId };
            const referralItem = new ReferralListItem(referralForStore, 0, 0, 0);

            return {
                ...state,
                referrals: [...state.referrals, referralItem],
                loading: false,
                failed: false
            };
        }
    ),
    on(
        referralsActions.loadSuccess,
        (state, { referrals }) => {
            const referralsToShow = referrals.map(ref =>
                new ReferralListItem(ref.candidate,
                    ref.processId,
                    ref.processStage,
                    ref.processStatus)
            );
            return {
                ...state,
                referrals: referralsToShow,
                loading: false,
                failed: false
            };
        }
    ),
    on(
        referralsActions.load,
        (state) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),
    on(
        referralsActions.editSuccess,
        (state, { referral }) => {

            return {
                ...state,
                referrals: [...state.referrals.filter(c => c.candidate.id !== referral.candidate.id), referral],
                loading: false,
                failed: false
            };
        }
    ),
    on(
        referralsActions.remove,
        (state) => ({
            ...state,
            loading: false,
            failed: false
        })
    ),
    on(
        referralsActions.removeSuccess,
        (state, { referralId }) => ({
            ...state,
            referrals: state.referrals.filter(c => c.candidate.id !== referralId),
            loading: false,
            failed: false
        })
    ),
    on(
        referralsActions.loadFailed,
        referralsActions.addFailed,
        referralsActions.editFailed,
        referralsActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    )
);

export const selectReferrals = (state: ReferralsState) => state.referrals;
export const selectReferralsErrorMsg = (state: ReferralsState) => state.errorMsg;
export const selectReferralsLoading = (state: ReferralsState) => state.loading;
export const selectReferralsFailed = (state: ReferralsState) => state.failed;


export function referralsReducer(state, action) {
    return _referralsReducer(state, action);
}
