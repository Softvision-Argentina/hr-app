import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Community } from '@shared/models/community.model';
import { CommunityService } from '@shared/services/community.service';
import { of } from 'rxjs';
import { catchError, exhaustMap, map, switchMap } from 'rxjs/operators';
import { communitiesActions } from './communities.actions';
import { Injectable } from '@angular/core';

@Injectable()
export class CommunitiesEffects {
    loadCommunities$ = createEffect(() =>
        this.action$.pipe(
            ofType(communitiesActions.load),
            switchMap(() =>
                this.communityService.getCommunities()
                    .pipe(
                        map((communities: Community[]) => communitiesActions.loadSuccess({ communities })),
                        catchError((errorMsg: any) => of(communitiesActions.loadFailed({ errorMsg })))
                    )
            )
        )
    );

    addCommunity$ = createEffect(() =>
        this.action$.pipe(
            ofType(communitiesActions.add),
            exhaustMap(action =>
                this.communityService.add(action.community)
                    .pipe(
                        map((community: { id: number }) => communitiesActions.addSuccess({ ...action.community, id: community.id })),
                        catchError((errorMsg: any) => of(communitiesActions.addFailed({ errorMsg })))
                    )
            )
        )
    );

    editCommunity$ = createEffect(() =>
        this.action$.pipe(
            ofType(communitiesActions.edit),
            exhaustMap(action =>
                this.communityService.update(action.community.id, action.community)
                    .pipe(
                        map(() => communitiesActions.editSuccess({ community: action.community })),
                        catchError((errorMsg: any) => of(communitiesActions.editFailed({ errorMsg })))
                    )
            )
        )
    );

    removeCommunity$ = createEffect(() =>
        this.action$.pipe(
            ofType(communitiesActions.remove),
            exhaustMap((action) =>
                this.communityService.delete(action.communityId)
                    .pipe(
                        map(() => communitiesActions.removeSuccess({ communityId: action.communityId })),
                        catchError((errorMsg: any) => of(communitiesActions.removeFailed({ errorMsg })))
                    )
            )
        )
    );

    constructor(private action$: Actions, private communityService: CommunityService) {

    }
}
