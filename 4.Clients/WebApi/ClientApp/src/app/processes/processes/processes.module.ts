import { NgModule } from '@angular/core';
import { ProcessesComponent } from './processes.component';
import { RouterModule } from '@angular/router';
import { ProcessesRoutes } from './processes.routes';
import { CommonModule } from '@angular/common';
import { ProcessTableComponent } from '../processes-table/processes-table.component';
import { PipesModule } from '../../pipes/pipes.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import {
    NzFormModule,
    NzTabsModule,
    NzModalModule,
    NzDividerModule,
    NzStepsModule,
    NzSelectModule,
    NzSpinModule,
    NzListModule,
    NzTableModule,
    NzDropDownModule,
    NzBadgeModule,
    NzInputModule,
    NzDatePickerModule,
    NzRadioModule,
    NzSliderModule,
    NzCheckboxModule,
    NzIconModule,
    NzToolTipModule
} from 'ng-zorro-antd';

import { CandidateDetailsComponent } from '../../candidates/details/candidate-details.component';
import { UserDetailsComponent } from '../../users/details/user-details.component';
import { SharedModule } from '../../shared.module';

@NgModule({
    declarations: [
        ProcessesComponent,
        ProcessTableComponent,
    ],
    imports: [
        RouterModule.forChild(ProcessesRoutes),
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        SlickCarouselModule,
        NzTabsModule,
        NzModalModule,
        NzDividerModule,
        NzStepsModule,
        NzFormModule,
        NzSelectModule,
        NzSpinModule,
        NzListModule,
        NzTableModule,
        NzDropDownModule,
        NzBadgeModule,
        NzInputModule,
        NzDatePickerModule,
        NzRadioModule,
        NzSliderModule,
        NzCheckboxModule,
        NzIconModule,
        NzToolTipModule,
        PipesModule,
        SharedModule
    ],
    providers: [
        CandidateDetailsComponent,
        UserDetailsComponent
    ],
    exports: [
        ProcessesComponent,
        ProcessTableComponent,
        NzTabsModule,
        NzModalModule,
        NzDividerModule,
        NzStepsModule,
        NzFormModule,
        NzSelectModule,
        NzSpinModule,
        NzListModule,
        NzTableModule,
        NzDropDownModule,
        NzBadgeModule,
        NzInputModule,
        NzDatePickerModule,
        NzRadioModule,
        NzSliderModule,
        NzCheckboxModule,
        NzIconModule,
        NzToolTipModule,
    ],
})

export class ProcessesModule { }
