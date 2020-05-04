import { PipesModule } from './../pipes/pipes.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NzTableModule,
  NzDividerModule,
  NzFormModule,
  NzSelectModule,
  NzModalModule,
  NzListModule,
  NzTagModule,
  NzPopoverModule
} from 'ng-zorro-antd';

import { ProfilesRoutes } from './profiles.routes';
import { ProfilesComponent } from './profiles.component';
import { SharedModule } from '../shared.module';

@NgModule({
  declarations: [
    ProfilesComponent
  ],
  imports: [
    RouterModule.forChild(ProfilesRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzSelectModule,
    NzModalModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
    PipesModule,
    SharedModule
  ],
  exports: [ProfilesComponent]
})
export class ProfilesModule { }
