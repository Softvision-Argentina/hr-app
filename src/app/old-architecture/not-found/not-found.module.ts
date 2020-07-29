import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NzGridModule } from 'ng-zorro-antd';
import { NotFoundComponent } from './not-found.component';
import { NotFoundRoutes } from './not-found.routing';


@NgModule({
  declarations: [NotFoundComponent],
  imports: [
    RouterModule.forChild(NotFoundRoutes),
    CommonModule,
    NzGridModule
  ],
  exports: [
    NotFoundComponent,
    NzGridModule
  ]
})
export class NotFoundModule { }
