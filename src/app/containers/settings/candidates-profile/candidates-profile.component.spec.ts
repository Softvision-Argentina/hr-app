import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CandidatesProfileComponent } from './candidates-profile.component';

describe('CandidatesProfileComponent', () => {
  let component: CandidatesProfileComponent;
  let fixture: ComponentFixture<CandidatesProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CandidatesProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CandidatesProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
