import { DeclineReasonsModule } from './decline-reasons.module';

describe('DeclineReasonsModule', () => {
  let declineReasonsModule: DeclineReasonsModule;

  beforeEach(() => {
    declineReasonsModule = new DeclineReasonsModule();
  });

  it('should create an instance', () => {
    expect(declineReasonsModule).toBeTruthy();
  });
});
