import { Actions, createEffect, ofType } from '@ngrx/effects';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
import { CandidateProfileService } from '@shared/services/candidate-profile.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { candidateProfilesActions } from './candidates-profile.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class CandidateProfileEffects {
    loadCandidateProfiles$ = createEffect(() =>
        this.action$.pipe(
            ofType(candidateProfilesActions.load),
            switchMap(() =>
                this.candidateProfileService.get()
                    .pipe(
                        map((candidateProfiles: CandidateProfile[]) => candidateProfilesActions.loadSuccess({ candidateProfiles })
                        ),
                        catchError((errorMsg: any) => of(candidateProfilesActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addCandidateProfile$ = createEffect(() =>
        this.action$.pipe(
            ofType(candidateProfilesActions.add),
            exhaustMap(action =>
                this.candidateProfileService.add(action.candidateProfile)
                    .pipe(
                        map((candidateProfile: { id: number }) => candidateProfilesActions.addSuccess({ ...action.candidateProfile, id: candidateProfile.id })),
                        catchError((errorMsg: any) => of(candidateProfilesActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editCandidateProfile$ = createEffect(() =>
        this.action$.pipe(
            ofType(candidateProfilesActions.edit),
            exhaustMap(action =>
                this.candidateProfileService.update(action.candidateProfile.id, action.candidateProfile)
                    .pipe(
                        map(() => candidateProfilesActions.editSuccess({ candidateProfile: action.candidateProfile })),
                        catchError((errorMsg: any) => of(candidateProfilesActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeCandidateProfile$ = createEffect(() =>
        this.action$.pipe(
            ofType(candidateProfilesActions.remove),
            exhaustMap((action) =>
                this.candidateProfileService.delete(action.candidateProfileId)
                    .pipe(
                        map(() => candidateProfilesActions.removeSuccess({ candidateProfileId: action.candidateProfileId })),
                        catchError((errorMsg: any) => of(candidateProfilesActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private candidateProfileService: CandidateProfileService) {

    }
}