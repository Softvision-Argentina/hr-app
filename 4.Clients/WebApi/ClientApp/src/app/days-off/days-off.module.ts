import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DaysOffRoutes } from './days-off.routes';
import { RouterModule } from '@angular/router';
import { DaysOffComponent } from './days-off.component';
import { PipesModule } from '../pipes/pipes.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';

@NgModule({
    declarations: [DaysOffComponent],
    imports: [
        RouterModule.forChild(DaysOffRoutes),
        CommonModule,
        FormsModule,
        PipesModule,
        ReactiveFormsModule,
        NgZorroAntdModule
    ],
    exports: [DaysOffComponent],
    providers: [{ provide: NZ_I18N, useValue: en_US } ]
})

export class DaysOffModule {}
