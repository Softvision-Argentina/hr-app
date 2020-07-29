import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TechnicalStageComponent } from './technical-stage.component';

describe('TechnicalStageComponent', () => {
  let component: TechnicalStageComponent;
  let fixture: ComponentFixture<TechnicalStageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TechnicalStageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TechnicalStageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
