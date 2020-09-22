import { ActionReducerMap, createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromDeclineReason from './decline-reasons.reducer';

export interface State {
    [fromDeclineReason.key]: fromDeclineReason.State;
}

export const declineReasonReducers: ActionReducerMap<State> = {
    [fromDeclineReason.key]: fromDeclineReason.reducer,
};

export const selectDeclineReasonState = createFeatureSelector<State, fromDeclineReason.State>(fromDeclineReason.key);

export const selectDeclineReasons = createSelector(
    selectDeclineReasonState,
    fromDeclineReason.selectDeclineReasons
);
export const selectDeclineReasonErrorMsg = createSelector(
    selectDeclineReasonState,
    fromDeclineReason.selectDeclineReasonErrorMsg
);
export const selectDeclineReasonLoading = createSelector(
    selectDeclineReasonState,
    fromDeclineReason.selectDeclineReasonLoading
);
export const selectDeclineReasonFailed = createSelector(
    selectDeclineReasonState,
    fromDeclineReason.selectDeclineReasonFailed
);
