import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import {
    NzDividerModule,
    NzSelectModule,
    NzStepsModule,
    NzTimelineModule
} from 'ng-zorro-antd';
import { ProcessStepsComponent } from './process-steps.component';
import { ProcessStepsRoutes } from './process-steps.routing';

@NgModule({
    declarations: [ProcessStepsComponent],
    imports: [
        CommonModule,
        RouterModule.forChild(ProcessStepsRoutes),
        FormsModule,
        ReactiveFormsModule,
        NzDividerModule,
        NzSelectModule,
        NzStepsModule,
        NzTimelineModule
    ],
    exports: [
        ProcessStepsComponent,
        NzDividerModule,
        NzSelectModule,
        NzStepsModule,
        NzTimelineModule
    ]
})

export class ProcessStepsModule { }
