import { createReducer, on, createFeatureSelector, createSelector } from '@ngrx/store';
import { tasksActions } from './tasks.actions';
import { Task } from '@shared/models/task.model';

export interface TasksState {
    tasks: Task[];
    loading: boolean;
    failed: boolean;
    openModal: boolean;
    onAddLoading: boolean;
}

export const initialState = {
    tasks: [],
    loading: false,
    failed: false,
    openModal: false,
    onAddLoading: false,
};

const selectAllTasks = createFeatureSelector<TasksState>('tasks');

export const getTasksState = createSelector(
    selectAllTasks,
    (state: TasksState) => state.tasks
);

export const getTasksLoading = createSelector(
    selectAllTasks,
    (state: TasksState) => state.loading
);

export const getTasksError = createSelector(
    selectAllTasks,
    (state: TasksState) => state.failed
);

export const getOpenModal = createSelector(
    selectAllTasks,
    (state: TasksState) => state.openModal
)

export const getTasksAddingLoading = createSelector(
    selectAllTasks,
    (state: TasksState) => state.onAddLoading
);

const _taskReducer = createReducer(initialState,
    on(tasksActions.add, state => ({
        ...state,
        loading: false,
        failed: false,
        onAddLoading: true
    })),
    on(tasksActions.addSuccess, (state, { task }) => {
        return {
            ...state,
            tasks: [...state.tasks, task],
            loading: false,
            failed: false,
            onAddLoading: false,
            openModal: false,
        }
    }),
    on(tasksActions.addFailed, state => ({
        ...state,
        loading: false,
        failed: true,
        onAddLoading: false,
    })),
    on(tasksActions.load, (state, { userRole, userEmail }) => ({
        ...state,
        loading: true,
        failed: false
    })),
    on(tasksActions.loadSuccess, (state, { tasks }) => ({
        ...state,
        tasks,
        loading: false,
        failed: false,
    })),
    on(tasksActions.loadFailed, state => ({
        ...state,
        loading: false,
        failed: true
    })),
    on(tasksActions.remove, (state, { taskId }) => ({
        ...state,
        loading: true,
        failed: false
    })),
    on(tasksActions.onRemoveSuccess, (state, { taskId }) => ({
        ...state,
        tasks: state.tasks.filter(task => task.id !== taskId),
        loading: false,
        failed: false,
    })),
    on(tasksActions.onRemoveFailed, state => ({
        ...state,
        loading: false,
        failed: true
    })),
    on(tasksActions.toogleModal, state => ({
        ...state,
        openModal: !state.openModal
    })),
);

export function taskReducer(state, action) {
    return _taskReducer(state, action);
}

