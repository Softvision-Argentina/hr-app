  import { Room } from './room.model';

export interface Office {
  id: number;
  name: string;
  description: string;
  roomItems: Room[];
}
