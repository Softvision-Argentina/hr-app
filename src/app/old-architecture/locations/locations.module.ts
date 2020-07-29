import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzDividerModule, NzFormModule, NzInputModule, NzListModule, NzModalModule, NzPopoverModule, NzSelectModule, NzTableModule, NzTagModule } from 'ng-zorro-antd';
import { OfficeComponent } from './../office/office.component';
import { RoomComponent } from './../room/room.component';
import { PipesModule } from '@shared/pipes/pipes.module';
import { LocationsComponent } from './locations.component';
import { LocationsRoutes } from './locations.routing';

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
    PipesModule
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
    NzInputModule
  ]
})
export class LocationsModule { }
