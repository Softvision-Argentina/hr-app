import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfferHistory } from './offer-history.component';

describe('OfferHistory', () => {
  let component: OfferHistory;
  let fixture: ComponentFixture<OfferHistory>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfferHistory ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfferHistory);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
