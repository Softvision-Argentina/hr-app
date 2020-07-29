import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HrStageComponent } from './hr-stage.component';

describe('HrStageComponent', () => {
  let component: HrStageComponent;
  let fixture: ComponentFixture<HrStageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HrStageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HrStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
