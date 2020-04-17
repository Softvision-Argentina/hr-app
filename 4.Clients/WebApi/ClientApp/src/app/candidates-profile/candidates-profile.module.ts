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
  NzButtonModule
} from 'ng-zorro-antd';
import { CandidatesProfileRoutes } from './candidates-profile.routes';
import { RouterModule } from '@angular/router';
import { CandidatesProfileComponent } from './candidates-profile.component';
import { SettingsComponent } from './../settings/settings.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [CandidatesProfileComponent, SettingsComponent],
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
    PipesModule
  ],
  providers: [SettingsComponent],
  exports: [CandidatesProfileComponent]
})
export class CandidatesProfileModule { }
