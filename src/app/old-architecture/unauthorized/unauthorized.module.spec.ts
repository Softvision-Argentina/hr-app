import { UnauthorizedModule } from './unauthorized.module';

describe('UnauthorizedModule', () => {
  let unauthorizedModule: UnauthorizedModule;

  beforeEach(() => {
    unauthorizedModule = new UnauthorizedModule();
  });

  it('should create an instance', () => {
    expect(unauthorizedModule).toBeTruthy();
  });
});
