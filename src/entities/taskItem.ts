import { Task } from './task';

export class TaskItem {
    id: number;
    text: string;
    checked: boolean;

    taskId: number;
    task: Task;
}
