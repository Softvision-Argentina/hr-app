import { Injectable } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { State, selectRoleFailed, selectRoleLoading, selectRoleErrorMsg, selectRoles } from './store/index';
import { roleActions } from './store/role.actions';
import { Role } from '@shared/models/role.model';
@Injectable()

export class RoleSandbox {

    roles$ = this.roleState$.pipe(
        select(selectRoles)
    );

    roleLoading$ = this.roleState$.pipe(
        select(selectRoleLoading)
    );

    roleFailed$ = this.roleState$.pipe(
        select(selectRoleFailed)
    );

    roleErrorMsg$ = this.roleState$.pipe(
        select(selectRoleErrorMsg)
    );

    constructor(private roleState$: Store<State>) {
    }

    loadRoles(): void {
        this.roleState$.dispatch(roleActions.load());
    }

    addRole(role: Role) {
        this.roleState$.dispatch(roleActions.add({ role }));
    }

    removeRole(roleId: number) {
        this.roleState$.dispatch(roleActions.remove({ roleId }));
    }

    editRole(role: Role) {
        this.roleState$.dispatch(roleActions.edit({ role }));
    }

    resetFailed() {
        this.roleState$.dispatch(roleActions.resetFailed());
    }
}
