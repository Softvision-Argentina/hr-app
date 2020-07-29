import { Task } from './task.model';

export interface TaskItem {
    id: number;
    text: string;
    checked: boolean;

    taskId: number;
    task: Task;
}
