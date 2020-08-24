import { select, Store } from '@ngrx/store';
import { selectCommunities, State } from '../store';
import { communitiesActions } from '../store/communities/communities.actions';


export abstract class Sandbox {
    communities$ = this.state$.pipe(
        select(selectCommunities)
    );

    constructor(
        private state$: Store<State>
    ) { }

    public loadCommunities(): void {
        this.state$.dispatch(communitiesActions.load());
    }
}