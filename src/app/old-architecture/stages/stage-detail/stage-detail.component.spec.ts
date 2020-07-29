import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StageDetailComponent } from './stage-detail.component';

describe('StageDetailComponent', () => {
  let component: StageDetailComponent;
  let fixture: ComponentFixture<StageDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ StageDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StageDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
