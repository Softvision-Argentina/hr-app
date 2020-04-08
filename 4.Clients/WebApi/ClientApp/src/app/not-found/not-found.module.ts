import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzGridModule } from 'ng-zorro-antd';

import { NotFoundComponent } from './not-found.component';
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
