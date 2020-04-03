import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzGridModule } from 'ng-zorro-antd';
import { NotFoundRoutes } from './not-found.routes';

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
