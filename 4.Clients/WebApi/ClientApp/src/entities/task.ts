import { TaskItem } from './taskItem';
import { Consultant } from './consultant';

export class Task {
    id: number;
    title: string;
    isApprove: boolean;
    isNew: boolean;
    creationDate: Date;
    endDate: Date;

    consultantId: number;
    consultant: Consultant;

    taskItems: TaskItem[];
}
