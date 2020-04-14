import { TaskItem } from './taskItem';
import { User } from './user';

export class Task {
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
