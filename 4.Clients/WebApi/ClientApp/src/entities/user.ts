import { UserDashboard } from "./userDashboard";

export class User {
    id: number;
    name: string;
    imgURL: string;
    email: string;
    role: string;
    token: string;
    userDashboards: UserDashboard[]
}
