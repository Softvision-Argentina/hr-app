import { Community } from "./community";
import { UserDashboard } from "./userDashboard";

export class User {

    constructor(public id: number, public username: string) { }

    firstName: string;
    lastName: string;
    imgURL: string;
    role: string;
    token: string;
    community: Community;
    userDashboards: UserDashboard[]
}
