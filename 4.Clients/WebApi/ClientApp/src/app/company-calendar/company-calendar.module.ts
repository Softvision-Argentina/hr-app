import { PipesModule } from './../pipes/pipes.module';
import { TruncatePipe } from './../pipes/truncate.pipe';
import { NoticeCalendarComponent } from './../notice-calendar/notice-calendar.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CompanyCalendarComponent } from './company-calendar.component';
import { CompanyCalendarRoutes } from './company.calendar.routes';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NzTableModule,
  NzDividerModule,
  NzFormModule,
  NzRadioModule,
  NzDatePickerModule,
  NzCalendarModule,
  NzBadgeModule,
  NzLayoutModule,
  NzPopoverModule,
  NzIconModule,
  NzButtonModule,
  NzModalModule,
  NzInputModule
} from 'ng-zorro-antd';

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
