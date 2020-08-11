import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { NzButtonModule, NzDividerModule, NzFormModule, NzGridModule, NzInputModule, NzSelectModule, NzTableModule } from 'ng-zorro-antd';
import { RoleComponent } from './role.component';
import { RoleRoutes } from './role.routing';
import { SharedModule } from '@app/shared/shared.module';

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
    NzButtonModule,
    IconsProviderModule,
    SharedModule
  ],
  exports: [
    RoleComponent,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzGridModule,
    NzSelectModule,
    NzInputModule,
    NzButtonModule,
    IconsProviderModule
  ]
})
export class RoleModule { }
