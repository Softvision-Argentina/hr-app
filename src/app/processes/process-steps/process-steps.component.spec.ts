import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessStepsComponent } from './process-steps.component';

describe('ProcessStepsComponent', () => {
  let component: ProcessStepsComponent;
  let fixture: ComponentFixture<ProcessStepsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessStepsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessStepsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
