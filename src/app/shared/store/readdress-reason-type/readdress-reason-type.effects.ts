import { Actions, createEffect, ofType } from '@ngrx/effects';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';
import { ReaddressReasonTypeService } from '@shared/services/readdress-reason-type.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { readdressReasonTypesActions } from './readdress-reason-type.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class ReaddressReasonTypesEffects {
    loadReaddressReasonTypes$ = createEffect(() =>
        this.action$.pipe(
            ofType(readdressReasonTypesActions.load),
            switchMap(() =>
                this.readdressReasonTypeService.get()
                    .pipe(
                        map((readdressReasonTypes: ReaddressReasonType[]) => readdressReasonTypesActions.loadSuccess({ readdressReasonTypes })),
                        catchError((errorMsg: any) => of(readdressReasonTypesActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addReaddressReasonType$ = createEffect(() =>
        this.action$.pipe(
            ofType(readdressReasonTypesActions.add),
            exhaustMap(action =>
                this.readdressReasonTypeService.add(action.readdressReasonType)
                    .pipe(
                        map((readdressReasonType: { id: number }) => readdressReasonTypesActions.addSuccess({ ...action.readdressReasonType, id: readdressReasonType.id })),
                        catchError((errorMsg: any) => of(readdressReasonTypesActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editReaddressReasonType$ = createEffect(() =>
        this.action$.pipe(
            ofType(readdressReasonTypesActions.edit),
            exhaustMap(action =>
                this.readdressReasonTypeService.update(action.readdressReasonType.id, action.readdressReasonType)
                    .pipe(
                        map(() => readdressReasonTypesActions.editSuccess({ readdressReasonType: action.readdressReasonType })),
                        catchError((errorMsg: any) => of(readdressReasonTypesActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeReaddressReasonType$ = createEffect(() =>
        this.action$.pipe(
            ofType(readdressReasonTypesActions.remove),
            exhaustMap((action) =>
                this.readdressReasonTypeService.delete(action.readdressReasonTypeId)
                    .pipe(
                        map(() => readdressReasonTypesActions.removeSuccess({ readdressReasonTypeId: action.readdressReasonTypeId })),
                        catchError((errorMsg: any) => of(readdressReasonTypesActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private readdressReasonTypeService: ReaddressReasonTypeService) { }
}
