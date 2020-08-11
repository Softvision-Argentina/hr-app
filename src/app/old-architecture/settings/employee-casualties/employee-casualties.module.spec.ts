import { EmployeeCasualtiesModule } from './employee-casualties.module';

describe('EmployeeCasualtiesModule', () => {
  let employeeCasualtiesModule: EmployeeCasualtiesModule;

  beforeEach(() => {
    employeeCasualtiesModule = new EmployeeCasualtiesModule();
  });

  it('should create an instance', () => {
    expect(employeeCasualtiesModule).toBeTruthy();
  });
});
