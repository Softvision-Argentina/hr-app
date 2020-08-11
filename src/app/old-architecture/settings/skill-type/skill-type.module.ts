import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzDividerModule, NzDropDownModule, NzFormModule, NzGridModule, NzIconModule, NzInputModule, NzListModule, NzModalModule, NzTableModule } from 'ng-zorro-antd';
import { SkillTypeComponent } from './skill-type.component';
import { SkillTypeRoutes } from './skill-type.routing';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [SkillTypeComponent],
  imports: [
    RouterModule.forChild(SkillTypeRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzGridModule,
    NzTableModule,
    NzDropDownModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule,
    SharedModule
  ],
  exports: [
    SkillTypeComponent,
    NzFormModule,
    NzGridModule,
    NzTableModule,
    NzDropDownModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule
  ]
})
export class SkillTypeModule { }
