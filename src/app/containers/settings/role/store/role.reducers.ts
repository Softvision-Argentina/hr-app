import { createReducer, on } from '@ngrx/store';
import { Role } from '@shared/models/role.model';
import { roleActions } from './role.actions';

export const key = 'role';

export interface State {
    roles: Role[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    roles: [],
    errorMsg: null,
    loading: false,
    failed: null
};

export const reducer = createReducer(initialState,
    on(
        roleActions.add,
        roleActions.edit,
        (state) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        roleActions.addSuccess,
        (state, { role }) => {
            return ({
                ...state,
                roles: [...state.roles, role],
                loading: false,
                failed: false
            });
        }
    ),
    on(
        roleActions.loadSuccess,
        (state, { roles }) => ({
            ...state,
            roles,
            loading: false,
            failed: false
        })
    ),
    on(
        roleActions.editSuccess,
        (state, { role }) => {
            const editedRoles = [...state.roles.filter((value) => value.id !== role.id), role];
            editedRoles.sort((a, b) => a.id - b.id);
            return ({
                ...state,
                roles: editedRoles,
                loading: false,
                failed: false
            })
        }
    ),
    on(
        roleActions.removeSuccess,
        (state, { roleId }) => ({
            ...state,
            roles: state.roles.filter(c => c.id !== roleId),
            loading: false,
            failed: false
        })
    ),
    on(
        roleActions.loadFailed,
        roleActions.addFailed,
        roleActions.editFailed,
        roleActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    ),
    on(
        roleActions.resetFailed,
        (state) => ({
            ...state,
            loading: true,
            failed: null
        })
    )
);

export const selectRoles = (state: State) => state.roles;
export const selectRoleErrorMsg = (state: State) => state.errorMsg;
export const selectRoleLoading = (state: State) => state.loading;
export const selectRoleFailed = (state: State) => state.failed;
