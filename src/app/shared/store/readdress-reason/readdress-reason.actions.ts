import { createAction, props } from '@ngrx/store';
import { ReaddressReason } from '@shared/models/readdress-reason.model';

export const readdressReasonActions = {
    add: createAction('[ReaddressReason] add', props<{ readdressReason: ReaddressReason }>()),
    addSuccess: createAction('[ReaddressReason] addSuccess', props<{ readdressReason: ReaddressReason }>()),
    addFailed: createAction('[ReaddressReason] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[ReaddressReason] load'),
    loadSuccess: createAction('[ReaddressReason] loadSuccess', props<{ readdressReasons: ReaddressReason[] }>()),
    loadFailed: createAction('[ReaddressReason] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[ReaddressReason] edit', props<{ readdressReason: ReaddressReason }>()),
    editSuccess: createAction('[ReaddressReason] editSuccess', props<{ readdressReason: ReaddressReason }>()),
    editFailed: createAction('[ReaddressReason] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[ReaddressReason] remove', props<{ readdressReasonId: number }>()),
    removeSuccess: createAction('[ReaddressReason] removeSuccess', props<{ readdressReasonId: number }>()),
    removeFailed: createAction('[ReaddressReason] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[ReaddressReason] resetFailed'),
};
