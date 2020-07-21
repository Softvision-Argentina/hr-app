import { ReaddressReasonTypeModule } from './readdress-reason-type.module';

describe('DeclineReasonsModule', () => {
  let readdressReasonTypeModuleModule: ReaddressReasonTypeModule;

  beforeEach(() => {
    readdressReasonTypeModuleModule = new ReaddressReasonTypeModule();
  });

  it('should create an instance', () => {
    expect(readdressReasonTypeModuleModule).toBeTruthy();
  });
});
