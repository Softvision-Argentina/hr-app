import { SettingsModule } from './../settings/settings.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzGridModule, NzTableModule, NzDividerModule, NzFormModule, NzSelectModule, NzButtonModule, NzIconModule } from 'ng-zorro-antd';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { CommunitiesRoutes } from './communities.routes';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  imports: [
    RouterModule.forChild(CommunitiesRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzGridModule,
    NzTableModule,
    NzDividerModule,
    NzFormModule,
    NzSelectModule,
    NzButtonModule,
    NzIconModule,
    NzLayoutModule,
    SettingsModule
  ],
  exports: [
    NzGridModule,
    NzTableModule,
    NzDividerModule,
    NzFormModule,
    NzSelectModule,
    NzButtonModule,
    NzIconModule,
    NzLayoutModule
  ]
})
export class CommunitiesModule { }
