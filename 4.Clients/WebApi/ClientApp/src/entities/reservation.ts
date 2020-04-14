import { Room } from './room';

export class Reservation {
    id: number;
    description: string;
    sinceReservation: Date;
    untilReservation: Date;
    user: number;
    roomId: number;
    room: Room;
  }
