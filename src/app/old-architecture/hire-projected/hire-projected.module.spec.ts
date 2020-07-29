import { HireProjectedModule } from './hire-projected.module';

describe('HireProjectedModule', () => {
  let hireProjectedModule: HireProjectedModule;

  beforeEach(() => {
    hireProjectedModule = new HireProjectedModule();
  });

  it('should create an instance', () => {
    expect(hireProjectedModule).toBeTruthy();
  });
});
