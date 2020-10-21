import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzButtonModule, NzDividerModule, NzFormModule, NzInputModule, NzListModule, NzModalModule, NzPopoverModule, NzSelectModule, NzTableModule, NzTagModule } from 'ng-zorro-antd';
import { RoomComponent } from './room.component';
import { RoomRoutes } from './room.routing';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [
    RoomComponent,
  ],
  imports: [
    RouterModule.forChild(RoomRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzModalModule,
    NzTableModule,
    NzDividerModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
    NzSelectModule,
    NzButtonModule,
    NzInputModule,
    PipesModule,
    IconsProviderModule,
    SharedModule
  ],
  exports: [
    RoomComponent,
    NzFormModule,
    NzModalModule,
    NzTableModule,
    NzDividerModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
    NzSelectModule,
    NzButtonModule,
    NzInputModule,
    IconsProviderModule
  ]
})
export class RoomModule { }

