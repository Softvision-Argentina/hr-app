import { createReducer, on } from '@ngrx/store';
import { SkillType } from '@shared/models/skill-type.model';
import { skillTypeActions } from './skill-type.actions';

export const key = 'skillTypes';

export interface State {
    skillTypes: SkillType[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    skillTypes: [],
    errorMsg: null,
    loading: false,
    failed: null
}

export const reducer = createReducer(initialState,
    on(
        skillTypeActions.add,
        skillTypeActions.edit,
        (state, { skillType }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        skillTypeActions.addSuccess,
        (state, skillType) => ({
            ...state,
            skillTypes: [...state.skillTypes, skillType],
            loading: false,
            failed: false
        })
    ),
    on(
        skillTypeActions.loadSuccess,
        (state, { skillTypes }) => ({
            ...state,
            skillTypes,
            loading: false,
            failed: false
        })
    ),
    on(
        skillTypeActions.editSuccess,
        (state, { skillType }) => {
            const editedskillTypes = [...state.skillTypes.filter((value) => value.id !== skillType.id), skillType];
            editedskillTypes.sort((a, b) => a.id - b.id);
            return {
                ...state,
                skillTypes: editedskillTypes,
                loading: false,
                failed: false
            }
        }
    ),
    on(
        skillTypeActions.removeSuccess,
        (state, { skillTypeId }) => ({
            ...state,
            skillTypes: state.skillTypes.filter(c => c.id !== skillTypeId),
            loading: false,
            failed: false
        })
    ),
    on(
        skillTypeActions.loadFailed,
        skillTypeActions.addFailed,
        skillTypeActions.editFailed,
        skillTypeActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        skillTypeActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectSkillTypes = (state: State) => state.skillTypes;
export const selectSkillTypesErrorMsg = (state: State) => state.errorMsg;
export const selectSkillTypesLoading = (state: State) => state.loading;
export const selectSkillTypesFailed = (state: State) => state.failed;
