import { CandidatesProfileModule } from './candidates-profile.module';

describe('CandidatesProfileModule', () => {
  let candidatesProfileModule: CandidatesProfileModule;

  beforeEach(() => {
    candidatesProfileModule = new CandidatesProfileModule();
  });

  it('should create an instance', () => {
    expect(candidatesProfileModule).toBeTruthy();
  });
});
