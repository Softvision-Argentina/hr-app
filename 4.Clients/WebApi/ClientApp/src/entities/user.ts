import { UserDashboard } from "./userDashboard";

export class User {
    ID: number;
    Name: string;
    ImgURL: string;
    Email: string;
    Role: string;
    Token: string;
    userDashboards: UserDashboard[]
}
