import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { SettingsModule } from '@old-architecture/settings/settings.module';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzButtonModule, NzDividerModule, NzFormModule, NzInputModule, NzLayoutModule, NzListModule, NzModalModule, NzPopoverModule, NzTableModule, NzTagModule } from 'ng-zorro-antd';
import { CandidatesProfileRoutes } from './candidates-profile.routing';

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
