import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzButtonModule, NzDividerModule, NzDropDownModule, NzFormModule, NzInputModule, NzListModule, NzModalModule, NzSelectModule, NzTableModule } from 'ng-zorro-antd';
import { SkillsComponent } from './skills.component';
import { SkillsRoutes } from './skills.routing';
import { SharedModule } from '@app/shared/shared.module';

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
    IconsProviderModule,
    SharedModule
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
