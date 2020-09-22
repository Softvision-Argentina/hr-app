import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NoticeCalendarComponent } from './notice-calendar.component';

describe('NoticeCalendarComponent', () => {
  let component: NoticeCalendarComponent;
  let fixture: ComponentFixture<NoticeCalendarComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NoticeCalendarComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NoticeCalendarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
