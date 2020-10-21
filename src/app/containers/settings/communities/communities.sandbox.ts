import { Injectable } from '@angular/core';
import { Sandbox } from '@shared/sandbox/base.sandbox';
import { Store, select } from '@ngrx/store';
import { State, selectCommunitiesFailed, selectCommunitiesLoading, selectCommunitiesErrorMsg } from '@shared/store/index';
import { communitiesActions } from '@shared/store/communities/communities.actions';
import { Community } from '@shared/models/community.model';
@Injectable()

export class CommunitiesSandbox extends Sandbox {

    communitiesLoading$ = this.appState$.pipe(
        select(selectCommunitiesLoading)
    );

    communtiesFailed$ = this.appState$.pipe(
        select(selectCommunitiesFailed)
    );

    communtiesErrorMsg$ = this.appState$.pipe(
        select(selectCommunitiesErrorMsg)
    );

    constructor(private appState$: Store<State>) {
        super(appState$);
    }

    addCommunity(newCommunity: Community) {
        this.appState$.dispatch(communitiesActions.add({ community: newCommunity }));
    }

    removeCommunity(communityId: number) {
        this.appState$.dispatch(communitiesActions.remove({ communityId }));
    }

    editCommunity(community: Community) {
        this.appState$.dispatch(communitiesActions.edit({ community: community }));
    }

    resetFailed() {
        this.appState$.dispatch(communitiesActions.resetFailed());
    }
}
