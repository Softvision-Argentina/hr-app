import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Role } from '@shared/models/role.model';
import { RoleService } from '@shared/services/role.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { roleActions } from './role.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class RoleEffects {
    loadRoles$ = createEffect(() =>
        this.action$.pipe(
            ofType(roleActions.load),
            switchMap(() =>
                this.roleService.getRoles()
                    .pipe(
                        map((roles: Role[]) => roleActions.loadSuccess({ roles })),
                        catchError((errorMsg: any) => of(roleActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addRole$ = createEffect(() =>
        this.action$.pipe(
            ofType(roleActions.add),
            exhaustMap(action =>
                this.roleService.add(action.role)
                    .pipe(
                        map((newrole: { id: number }) => {
                            const roles = { ...action.role, id: newrole.id };
                            return roleActions.addSuccess({ role: roles });
                        }),
                        catchError((errorMsg: any) => of(roleActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editRole$ = createEffect(() =>
        this.action$.pipe(
            ofType(roleActions.edit),
            exhaustMap(action =>
                this.roleService.update(action.role.id, action.role)
                    .pipe(
                        map(() => roleActions.editSuccess({ role: action.role })),
                        catchError((errorMsg: any) => of(roleActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeRole$ = createEffect(() =>
        this.action$.pipe(
            ofType(roleActions.remove),
            exhaustMap((action) =>
                this.roleService.delete(action.roleId)
                    .pipe(
                        map(() => roleActions.removeSuccess({ roleId: action.roleId })),
                        catchError((errorMsg: any) => of(roleActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private roleService: RoleService) {

    }
}
