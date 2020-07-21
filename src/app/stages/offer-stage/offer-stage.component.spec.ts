import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OfferStageComponent } from './offer-stage.component';

describe('OfferStageComponent', () => {
  let component: OfferStageComponent;
  let fixture: ComponentFixture<OfferStageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OfferStageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OfferStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
