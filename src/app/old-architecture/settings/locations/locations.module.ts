import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { OfficeComponent } from '@old-architecture/settings/office/office.component';
import { RoomComponent } from '@old-architecture/settings/room/room.component';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzButtonModule, NzDividerModule, NzFormModule, NzInputModule, NzListModule, NzModalModule, NzPopoverModule, NzSelectModule, NzTableModule, NzTagModule } from 'ng-zorro-antd';
import { LocationsComponent } from './locations.component';
import { LocationsRoutes } from './locations.routing';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [
    OfficeComponent,
    RoomComponent,
    LocationsComponent,
  ],
  imports: [
    RouterModule.forChild(LocationsRoutes),
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
    LocationsComponent,
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
export class LocationsModule { }
