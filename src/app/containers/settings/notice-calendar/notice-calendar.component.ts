import { Component, Input, OnInit } from '@angular/core';
import { CompanyCalendar } from '@shared/models/company-calendar.model';
import { FacadeService } from '@shared/services/facade.service';

@Component({
  selector: 'app-notice-calendar',
  templateUrl: './notice-calendar.component.html',
  styleUrls: ['./notice-calendar.component.scss']
})
export class NoticeCalendarComponent implements OnInit {

  selectedDate: Date;
  events: Event[] = [];
  currentDayEvents: CompanyCalendar[] = [];
  calendarMode: string;
  @Input() listOfCompanyCalendar: CompanyCalendar[] = [];

  date: Date;
  constructor(private facade: FacadeService) { }

  ngOnInit() {
    this.selectedDate = new Date();
    this.calendarMode = 'month';
  }

  getCurrentDayEvents(): void {
    this.currentDayEvents = this.listOfCompanyCalendar
      .filter(r => r.date.toString().substr(0, 10) === this.selectedDate.toISOString().substr(0, 10));
  }
}

