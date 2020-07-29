import { UserDashboard } from './user-dashboard.model';

export interface Dashboard {
    id: number;
    name: string;
    userDashboards: UserDashboard[];
}
