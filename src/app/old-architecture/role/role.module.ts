import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzDividerModule, NzFormModule, NzGridModule, NzInputModule, NzSelectModule, NzTableModule } from 'ng-zorro-antd';
import { RoleComponent } from './role.component';
import { RoleRoutes } from './role.routing';

@NgModule({
  declarations: [RoleComponent],
  imports: [
    RouterModule.forChild(RoleRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzGridModule,
    NzSelectModule,
    NzInputModule,
    NzButtonModule
  ],
  exports: [
    RoleComponent,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzGridModule,
    NzSelectModule,
    NzInputModule,
    NzButtonModule
  ]
})
export class RoleModule { }
