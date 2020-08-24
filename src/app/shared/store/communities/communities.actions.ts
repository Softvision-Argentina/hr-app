import { createAction, props } from '@ngrx/store';
import { Community } from '@shared/models/community.model';

export const communitiesActions = {
    add: createAction('[Community] add', props<{ community: Community }>()),
    addSuccess: createAction('[Coimmunity] addSuccess', props<{ community: Community, communityId: number }>()),
    addFailed: createAction('[Commnuty] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[Community] load'),
    loadSuccess: createAction('[Community] loadSuccess', props<{ communities: Community[] }>()),
    loadFailed: createAction('[Community] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[Community] edit', props<{ community: Community }>()),
    editSuccess: createAction('[Community] editSuccess', props<{ community: Community }>()),
    editFailed: createAction('[Community] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[Community] remove', props<{ communityId: number }>()),
    removeSuccess: createAction('[Community] removeSuccess', props<{ communityId: number }>()),
    removeFailed: createAction('[Community] removeFailed', props<{ errorMsg: any }>()),
}