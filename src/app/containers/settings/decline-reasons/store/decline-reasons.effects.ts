import { Actions, createEffect, ofType } from '@ngrx/effects';
import { DeclineReason } from '@shared/models/decline-reason.model';
import { DeclineReasonService } from '@shared/services/decline-reason.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { declineReasonActions } from './decline-reasons.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class DeclineReasonEffects {
    loadDeclineReasons$ = createEffect(() =>
        this.action$.pipe(
            ofType(declineReasonActions.load),
            switchMap(() =>
                this.declineReasonService.get('named')
                    .pipe(
                        map((reasons: DeclineReason[]) => declineReasonActions.loadSuccess({ reasons })),
                        catchError((errorMsg: any) => of(declineReasonActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addDeclineReason$ = createEffect(() =>
        this.action$.pipe(
            ofType(declineReasonActions.add),
            exhaustMap(action =>
                this.declineReasonService.add(action.reason)
                    .pipe(
                        map((newReason: { id: number }) => {
                            const reasons = { ...action.reason, id: newReason.id };
                            return declineReasonActions.addSuccess({ reason: reasons });
                        }),
                        catchError((errorMsg: any) => of(declineReasonActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editDeclineReason$ = createEffect(() =>
        this.action$.pipe(
            ofType(declineReasonActions.edit),
            exhaustMap(action =>
                this.declineReasonService.update(action.reason.id, action.reason)
                    .pipe(
                        map(() => declineReasonActions.editSuccess({ reason: action.reason })),
                        catchError((errorMsg: any) => of(declineReasonActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeDeclineReason$ = createEffect(() =>
        this.action$.pipe(
            ofType(declineReasonActions.remove),
            exhaustMap((action) =>
                this.declineReasonService.delete(action.reasonId)
                    .pipe(
                        map(() => declineReasonActions.removeSuccess({ reasonId: action.reasonId })),
                        catchError((errorMsg: any) => of(declineReasonActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private declineReasonService: DeclineReasonService) {

    }
}
