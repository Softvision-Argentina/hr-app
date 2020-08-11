import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedModule } from '@app/shared/shared.module';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzDividerModule, NzFormModule, NzListModule, NzModalModule, NzPopoverModule, NzSelectModule, NzTableModule, NzTagModule } from 'ng-zorro-antd';
import { ProfilesComponent } from './profiles.component';
import { ProfilesRoutes } from './profiles.routing';


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
  exports: [
    ProfilesComponent,
    NzFormModule,
    NzTableModule,
    NzDividerModule,
    NzSelectModule,
    NzModalModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
  ]
})
export class ProfilesModule { }
