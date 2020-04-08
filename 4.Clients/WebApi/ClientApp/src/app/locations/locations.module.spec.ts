import { LocationsModule } from './locations.module';

describe('LocationsModule', () => {
  let locationsModule: LocationsModule;

  beforeEach(() => {
    locationsModule = new LocationsModule();
  });

  it('should create an instance', () => {
    expect(locationsModule).toBeTruthy();
  });
});
