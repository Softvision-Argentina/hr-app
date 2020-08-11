import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReaddressReasonComponent  } from './readdress-reason.component';


describe('ReaddressReasonComponent', () => {
  let component: ReaddressReasonComponent;
  let fixture: ComponentFixture<ReaddressReasonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ReaddressReasonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReaddressReasonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

