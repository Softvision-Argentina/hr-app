import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReaddressReasonTypeComponent } from './readdress-reason-type.component';

describe('ReaddressReasonTypeComponent', () => {
  let component: ReaddressReasonTypeComponent;
  let fixture: ComponentFixture<ReaddressReasonTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReaddressReasonTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReaddressReasonTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

