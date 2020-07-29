import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzBadgeModule, NzButtonModule, NzCalendarModule, NzDatePickerModule, NzDividerModule, NzFormModule, NzIconModule, NzInputModule, NzLayoutModule, NzModalModule, NzPopoverModule, NzRadioModule, NzTableModule } from 'ng-zorro-antd';
import { NoticeCalendarComponent } from './../notice-calendar/notice-calendar.component';
import { CompanyCalendarComponent } from './company-calendar.component';
import { CompanyCalendarRoutes } from './company.calendar.routing';

@NgModule({
  declarations: [
    CompanyCalendarComponent,
    NoticeCalendarComponent,
  ],
  imports: [
    RouterModule.forChild(CompanyCalendarRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzRadioModule,
    NzDatePickerModule,
    NzCalendarModule,
    NzBadgeModule,
    NzLayoutModule,
    NzPopoverModule,
    NzIconModule,
    NzButtonModule,
    NzModalModule,
    NzInputModule,
    PipesModule
  ],
  exports: [
    CompanyCalendarComponent,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzRadioModule,
    NzDatePickerModule,
    NzCalendarModule,
    NzBadgeModule,
    NzLayoutModule,
    NzPopoverModule,
    NzIconModule,
    NzButtonModule,
    NzModalModule,
    NzInputModule,
  ]
})
export class CompanyCalendarModule { }
