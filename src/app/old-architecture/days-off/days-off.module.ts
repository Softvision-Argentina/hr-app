import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { en_US, NZ_I18N } from 'ng-zorro-antd';
import { SharedModule } from '@app/shared/shared.module';
import { PipesModule } from '@app/shared/pipes/pipes.module';
import { DaysOffComponent } from './days-off.component';
import { DaysOffRoutes } from './days-off.routing';

@NgModule({
    declarations: [DaysOffComponent],
    imports: [
        RouterModule.forChild(DaysOffRoutes),
        CommonModule,
        FormsModule,
        PipesModule,
        ReactiveFormsModule,
        SharedModule
    ],
    exports: [ DaysOffComponent ],
    providers: [{ provide: NZ_I18N, useValue: en_US } ]
})

export class DaysOffModule {}
