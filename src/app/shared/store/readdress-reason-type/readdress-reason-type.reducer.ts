import { createReducer, on } from '@ngrx/store';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
import { readdressReasonTypesActions } from './readdress-reason-type.actions';

export const key = 'readdressReasonTypes';

export interface State {
    readdressReasonTypes: ReaddressReasonType[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    readdressReasonTypes: [],
    errorMsg: null,
    loading: false,
    failed: null
}

export const reducer = createReducer(initialState,
    on(
        readdressReasonTypesActions.add,
        readdressReasonTypesActions.edit,
        (state, { readdressReasonType }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        readdressReasonTypesActions.addSuccess,
        (state, readdressReasonType) => ({
            ...state,
            readdressReasonTypes: [...state.readdressReasonTypes, readdressReasonType],
            loading: false,
            failed: false
        })
    ),
    on(
        readdressReasonTypesActions.loadSuccess,
        (state, { readdressReasonTypes }) => ({
            ...state,
            readdressReasonTypes,
            loading: false,
            failed: false
        })
    ),
    on(
        readdressReasonTypesActions.editSuccess,
        (state, { readdressReasonType }) => {
            const editedReasons = [...state.readdressReasonTypes.filter((value) => value.id !== readdressReasonType.id), readdressReasonType];
            editedReasons.sort((a, b) => a.id - b.id);
            return ({
                ...state,
                readdressReasonTypes: editedReasons,
                loading: false,
                failed: false
            })
        }
    ),
    on(
        readdressReasonTypesActions.removeSuccess,
        (state, { readdressReasonTypeId }) => ({
            ...state,
            readdressReasonTypes: state.readdressReasonTypes.filter(c => c.id !== readdressReasonTypeId),
            loading: false,
            failed: false
        })
    ),
    on(
        readdressReasonTypesActions.loadFailed,
        readdressReasonTypesActions.addFailed,
        readdressReasonTypesActions.editFailed,
        readdressReasonTypesActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        readdressReasonTypesActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectReaddressReasonTypes = (state: State) => state.readdressReasonTypes;
export const selectReaddressReasonTypesErrorMsg = (state: State) => state.errorMsg;
export const selectReaddressReasonTypesLoading = (state: State) => state.loading;
export const selectReaddressReasonTypesFailed = (state: State) => state.failed;
