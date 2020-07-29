import { User } from './user.model';

export interface Notification {
  id: number;
  text: string;
  applicationUserId: number;
  applicationUser: User;
  isRead: boolean;
  referredBy: string;
}
