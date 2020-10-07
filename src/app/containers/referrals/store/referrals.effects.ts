import { Actions, createEffect, ofType } from '@ngrx/effects';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { ReferralsService } from '@shared/services/referrals.service';
import { referralsActions } from './referrals.actions';
import { Candidate } from '@shared/models/candidate.model';

@Injectable()
export class ReferralsEffects {
    loadReferrals$ = createEffect(() =>
        this.action$.pipe(
            ofType(referralsActions.load),
            switchMap(() =>
                this.referralsService.get()
                    .pipe(
                        map((referrals: Candidate[]) => referralsActions.loadSuccess({ referrals })),
                        catchError((errorMsg: any) => of(referralsActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addReferral$ = createEffect(() =>
        this.action$.pipe(
            ofType(referralsActions.add),
            exhaustMap(action =>
                this.referralsService.add(action.referral)
                    .pipe(
                        map((referral: { id: number }) => {

                            if (action.file) {
                                const file = new FormData();
                                file.append('file', action.file);
                                this.referralsService.saveCv(referral.id, file).subscribe()
                            }
                            this.referralsService.addNew({ ...action.referral.candidate, id: referral.id });

                            return referralsActions.addSuccess({ referral: action.referral, referralId: referral.id })

                        }),
                        catchError((errorMsg: any) => of(referralsActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editReferral$ = createEffect(() =>
        this.action$.pipe(
            ofType(referralsActions.edit),
            exhaustMap(action =>
                this.referralsService.update(action.referralId, action.referral.candidate)
                    .pipe(
                        map((referral: { id: number }) => {

                            if (action.file) {
                                const file = new FormData();
                                file.append('file', action.file);
                                this.referralsService.saveCv(referral.id, file).subscribe();
                            }

                            return referralsActions.editSuccess({ referral: action.referral });

                        }),
                        catchError((errorMsg: any) => of(referralsActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeReferral$ = createEffect(() =>
        this.action$.pipe(
            ofType(referralsActions.remove),
            exhaustMap((action) =>
                this.referralsService.delete(action.referralId)
                    .pipe(
                        map(() => referralsActions.removeSuccess({ referralId: action.referralId })),
                        catchError((errorMsg: any) => of(referralsActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private referralsService: ReferralsService) {

    }
}