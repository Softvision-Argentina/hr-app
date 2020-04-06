import { Room } from './room';

export class Reservation {
    id: number;
    description: string;
    sinceReservation: Date;
    untilReservation: Date;
    recruiter: number;
    roomId: number;
    room: Room;
  }
