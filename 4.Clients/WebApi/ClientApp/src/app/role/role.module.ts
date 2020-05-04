import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzTableModule, NzDividerModule, NzGridModule, NzFormModule, NzSelectModule, NzInputModule, NzButtonModule } from 'ng-zorro-antd';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleRoutes } from './role.routes';
import { RoleComponent } from './role.component';

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
  exports: [RoleComponent]
})
export class RoleModule { }
