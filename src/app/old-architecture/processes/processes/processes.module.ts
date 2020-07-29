import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PipesModule } from '@shared/pipes/pipes.module';
import { SharedModule } from '@shared/shared.module';
import { NzBadgeModule, NzCheckboxModule, NzDatePickerModule, NzDividerModule, NzDropDownModule, NzFormModule, NzIconModule, NzInputModule, NzListModule, NzModalModule, NzRadioModule, NzSelectModule, NzSliderModule, NzSpinModule, NzStepsModule, NzTableModule, NzTabsModule, NzToolTipModule } from 'ng-zorro-antd';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { CandidateDetailsComponent } from '../../candidates/details/candidate-details.component';
import { UserDetailsComponent } from '../../users/details/user-details.component';
import { ProcessTableComponent } from '../processes-table/processes-table.component';
import { ProcessesComponent } from './processes.component';
import { ProcessesRoutes } from './processes.routing';


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
