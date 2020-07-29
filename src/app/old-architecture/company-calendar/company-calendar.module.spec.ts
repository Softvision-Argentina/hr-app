import { CompanyCalendarModule } from './company-calendar.module';

describe('CompanyCalendarModule', () => {
  let companyCalendarModule: CompanyCalendarModule;

  beforeEach(() => {
    companyCalendarModule = new CompanyCalendarModule();
  });

  it('should create an instance', () => {
    expect(companyCalendarModule).toBeTruthy();
  });
});
