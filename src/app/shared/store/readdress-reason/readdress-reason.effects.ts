import { Actions, createEffect, ofType } from '@ngrx/effects';
import { ReaddressReason } from '@shared/models/readdress-reason.model';
import { ReaddressReasonService } from '@shared/services/readdress-reason.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { readdressReasonActions } from './readdress-reason.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class ReaddressReasonEffects {
    loadReaddressReasons$ = createEffect(() =>
        this.action$.pipe(
            ofType(readdressReasonActions.load),
            switchMap(() =>
                this.readdressReasonService.get()
                    .pipe(
                        map((readdressReasons: ReaddressReason[]) => readdressReasonActions.loadSuccess({ readdressReasons })),
                        catchError((errorMsg: any) => of(readdressReasonActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addReaddressReason$ = createEffect(() =>
        this.action$.pipe(
            ofType(readdressReasonActions.add),
            exhaustMap(action =>
                this.readdressReasonService.add(action.readdressReason)
                    .pipe(
                        map((readdressReasonId: { id: number }) => {
                            const readdressReason = { ...action.readdressReason, id: readdressReasonId.id }
                            return readdressReasonActions.addSuccess({ readdressReason: readdressReason });
                        }),
                        catchError((errorMsg: any) => of(readdressReasonActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editReaddressReason$ = createEffect(() =>
        this.action$.pipe(
            ofType(readdressReasonActions.edit),
            exhaustMap(action =>
                this.readdressReasonService.update(action.readdressReason.id, action.readdressReason)
                    .pipe(
                        map(() => readdressReasonActions.editSuccess({ readdressReason: action.readdressReason })),
                        catchError((errorMsg: any) => of(readdressReasonActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeReaddressReason$ = createEffect(() =>
        this.action$.pipe(
            ofType(readdressReasonActions.remove),
            exhaustMap((action) =>
                this.readdressReasonService.delete(action.readdressReasonId)
                    .pipe(
                        map(() => readdressReasonActions.removeSuccess({ readdressReasonId: action.readdressReasonId })),
                        catchError((errorMsg: any) => of(readdressReasonActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private readdressReasonService: ReaddressReasonService) {

    }
}
