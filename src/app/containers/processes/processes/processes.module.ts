import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PipesModule } from '@shared/pipes/pipes.module';
import { SharedModule } from '@shared/shared.module';
import { NzBadgeModule, NzCheckboxModule, NzDatePickerModule, NzDividerModule, NzDropDownModule, NzFormModule, NzIconModule, NzInputModule, NzListModule, NzModalModule, NzRadioModule, NzSelectModule, NzSliderModule, NzSpinModule, NzStepsModule, NzTableModule, NzTabsModule, NzToolTipModule } from 'ng-zorro-antd';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { CandidateDetailsComponent } from '@old-architecture/candidates/details/candidate-details.component';
import { UserDetailsComponent } from '@old-architecture/users/details/user-details.component';
import { ProcessTableComponent } from '../processes-table/processes-table.component';
import { ProcessesComponent } from './processes.component';
import { ProcessesRoutes } from './processes.routing';
import { ProcessSandbox } from '@app/containers/processes/processes.sandbox';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { processReducer } from '@app/containers/processes/store/process.reducer';
import { ProcessEffects } from '@app/containers/processes/store/process.effects';
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
        SharedModule,
        StoreModule.forFeature('processes', processReducer),
        EffectsModule.forFeature([ProcessEffects]),
    ],
    providers: [
        CandidateDetailsComponent,
        UserDetailsComponent,
        ProcessSandbox
    ],
    exports: [
        ProcessesComponent,
        ProcessTableComponent,
        // NzTabsModule,
        // NzModalModule,
        // NzDividerModule,
        // NzStepsModule,
        // NzFormModule,
        // NzSelectModule,
        // NzSpinModule,
        // NzListModule,
        // NzTableModule,
        // NzDropDownModule,
        // NzBadgeModule,
        // NzInputModule,
        // NzDatePickerModule,
        // NzRadioModule,
        // NzSliderModule,
        // NzCheckboxModule,
        // NzIconModule,
        // NzToolTipModule,
    ],
})

export class ProcessesModule { }
