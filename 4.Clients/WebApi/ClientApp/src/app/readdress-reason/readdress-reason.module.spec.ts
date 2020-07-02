import { ReaddressReasonModule } from './readdress-reason.module';

describe('DeclineReasonsModule', () => {
  let declineReasonsModule: ReaddressReasonModule;

  beforeEach(() => {
    declineReasonsModule = new ReaddressReasonModule();
  });

  it('should create an instance', () => {
    expect(declineReasonsModule).toBeTruthy();
  });
});
