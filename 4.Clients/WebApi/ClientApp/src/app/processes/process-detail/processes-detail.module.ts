import { NgModule } from '@angular/core';
import { ProcessDetailComponent } from './process-detail.component';
import { CandidateDetailsComponent } from 'src/app/candidates/details/candidate-details.component';
import { RouterModule } from '@angular/router';
import { ProcessDetailRoutes } from './processes-detail.routes';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import {
    NzDividerModule,
    NzButtonModule,
    NzFormModule
} from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
    declarations: [ProcessDetailComponent],
    imports: [
        RouterModule.forChild(ProcessDetailRoutes),
        CommonModule,
        SharedModule,
        NzDividerModule,
        NzButtonModule,
        FormsModule,
        ReactiveFormsModule,
        NzFormModule
    ],
    exports: [ProcessDetailComponent]
})

export class ProcessDetailModule { }
