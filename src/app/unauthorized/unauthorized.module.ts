import { NzGridModule } from 'ng-zorro-antd';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { RouterModule } from '@angular/router';
import { UnauthorizedRoutes } from './unauthorized.routes';

@NgModule({
  declarations: [UnauthorizedComponent],
  imports: [
    RouterModule.forChild(UnauthorizedRoutes),
    CommonModule,
    NzGridModule
  ],
  exports: [
    UnauthorizedComponent,
    NzGridModule
  ]
})
export class UnauthorizedModule { }
