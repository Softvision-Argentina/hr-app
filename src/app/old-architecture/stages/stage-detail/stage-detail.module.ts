import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { StageDetailComponent } from './stage-detail.component';
import { StageDetailRoutes } from './stage-detail.routing';

@NgModule({
    declarations: [StageDetailComponent],
    imports: [
        RouterModule.forChild(StageDetailRoutes),
        CommonModule
    ],
    exports: [StageDetailComponent]
})

export class StageDetailModule { }

