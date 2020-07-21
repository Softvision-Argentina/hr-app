import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HireProjectedComponent } from './hire-projected.component';

describe('HireProjectedComponent', () => {
  let component: HireProjectedComponent;
  let fixture: ComponentFixture<HireProjectedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HireProjectedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HireProjectedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
