import { RoomModule } from './room.module';

describe('RoomModule', () => {
  let roomModule: RoomModule;

  beforeEach(() => {
    roomModule = new RoomModule();
  });

  it('should create an instance', () => {
    expect(roomModule).toBeTruthy();
  });
});
