import { NgModule } from '@angular/core';
import { ProcessDetailComponent } from './process-detail.component';
import { RouterModule } from '@angular/router';
import { ProcessDetailRoutes } from './processes-detail.routes';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NZ_I18N, en_US } from 'ng-zorro-antd';
import { PipesModule } from '../../pipes/pipes.module';

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
