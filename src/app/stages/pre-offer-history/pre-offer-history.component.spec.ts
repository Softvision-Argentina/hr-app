import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreOfferHistory } from './pre-offer-history.component';

describe('OfferHistory', () => {
  let component: PreOfferHistory;
  let fixture: ComponentFixture<PreOfferHistory>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreOfferHistory ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PreOfferHistory);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
