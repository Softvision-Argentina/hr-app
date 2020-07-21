import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeCasualtiesComponent } from './employee-casualties.component';

describe('EmployeeCasualtiesComponent', () => {
  let component: EmployeeCasualtiesComponent;
  let fixture: ComponentFixture<EmployeeCasualtiesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeCasualtiesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeCasualtiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
