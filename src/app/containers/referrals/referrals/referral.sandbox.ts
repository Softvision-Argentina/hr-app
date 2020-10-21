import { select, Store } from '@ngrx/store';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { State } from '@shared/store';
import { Injectable } from '@angular/core';
import { ReferralsState, selectReferrals, getReferralsState, getReferralsError } from '../store/referrals.reducers';
import { referralsActions } from '../store/referrals.actions';

@Injectable()
export class ReferralsSandbox extends Sandbox {
    

    referrals$ = this.referralsState$.pipe(
        select(getReferralsState)
    )

    referralsLoadingError$ = this.referralsState$.pipe(
        select(getReferralsError)
    );

    constructor(private appState$: Store<State>,private referralsState$: Store<ReferralsState>) {
        super(appState$);
    }

    public loadReferrals(): void {
        this.referralsState$.dispatch(referralsActions.load());
    }

    public remove (referralId: number){
        this.referralsState$.dispatch(referralsActions.remove({referralId}));
    }

    public add (newReferral: any,file: any){
        this.referralsState$.dispatch(referralsActions.add({referral: newReferral,file: file}));
    }

    public edit (newReferral: any,referralId: number,file: any){
        this.referralsState$.dispatch(referralsActions.edit({referral: newReferral,referralId: referralId,file: file}));
    }
}