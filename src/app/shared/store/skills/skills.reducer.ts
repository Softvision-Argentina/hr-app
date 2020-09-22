import { createReducer, on } from '@ngrx/store';
import { Skill } from '@shared/models/skill.model';
import { skillsActions } from './skills.actions';

export const key = 'skills';

export interface State {
    skills: Skill[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    skills: [],
    errorMsg: null,
    loading: false,
    failed: null
}

export const reducer = createReducer(initialState,
    on(
        skillsActions.add,
        skillsActions.edit,
        (state, { skill }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        skillsActions.addSuccess,
        (state, skill) => {
            const orderedSkills = [...state.skills, skill].sort((a, b) => (a.name.localeCompare(b.name)));
            return {
                ...state,
                skills: orderedSkills,
                loading: false,
                failed: false
            }
        }
    ),
    on(
        skillsActions.loadSuccess,
        (state, { skills }) => {
            const orderedSkills = skills.slice().sort((a, b) => (a.name.localeCompare(b.name)));
            return {
                ...state,
                skills: orderedSkills,
                loading: false,
                failed: false
            }
        }
    ),
    on(
        skillsActions.editSuccess,
        (state, { skill }) => {
            const editedSkills = [...state.skills.filter((value) => value.id !== skill.id), skill]
            editedSkills.sort((a, b) => (a.name.localeCompare(b.name)));
            return ({
                ...state,
                skills: editedSkills,
                loading: false,
                failed: false
            })
        }
    ),
    on(
        skillsActions.removeSuccess,
        (state, { skillId }) => ({
            ...state,
            skills: state.skills.filter(skill => skill.id !== skillId),
            loading: false,
            failed: false
        })
    ),
    on(
        skillsActions.loadFailed,
        skillsActions.addFailed,
        skillsActions.editFailed,
        skillsActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        skillsActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectSkills = (state: State) => state.skills;
export const selectSkillsErrorMsg = (state: State) => state.errorMsg;
export const selectSkillsLoading = (state: State) => state.loading;
export const selectSkillsFailed = (state: State) => state.failed;
