import { User } from './user.model';
import { Dashboard } from './dashboard.model';

export class UserDashboard {
    userId: number;
    user: User;
    dashboardId: number;
    dashboard: Dashboard;
}
