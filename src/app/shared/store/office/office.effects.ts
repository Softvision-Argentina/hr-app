import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Office } from '@shared/models/office.model';
import { OfficeService } from '@shared/services/office.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { officeActions } from './office.actions';

@Injectable()
export class OfficeEffects {
    loadOffices$ = createEffect(() =>
        this.action$.pipe(
            ofType(officeActions.load),
            switchMap(() =>
                this.officeService.getOffices()
                    .pipe(
                        map((offices: Office[]) => officeActions.loadSuccess({ offices })),
                        catchError((errorMsg: any) => of(officeActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addOffice$ = createEffect(() =>
        this.action$.pipe(
            ofType(officeActions.add),
            exhaustMap(action =>
                this.officeService.add(action.office)
                    .pipe(
                        map((office: { id: number }) => officeActions.addSuccess({ ...action.office, id: office.id })),
                        catchError((errorMsg: any) => of(officeActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editOffice$ = createEffect(() =>
        this.action$.pipe(
            ofType(officeActions.edit),
            exhaustMap(action =>
                this.officeService.update(action.office.id, action.office)
                    .pipe(
                        map(() => officeActions.editSuccess({ office: action.office })),
                        catchError((errorMsg: any) => of(officeActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeOffice$ = createEffect(() =>
        this.action$.pipe(
            ofType(officeActions.remove),
            exhaustMap((action) =>
                this.officeService.delete(action.officeId)
                    .pipe(
                        map(() => officeActions.removeSuccess({ officeId: action.officeId })),
                        catchError((errorMsg: any) => of(officeActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private officeService: OfficeService) {

    }
}