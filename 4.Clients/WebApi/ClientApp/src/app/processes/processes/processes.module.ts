import { NgModule } from '@angular/core';
import { ProcessesComponent } from './processes.component';
import { RouterModule } from '@angular/router';
import { ProcessesRoutes } from './processes.routes';
import { CommonModule } from '@angular/common';
import { ProcessTableComponent } from '../processes-table/processes-table.component';
import { PipesModule } from '../../pipes/pipes.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SlickModule } from 'ngx-slick';
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
        NzTabsModule,
        NzModalModule,
        NzDividerModule,
        NzStepsModule,
        FormsModule,
        ReactiveFormsModule,
        NzFormModule,
        SlickModule,
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
        PipesModule,
        NzIconModule,
        NzToolTipModule,
        SharedModule
    ],
    providers: [
        CandidateDetailsComponent,
        UserDetailsComponent
    ],
    exports: [
        ProcessesComponent,
        ProcessTableComponent
    ],
})

export class ProcessesModule { }
