import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NzGridModule } from 'ng-zorro-antd';
import { UnauthorizedRoutes } from './unauthorized.routing';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';

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
