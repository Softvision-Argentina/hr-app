import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzCalendarModule } from 'ng-zorro-antd/calendar';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { ReservationsComponent } from './reservations.component';
import { ReservationsRoutes } from './reservations.routing';

@NgModule({
    declarations: [ReservationsComponent],
    imports: [
        RouterModule.forChild(ReservationsRoutes),
        CommonModule,
        FormsModule,
        NzSelectModule,
        NzMenuModule,
        NzCalendarModule,
        NzPopoverModule,
        NzPageHeaderModule,
        NzBadgeModule,
        NzDividerModule,
        NzFormModule,
        NzDatePickerModule,
        ReactiveFormsModule,
        
    ],
    exports: [
        ReservationsComponent,
        NzSelectModule,
        NzMenuModule,
        NzCalendarModule,
        NzPopoverModule,
        NzPageHeaderModule,
        NzBadgeModule,
        NzDividerModule,
        NzFormModule,
        NzDatePickerModule
    ]
})

export class ReservationsModule {}