import { createReducer, on } from '@ngrx/store';
import { Community } from '@shared/models/community.model';
import { communitiesActions } from './communities.actions';

export const key = 'communities';

export interface State {
    communities: Community[];
    errorMsg?: any;
    loading: boolean;
    failed: boolean;
}

export const initialState: State = {
    communities: [],
    errorMsg: null,
    loading: false,
    failed: false
}

export const reducer = createReducer(initialState,
    on(
        communitiesActions.add,
        communitiesActions.edit,
        (state, { community }) => ({
            ...state,
            loading: true,
            failed: false
        })
    ),

    on(
        communitiesActions.addSuccess,
        (state, { community, communityId }) => ({
            ...state,
            communities: [...state.communities, { community, id: communityId }],
            loading: false,
            failed: false
        })
    ),
    on(
        communitiesActions.loadSuccess,
        (state, { communities }) => ({
            ...state,
            communities,
            loading: false,
            failed: false
        })
    ),
    on(
        communitiesActions.editSuccess,
        (state, { community }) => ({
            ...state,
            communities: [...state.communities.filter((value) => value.id !== community.id), community],
            loading: false,
            failed: false
        })
    ),
    on(
        communitiesActions.removeSuccess,
        (state, { communityId }) => ({
            ...state,
            communities: state.communities.filter(c => c.id !== communityId),
            loading: false,
            failed: false
        })
    ),

    on(
        communitiesActions.loadFailed,
        communitiesActions.addFailed,
        communitiesActions.editFailed,
        communitiesActions.removeFailed,
        (state, { errorMsg }) => ({
            ...state,
            errorMsg,
            loading: false,
            failed: true
        })
    )
)

export const selectCommunities = (state: State) => state.communities;
export const selectCommunitiesErrorMsg = (state: State) => state.errorMsg;
export const selectCommunitiesLoading = (state: State) => state.loading;
export const selectCommunitiesFailed = (state: State) => state.failed;