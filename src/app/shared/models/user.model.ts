import { Community } from "./community.model";
import { UserDashboard } from "./user-dashboard.model";

export class User {
    constructor(id?: number, username?: string) {
        this.id = id;
        this.username = username;
    }
    id: number;
    username: string;
    firstName: string;
    lastName: string;
    imgURL: string;
    role: string;
    token: string;
    community: Community;
    userDashboards: UserDashboard[]
}
