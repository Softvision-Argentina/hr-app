import { createAction, props } from '@ngrx/store';
import { ReaddressReasonType } from '@shared/models/readdress-reason-type.model';

export const readdressReasonTypesActions = {
    add: createAction('[ReaddressReasonType] add', props<{ readdressReasonType: ReaddressReasonType }>()),
    addSuccess: createAction('[ReaddressReasonType] addSuccess', props<ReaddressReasonType>()),
    addFailed: createAction('[ReaddressReasonType] addFailed', props<{ errorMsg: any }>()),

    load: createAction('[ReaddressReasonType] load'),
    loadSuccess: createAction('[ReaddressReasonType] loadSuccess', props<{ readdressReasonTypes: ReaddressReasonType[] }>()),
    loadFailed: createAction('[ReaddressReasonType] loadFailed', props<{ errorMsg: any }>()),

    edit: createAction('[ReaddressReasonType] edit', props<{ readdressReasonType: ReaddressReasonType }>()),
    editSuccess: createAction('[ReaddressReasonType] editSuccess', props<{ readdressReasonType: ReaddressReasonType }>()),
    editFailed: createAction('[ReaddressReasonType] editFailed', props<{ errorMsg: any }>()),

    remove: createAction('[ReaddressReasonType] remove', props<{ readdressReasonTypeId: number }>()),
    removeSuccess: createAction('[ReaddressReasonType] removeSuccess', props<{ readdressReasonTypeId: number }>()),
    removeFailed: createAction('[ReaddressReasonType] removeFailed', props<{ errorMsg: any }>()),

    resetFailed: createAction('[ReaddressReasonType] resetFailed'),
};
