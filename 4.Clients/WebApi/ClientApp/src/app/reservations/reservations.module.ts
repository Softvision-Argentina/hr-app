import { NgModule } from '@angular/core';
import { ReservationsComponent } from './reservations.component'
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReservationsRoutes } from './reservations.routes';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzCalendarModule } from 'ng-zorro-antd/calendar';
import { NzPageHeaderModule } from 'ng-zorro-antd/page-header';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzPopoverModule } from 'ng-zorro-antd/popover';

@NgModule({
    declarations: [ReservationsComponent],
    imports: [
        RouterModule.forChild(ReservationsRoutes),
        CommonModule,
        NzSelectModule,
        NzMenuModule,
        NzCalendarModule,
        FormsModule,
        NzPopoverModule,
        NzPageHeaderModule,
        NzBadgeModule,
        NzDividerModule,
        NzFormModule,
        NzDatePickerModule,
        ReactiveFormsModule,
        
    ],
    exports: [ReservationsComponent]
})

export class ReservationsModule {}