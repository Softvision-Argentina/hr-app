import { Reservation } from './reservation.model';
import { Office } from './office.model';


export interface Room {
  id: number;
  name: string;
  description: string;
  officeId: number;
  office: Office;
  reservationItems: Reservation;
}
