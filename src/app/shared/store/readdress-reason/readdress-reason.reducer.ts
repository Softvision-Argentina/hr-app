import { createReducer, on } from '@ngrx/store';
import { ReaddressReason } from '@shared/models/readdress-reason.model';
import { readdressReasonActions } from './readdress-reason.actions';

export const key = 'readdressReasons';

export interface State {
    readdressReasons: ReaddressReason[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    readdressReasons: [],
    errorMsg: null,
    loading: false,
    failed: null
}

export const reducer = createReducer(initialState,
    on(
        readdressReasonActions.add,
        readdressReasonActions.edit,
        (state, { readdressReason }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        readdressReasonActions.addSuccess,
        (state, { readdressReason }) => ({
            ...state,
            readdressReasons: [...state.readdressReasons, readdressReason],
            loading: false,
            failed: false
        })
    ),
    on(
        readdressReasonActions.loadSuccess,
        (state, { readdressReasons }) => ({
            ...state,
            readdressReasons,
            loading: false,
            failed: false
        })
    ),
    on(
        readdressReasonActions.editSuccess,
        (state, { readdressReason }) => {
            const editedReasons = [...state.readdressReasons.filter((value) => value.id !== readdressReason.id), readdressReason];
            editedReasons.sort((a, b) => a.id - b.id);
            return {
                ...state,
                readdressReasons: editedReasons,
                loading: false,
                failed: false
            }
        }
    ),
    on(
        readdressReasonActions.removeSuccess,
        (state, { readdressReasonId }) => ({
            ...state,
            readdressReasons: state.readdressReasons.filter(c => c.id !== readdressReasonId),
            loading: false,
            failed: false
        })
    ),
    on(
        readdressReasonActions.loadFailed,
        readdressReasonActions.addFailed,
        readdressReasonActions.editFailed,
        readdressReasonActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        readdressReasonActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectReaddressReasons = (state: State) => state.readdressReasons;
export const selectReaddressReasonsErrorMsg = (state: State) => state.errorMsg;
export const selectReaddressReasonsLoading = (state: State) => state.loading;
export const selectReaddressReasonsFailed = (state: State) => state.failed;
