import { createAction, props } from '@ngrx/store';
import { Process } from '@shared/models/process.model';
import { ProcessTableView } from '@shared/models/process-tableView.model';

export const processActions = {
    add: createAction('[ProcessComponent] add', props<{process: Process}>()),
    onAddSuccess: createAction('[ProcessComponent] addSuccess', props<{processTableView: ProcessTableView}>()),
    onAddFailed: createAction('[ProcessComponent] addFailed', props<{errorMsg: string}>()),

    update: createAction('[ProcessComponent] update', props<{process: Process}>()),
    onUpdateSuccess: createAction('[ProcessComponent] onUpdateSuccess', props<{process: ProcessTableView}>()),
    onUpdateFailed: createAction('[ProcessComponent] onUpdateFailed', props<{errorMsg: string}>()),

    load: createAction('[ProcessComponent] load'),
    onLoadSuccess: createAction('[ProcessComponent] onLoadSuccess', props<{processesTableView: ProcessTableView[]}>()),
    onLoadFailed: createAction('[ProcessComponent] onLoadFailed', props<{ errorMsg: string }>()),
    
    loadDeleted: createAction('[ProcessComponent] loadDeleted'),
    onLoadDeletedSuccess: createAction('[ProcessComponent] onLoadDeletedSuccess', props<{deletedProcessesTableView: ProcessTableView[]}>()),
    onLoadDeletedFailed: createAction('[ProcessComponent] onLoadDeletedFailed', props<{errorMsg: string}>()),

    remove: createAction('[ProcessComponent] remove', props<{processId: number}>()),
    onRemoveSuccess: createAction('[ProcessComponent] onRemoveSuccess', props<{processId: number}>()),
    onRemoveFailed: createAction('[ProcessComponent] onRemoveFailed', props<{ errorMsg: string }>()),
    
    reactivate: createAction('[ProcessComponent] reactivate', props<{processId: number}>()),
    onReactivateSuccess: createAction('[ProcessComponent] onReactivateSuccess', props<{processId: number, reactivatedStatus: number}>()),
    onReactivateFailed: createAction('[ProcessComponent] onReactivateFailed', props<{errorMsg: string}>()),

    approve: createAction('[ProcessComponent] approve', props<{processId: number}>()),
    onApproveSuccess: createAction('[ProcessComponent] onApproveSuccess', props<{approvedProcess: ProcessTableView}>()),
    onApproveFailed: createAction('[ProcessComponent] onApproveFailed', props<{errorMsg: string}>()),

    reject: createAction('[ProcessComponent] reject', props<{processId: number, rejectionReason: string }>()),
    onRejectSuccess: createAction('[ProcessComponent] onRejectSuccess', props<{processId: number}>()),
    onRejectFailed: createAction('[ProcessComponent], onRejectFailed', props<{errorMsg: string}>()),

};
