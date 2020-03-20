import { Community } from "./community";

export class User {
    ID: number;
    Name: string;
    ImgURL: string;
    Email: string;
    Role: string;
    Token: string;
    Community: Community;
}
