import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeclineReasonsComponent } from './decline-reasons.component';

describe('DeclineReasonsComponent', () => {
  let component: DeclineReasonsComponent;
  let fixture: ComponentFixture<DeclineReasonsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeclineReasonsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeclineReasonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
