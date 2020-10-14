import { createReducer, on, createFeatureSelector, createSelector } from '@ngrx/store';
import { processActions } from './process.action';
import { ProcessStatusEnum } from '@shared/enums/process-status.enum';
import { ProcessTableView } from '@shared/models/process-tableView.model';
import { CallState } from '@shared/models/callState.model';
import { LoadingState } from '@shared/enums/loadingState.enum';
import { ErrorState } from '@shared/models/errorState.model';
export interface ProcessState {
    processesTableView: ProcessTableView[];
    deletedProcessesTableView: ProcessTableView[];
    callState: CallState;
    currentProcessId: number;
}

export const initialState = {
    processesTableView: [],
    deletedProcessesTableView: [],
    callState: null,
    currentProcessId: null
};

export const getProcessesState = createFeatureSelector<ProcessState>('processes');

export const getProcesses = createSelector(
    getProcessesState,
    (state: ProcessState) => state.processesTableView
);

export const getDeletedProcesses = createSelector(
    getProcessesState,
    (state: ProcessState) => state.deletedProcessesTableView
);

export const getCurrentProcessId = createSelector(
    getProcessesState,
    (state: ProcessState) => state.currentProcessId
)


export const isLoading = createSelector(
    getProcessesState,
    (state: ProcessState) => state.callState === LoadingState.LOADING
);

export const isLoaded = createSelector(
    getProcessesState,
    (state: ProcessState) => state.callState === LoadingState.LOADED
);

export const getErrorState = createSelector(
    getProcessesState,
    (state: ProcessState) => getError(state.callState)
);


const _processReducer = createReducer(initialState,
    on(processActions.add, state => {
        return {
            ...state,
            callState: LoadingState.LOADING
        }
    }),

    on(processActions.onAddSuccess, (state, { processTableView }) => {
        const sortedProcesses = [...state.processesTableView, processTableView].sort((a, b) => b.id - a.id);

        return {
            ...state,
            processesTableView: sortedProcesses,
            currentProcessId: processTableView.id,
            callState: LoadingState.LOADED
        };
    }),

    on(processActions.onAddFailed, (state, { errorMsg }) => {
        return {
            ...state,
            callState: { errorMsg }
        };
    }),

    on(processActions.update, state => {
        return {
            ...state,
            callState: LoadingState.LOADING
        }
    }),

    on(processActions.onUpdateSuccess, (state, { process }) => {
        const sortedProcesses = [...state.processesTableView.filter(p => p.id !== process.id), process].sort((a, b) => b.id - a.id);

        return {
            ...state,
            processesTableView: sortedProcesses,
            currentProcessId: process.id,
            callState: LoadingState.LOADED
        }
    }),

    on(processActions.onUpdateFailed, (state, { errorMsg }) => {
        return {
            ...state,
            callState: { errorMsg }
        }
    }),

    on(processActions.load, state => ({
        ...state,
        callState: LoadingState.INIT
    })),

    on(processActions.onLoadSuccess, (state, { processesTableView }) => ({
        ...state,
        processesTableView,
        callState: LoadingState.LOADED
    })),

    on(processActions.onLoadFailed, (state, { errorMsg }) => ({
        ...state,
        callState: { errorMsg }
    })),

    on(processActions.loadDeleted, state => ({
        ...state,
        callState: LoadingState.INIT
    })),

    on(processActions.onLoadDeletedSuccess, (state, { deletedProcessesTableView }) => ({
        ...state,
        deletedProcessesTableView,
        callState: LoadingState.LOADED
    })),

    on(processActions.onLoadDeletedFailed, (state, { errorMsg }) => ({
        ...state,
        callState: { errorMsg }
    })),

    on(processActions.remove, state => ({
        ...state,
        callState: LoadingState.LOADING
    })),

    on(processActions.onRemoveSuccess, (state, { processId }) => {
        const deletedProcess = { ...state.processesTableView.find(process => process.id === processId) };
        deletedProcess.status = ProcessStatusEnum.Eliminated;
        return ({
            ...state,
            processesTableView: state.processesTableView.filter(process => process.id !== processId),
            deletedProcessesTableView: [...state.deletedProcessesTableView, deletedProcess],
            callState: LoadingState.LOADED
        })
    }),
    on(processActions.onRemoveFailed, (state, { errorMsg }) => {
        return {
            ...state,
            callState: { errorMsg }
        };
    }),

    on(processActions.reactivate, state => ({
        ...state,
        callState: LoadingState.LOADING
    })),

    on(processActions.onReactivateSuccess, (state, { processId, reactivatedStatus }) => {
        const ractivatedProcess = { ...state.deletedProcessesTableView.find(process => process.id === processId) };
        ractivatedProcess.status = reactivatedStatus;

        return ({
            ...state,
            processesTableView: [...state.processesTableView, ractivatedProcess],
            deletedProcessesTableView: state.deletedProcessesTableView.filter(process => process.id !== processId),
            callState: LoadingState.LOADED
        })
    }),
    on(processActions.onReactivateFailed, (state, { errorMsg }) => {
        return {
            ...state,
            callState: { errorMsg }
        };
    }),




    on(processActions.approve, state => ({
        ...state,
        callState: LoadingState.LOADING
    })),
    on(processActions.onApproveSuccess, (state, { approvedProcess }) => ({
        ...state,
        processesTableView: [...state.processesTableView.filter(process => process.id !== approvedProcess.id), approvedProcess],
        callState: LoadingState.LOADED
    })),
    on(processActions.onApproveFailed, (state, { errorMsg }) => ({
        ...state,
        callState: { errorMsg }
    })),
    on(processActions.reject, state => ({
        ...state,
        callState: LoadingState.LOADING
    })),
    on(processActions.onRejectSuccess, (state, { processId }) => {
        const rejectedProcess = { ...state.processesTableView.find(process => process.id === processId) };
        rejectedProcess.status = ProcessStatusEnum.Rejected;
        return {
            ...state,
            processesTableView: [...state.processesTableView.filter(process => process.id !== processId), rejectedProcess],
            callState: LoadingState.LOADED
        };
    }),
    on(processActions.onRejectFailed, (state, { errorMsg }) => ({
        ...state,
        callState: { errorMsg }
    })),

);

export function processReducer(state, action) {
    return _processReducer(state, action);
}

export function getError(callState: CallState): string | null {
    if ((callState as ErrorState).errorMsg !== undefined) {
        return (callState as ErrorState).errorMsg;
    }
    return null;
}
