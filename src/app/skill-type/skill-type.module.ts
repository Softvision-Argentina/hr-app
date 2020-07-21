import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NzDividerModule,
  NzGridModule,
  NzTableModule,
  NzDropDownModule,
  NzModalModule,
  NzListModule,
  NzFormModule,
  NzButtonModule,
  NzIconModule,
  NzInputModule
} from 'ng-zorro-antd';
import { SkillTypeComponent } from './skill-type.component';
import { SkillTypeRoutes } from './skill-type.routes';

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
    NzInputModule
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
