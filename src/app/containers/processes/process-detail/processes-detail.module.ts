import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PipesModule } from '@shared/pipes/pipes.module';
import { SharedModule } from '@shared/shared.module';
import { en_US, NZ_I18N } from 'ng-zorro-antd';
import { ProcessDetailComponent } from './process-detail.component';
import { ProcessDetailRoutes } from './processes-detail.routing';

@NgModule({
    declarations: [ProcessDetailComponent],
    imports: [
        RouterModule.forChild(ProcessDetailRoutes),
        CommonModule,
        SharedModule,
        FormsModule,
        ReactiveFormsModule,
        PipesModule
    ],
    exports: [ProcessDetailComponent],
    providers: [ { provide: NZ_I18N, useValue: en_US }]
})

export class ProcessDetailModule { }
