import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzDividerModule, NzFormModule, NzGridModule, NzIconModule, NzSelectModule, NzTableModule } from 'ng-zorro-antd';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { SettingsModule } from './../settings/settings.module';
import { CommunitiesRoutes } from './communities.routing';

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
