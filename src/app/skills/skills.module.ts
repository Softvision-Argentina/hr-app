import { PipesModule } from './../pipes/pipes.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  NzTableModule,
  NzDropDownModule,
  NzButtonModule,
  NzInputModule,
  NzDividerModule,
  NzModalModule,
  NzListModule,
  NzFormModule,
  NzSelectModule
} from 'ng-zorro-antd';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SkillsRoutes } from './skills.routes';
import { SkillsComponent } from './skills.component';
import { IconsProviderModule } from '@app/icons-provider.module';

@NgModule({
  declarations: [SkillsComponent],
  imports: [
    RouterModule.forChild(SkillsRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzTableModule,
    NzDropDownModule,
    NzButtonModule,
    NzInputModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzSelectModule,
    PipesModule,
    IconsProviderModule
  ],
  exports: [
    SkillsComponent,
    NzFormModule,
    NzTableModule,
    NzDropDownModule,
    NzButtonModule,
    NzInputModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzSelectModule,
  ]
})
export class SkillsModule { }
