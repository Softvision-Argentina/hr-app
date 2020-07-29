import { TaskItem } from './task-item.model';
import { User } from './user.model';

export interface Task {
    id: number;
    title: string;
    isApprove: boolean;
    isNew: boolean;
    creationDate: Date;
    endDate: Date;
    userId: number;
    user: User;
    taskItems: TaskItem[];
}
