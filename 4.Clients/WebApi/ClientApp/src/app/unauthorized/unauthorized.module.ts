import { UnauthorizedRoutes } from './unauthorized.routes';
import { RouterModule } from '@angular/router';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzGridModule } from 'ng-zorro-antd';

@NgModule({
  declarations: [UnauthorizedComponent],
  imports: [
    RouterModule.forChild(UnauthorizedRoutes),
    CommonModule,
    NzGridModule
  ],
  exports: [UnauthorizedComponent]
})
export class UnauthorizedModule { }
