import { ProfilesModule } from './profiles.module';

describe('ProfilesModule', () => {
  let profilesModule: ProfilesModule;

  beforeEach(() => {
    profilesModule = new ProfilesModule();
  });

  it('should create an instance', () => {
    expect(profilesModule).toBeTruthy();
  });
});
