import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StageDetailComponent } from './stage-detail.component';
import { StageDetailRoutes } from './stage-detail.routes';

@NgModule({
    declarations: [StageDetailComponent],
    imports: [
        RouterModule.forChild(StageDetailRoutes),
        CommonModule
    ],
    exports: [StageDetailComponent]
})

export class StageDetailModule { }

