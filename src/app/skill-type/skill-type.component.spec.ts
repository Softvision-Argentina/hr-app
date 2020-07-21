import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillTypeComponent } from './skill-type.component';

describe('SkillTypeComponent', () => {
  let component: SkillTypeComponent;
  let fixture: ComponentFixture<SkillTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
