import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { en_US, NZ_I18N } from 'ng-zorro-antd';
import { ChartsModule } from 'ng2-charts';
import { SharedModule } from '@app/shared/shared.module';
import { ReportsComponent } from './reports.component';
import { ReportsRoutes } from './reports.routing';

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
