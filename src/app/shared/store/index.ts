import { ActionReducerMap, createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromCommunities from './communities/communities.reducer';

export interface State {
    [fromCommunities.key]: fromCommunities.State
}

export const reducers: ActionReducerMap<State> = {
    [fromCommunities.key]: fromCommunities.reducer
}

//Communities selector
export const selectCommunitiesState = createFeatureSelector<State, fromCommunities.State>(fromCommunities.key);

export const selectCommunities = createSelector(
    selectCommunitiesState,
    fromCommunities.selectCommunities
);
export const selectCommunitiesErrorMsg = createSelector(
    selectCommunitiesState,
    fromCommunities.selectCommunitiesErrorMsg
);
export const selectCommunitiesLoading = createSelector(
    selectCommunitiesState,
    fromCommunities.selectCommunitiesLoading
);
export const selectCommunitiesFailed = createSelector(
    selectCommunitiesState,
    fromCommunities.selectCommunitiesFailed
);