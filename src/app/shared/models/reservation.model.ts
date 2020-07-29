import { Room } from './room.model';

export interface Reservation {
  id: number;
  description: string;
  sinceReservation: Date;
  untilReservation: Date;
  user: number;
  roomId: number;
  room: Room;
}
