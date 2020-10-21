import { createAction, props } from '@ngrx/store';
import { DeclineReason } from '@shared/models/decline-reason.model';

export const declineReasonActions = {
    add: createAction('[DeclineReason] add', props<{ reason: DeclineReason }>()),
    addSuccess: createAction('[DeclineReason] addSuccess', props<{ reason: DeclineReason }>()),
    addFailed: createAction('[DeclineReason] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[DeclineReason] load'),
    loadSuccess: createAction('[DeclineReason] loadSuccess', props<{ reasons: DeclineReason[] }>()),
    loadFailed: createAction('[DeclineReason] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[DeclineReason] edit', props<{ reason: DeclineReason }>()),
    editSuccess: createAction('[DeclineReason] editSuccess', props<{ reason: DeclineReason }>()),
    editFailed: createAction('[DeclineReason] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[DeclineReason] remove', props<{ reasonId: number }>()),
    removeSuccess: createAction('[DeclineReason] removeSuccess', props<{ reasonId: number }>()),
    removeFailed: createAction('[DeclineReason] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[DeclineReason] resetFailed'),
};
