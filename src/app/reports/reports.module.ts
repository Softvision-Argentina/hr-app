import { NgModule } from '@angular/core';
import { ReportsComponent } from './reports.component';
import { RouterModule } from '@angular/router';
import { ReportsRoutes } from './reports.routes';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { SharedModule } from '../shared.module';
import { NZ_I18N, en_US } from 'ng-zorro-antd';

@NgModule({
    declarations: [ReportsComponent],
    imports: [
        RouterModule.forChild(ReportsRoutes),
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ChartsModule,
        SharedModule
    ],
    exports: [ReportsComponent],
    providers: [{ provide: NZ_I18N, useValue: en_US } ]
})

export class ReportsModule { }
