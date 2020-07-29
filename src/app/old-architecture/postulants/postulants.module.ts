import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NzDividerModule, NzDropDownModule, NzTableModule } from 'ng-zorro-antd';
import { PostulantsComponent } from './postulants.component';
import { PostulantsRoutes } from './postulants.routing';

@NgModule({
    declarations: [PostulantsComponent],
    imports: [
        RouterModule.forChild(PostulantsRoutes),
        CommonModule,
        NzTableModule,
        NzDropDownModule,
        NzDividerModule
    ],
    exports: [
        PostulantsComponent,
        NzTableModule,
        NzDropDownModule,
        NzDividerModule
    ]
})

export class PostulantsModule { }
