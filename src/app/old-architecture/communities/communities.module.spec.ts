import { CommunitiesModule } from './communities.module';

describe('CommunitiesModule', () => {
  let communitiesModule: CommunitiesModule;

  beforeEach(() => {
    communitiesModule = new CommunitiesModule();
  });

  it('should create an instance', () => {
    expect(communitiesModule).toBeTruthy();
  });
});
