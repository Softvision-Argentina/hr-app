import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { NotFoundRoutes } from './not-found.routes';
import { NotFoundComponent } from './not-found.component';
import { NzGridModule } from 'ng-zorro-antd';

@NgModule({
  declarations: [NotFoundComponent],
  imports: [
    RouterModule.forChild(NotFoundRoutes),
    CommonModule,
    NzGridModule
  ],
  exports: [NotFoundComponent]
})
export class NotFoundModule { }
