import { Injectable } from '@angular/core';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { State, selectCandidateProfilesFailed, selectCandidateProfilesLoading, selectCandidateProfilesErrorMsg } from '@shared/store/index';
import { candidateProfilesActions } from '@shared/store/candidates-profile/candidates-profile.actions';
import { CandidateProfile } from '@shared/models/candidate-profile.model';
@Injectable()

export class CandidateProfilesSandbox extends Sandbox {

    candidateProfilesLoading$ = this.appState$.pipe(
        select(selectCandidateProfilesLoading)
    );

    candidateProfilesFailed$ = this.appState$.pipe(
        select(selectCandidateProfilesFailed)
    );

    candidateProfilesErrorMsg$ = this.appState$.pipe(
        select(selectCandidateProfilesErrorMsg)
    );

    constructor(private appState$: Store<State>) {
        super(appState$);
    }

    addCandidateProfile(newCandidateProfile: CandidateProfile) {
        this.appState$.dispatch(candidateProfilesActions.add({ candidateProfile: newCandidateProfile }));
    }

    removeCandidateProfile(candidateProfileId: number) {
        this.appState$.dispatch(candidateProfilesActions.remove({ candidateProfileId }));
    }

    editCandidateProfile(candidateProfile: CandidateProfile) {
        this.appState$.dispatch(candidateProfilesActions.edit({ candidateProfile: candidateProfile }));
    }

    resetFailed() {
        this.appState$.dispatch(candidateProfilesActions.resetFailed());
    }
}
