import { ActionReducerMap, createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromRole from './role.reducers';

export interface State {
    [fromRole.key]: fromRole.State;
}

export const RoleReducers: ActionReducerMap<State> = {
    [fromRole.key]: fromRole.reducer,
};

export const selectRoleState = createFeatureSelector<State, fromRole.State>(fromRole.key);

export const selectRoles = createSelector(
    selectRoleState,
    fromRole.selectRoles
);
export const selectRoleErrorMsg = createSelector(
    selectRoleState,
    fromRole.selectRoleErrorMsg
);
export const selectRoleLoading = createSelector(
    selectRoleState,
    fromRole.selectRoleLoading
);
export const selectRoleFailed = createSelector(
    selectRoleState,
    fromRole.selectRoleFailed
);
