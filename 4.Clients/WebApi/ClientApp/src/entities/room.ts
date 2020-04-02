import { Reservation } from './reservation';
import { Office } from './office';


export class Room {
  id: number;
  name: string;
  description: string;
  officeId: number;
  office: Office;
  reservationItems: Reservation;
}
