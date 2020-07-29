import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DaysOffComponent } from './days-off.component';

describe('DaysOffComponent', () => {
  let component: DaysOffComponent;
  let fixture: ComponentFixture<DaysOffComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DaysOffComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DaysOffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
