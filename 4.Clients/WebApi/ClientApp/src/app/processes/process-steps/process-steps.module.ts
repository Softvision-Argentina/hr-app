import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ProcessStepsComponent } from './process-steps.component';
import { ProcessStepsRoutes } from './process-steps.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
    NzDividerModule,
    NzSelectModule,
    NzStepsModule,
    NzTimelineModule
} from 'ng-zorro-antd';

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
