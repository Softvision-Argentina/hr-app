import { User } from "./user";

export class Notification {
  id: number;
  text: string;
  applicationUserId: number;
  applicationUser: User;
  isRead: boolean;
  referredBy: string;
}
