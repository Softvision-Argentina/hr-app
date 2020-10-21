import { Injectable } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { State, selectDeclineReasonFailed, selectDeclineReasonLoading, selectDeclineReasonErrorMsg, selectDeclineReasons } from './store/index';
import { declineReasonActions } from './store/decline-reasons.actions';
import { DeclineReason } from '@shared/models/decline-reason.model';
@Injectable()

export class DeclineReasonSandbox {

    declineReasons$ = this.declineReasonState$.pipe(
        select(selectDeclineReasons)
    );

    declineReasonLoading$ = this.declineReasonState$.pipe(
        select(selectDeclineReasonLoading)
    );

    declineReasonFailed$ = this.declineReasonState$.pipe(
        select(selectDeclineReasonFailed)
    );

    declineReasonErrorMsg$ = this.declineReasonState$.pipe(
        select(selectDeclineReasonErrorMsg)
    );

    constructor(private declineReasonState$: Store<State>) {
    }

    loadDeclineReasons(): void {
        this.declineReasonState$.dispatch(declineReasonActions.load());
    }

    addDeclineReason(reason: DeclineReason) {
        this.declineReasonState$.dispatch(declineReasonActions.add({ reason }));
    }

    removeDeclineReason(reasonId: number) {
        this.declineReasonState$.dispatch(declineReasonActions.remove({ reasonId }));
    }

    editDeclineReason(reason: DeclineReason) {
        this.declineReasonState$.dispatch(declineReasonActions.edit({ reason }));
    }

    resetFailed() {
        this.declineReasonState$.dispatch(declineReasonActions.resetFailed());
    }
}
