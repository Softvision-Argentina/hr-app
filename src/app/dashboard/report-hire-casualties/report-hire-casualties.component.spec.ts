import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportHireCasualtiesComponent } from './report-hire-casualties.component';

describe('ReportHireCasualtiesComponent', () => {
  let component: ReportHireCasualtiesComponent;
  let fixture: ComponentFixture<ReportHireCasualtiesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReportHireCasualtiesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReportHireCasualtiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
