import { PipesModule } from './../pipes/pipes.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import {
  NzTableModule,
  NzDividerModule,
  NzModalModule,
  NzListModule,
  NzTagModule,
  NzPopoverModule,
  NzFormModule,
  NzInputModule,
  NzButtonModule,
  NzLayoutModule
} from 'ng-zorro-antd';
import { CandidatesProfileRoutes } from './candidates-profile.routes';
import { RouterModule } from '@angular/router';
import { SettingsModule } from './../settings/settings.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    RouterModule.forChild(CandidatesProfileRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
    NzInputModule,
    NzButtonModule,
    PipesModule,
    NzLayoutModule,
    SettingsModule
  ]
})
export class CandidatesProfileModule { }
