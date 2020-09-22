import { createReducer, on } from '@ngrx/store';
import { DeclineReason } from '@shared/models/decline-reason.model';
import { declineReasonActions } from './decline-reasons.actions';

export const key = 'declineReasons';

export interface State {
    reasons: DeclineReason[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    reasons: [],
    errorMsg: null,
    loading: false,
    failed: null
};

export const reducer = createReducer(initialState,
    on(
        declineReasonActions.add,
        declineReasonActions.edit,
        (state) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        declineReasonActions.addSuccess,
        (state, { reason }) => {
            return ({
                ...state,
                reasons: [...state.reasons, reason],
                loading: false,
                failed: false
            });
        }
    ),
    on(
        declineReasonActions.loadSuccess,
        (state, { reasons }) => ({
            ...state,
            reasons,
            loading: false,
            failed: false
        })
    ),
    on(
        declineReasonActions.editSuccess,
        (state, { reason }) => {
            const editedRreasons = [...state.reasons.filter((value) => value.id !== reason.id), reason];
            editedRreasons.sort((a, b) => a.id - b.id);
            return {
                ...state,
                reasons: editedRreasons,
                loading: false,
                failed: false
            };
        }
    ),
    on(
        declineReasonActions.removeSuccess,
        (state, { reasonId }) => ({
            ...state,
            reasons: state.reasons.filter(c => c.id !== reasonId),
            loading: false,
            failed: false
        })
    ),
    on(
        declineReasonActions.loadFailed,
        declineReasonActions.addFailed,
        declineReasonActions.editFailed,
        declineReasonActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        declineReasonActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectDeclineReasons = (state: State) => state.reasons;
export const selectDeclineReasonErrorMsg = (state: State) => state.errorMsg;
export const selectDeclineReasonLoading = (state: State) => state.loading;
export const selectDeclineReasonFailed = (state: State) => state.failed;
