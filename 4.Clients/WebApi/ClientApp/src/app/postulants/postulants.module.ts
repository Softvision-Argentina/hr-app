import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { PostulantsRoutes } from './postulants.routes';
import { PostulantsComponent } from './postulants.component';
import { NzTableModule, NzDropDownModule, NzDividerModule } from 'ng-zorro-antd';

@NgModule({
    declarations: [PostulantsComponent],
    imports: [
        RouterModule.forChild(PostulantsRoutes),
        CommonModule,
        NzTableModule,
        NzDropDownModule,
        NzDividerModule
    ],
    exports: [PostulantsComponent]
})

export class PostulantsModule { }
