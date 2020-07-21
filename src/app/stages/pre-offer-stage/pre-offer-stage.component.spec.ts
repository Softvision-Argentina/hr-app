import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PreOfferStageComponent } from './pre-offer-stage.component';

describe('OfferStageComponent', () => {
  let component: PreOfferStageComponent;
  let fixture: ComponentFixture<PreOfferStageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PreOfferStageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PreOfferStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
