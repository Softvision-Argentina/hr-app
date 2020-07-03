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
import { IconsProviderModule } from '@app/icons-provider.module';

@NgModule({
  imports: [
    RouterModule.forChild(CandidatesProfileRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PipesModule,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
    NzInputModule,
    NzButtonModule,
    NzLayoutModule,
    SettingsModule,
    IconsProviderModule
  ],
  exports: [
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
    NzInputModule,
    NzButtonModule,
    NzLayoutModule,
    IconsProviderModule,
    SettingsModule
  ]
})
export class CandidatesProfileModule { }
