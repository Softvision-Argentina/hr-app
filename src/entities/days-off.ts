import { Employee } from './employee';
import { DaysOffTypeEnum } from './enums/daysoff-type.enum';
import { DaysOffStatusEnum } from './enums/daysoff-status.enum';

export class DaysOff {
    id: number;
    date: Date; // RANGE PICKER
    endDate: Date; // RANGE PICKER
    type: DaysOffTypeEnum; // studyDays, Holydays, Training, PTO
    status: DaysOffStatusEnum; // in review, accepted
    employeeId: number;
    employee: Employee;
}
