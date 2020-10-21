import { OfficeModule } from './office.module';

describe('OfficeModule', () => {
  let officeModule: OfficeModule;

  beforeEach(() => {
    officeModule = new OfficeModule();
  });

  it('should create an instance', () => {
    expect(officeModule).toBeTruthy();
  });
});
