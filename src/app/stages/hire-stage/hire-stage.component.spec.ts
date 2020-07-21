import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HireStageComponent } from './hire-stage.component';

describe('HireStageComponent', () => {
  let component: HireStageComponent;
  let fixture: ComponentFixture<HireStageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HireStageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HireStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
