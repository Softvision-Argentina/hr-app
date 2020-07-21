import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeclineReasonComponent } from './decline-reasons.component';

describe('DeclineReasonComponent', () => {
  let component: DeclineReasonComponent;
  let fixture: ComponentFixture<DeclineReasonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeclineReasonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeclineReasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

