import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NoticeCalendarComponent } from '@app/containers/settings/notice-calendar/notice-calendar.component';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzBadgeModule, NzButtonModule, NzCalendarModule, NzDatePickerModule, NzDividerModule, NzFormModule, NzIconModule, NzInputModule, NzLayoutModule, NzModalModule, NzPopoverModule, NzRadioModule, NzTableModule } from 'ng-zorro-antd';
import { CompanyCalendarComponent } from './company-calendar.component';
import { CompanyCalendarRoutes } from './company-calendar.routing';
import { SharedModule } from '@app/shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects'
import { reducer } from './store/company-calendar.reducer';
import { CompanyCalendarEffects } from './store/company-calendar.effects';
import { CompanyCalendarSandbox } from './company-calendar.sandbox';

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
    PipesModule,
    SharedModule,
    StoreModule.forFeature('companyCalendar', reducer),
    EffectsModule.forFeature([CompanyCalendarEffects])
  ],
  providers: [CompanyCalendarSandbox]
})
export class CompanyCalendarModule { }
