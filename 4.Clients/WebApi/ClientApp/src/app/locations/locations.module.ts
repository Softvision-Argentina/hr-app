import { PipesModule } from './../pipes/pipes.module';
import { TruncatePipe } from './../pipes/truncate.pipe';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NzTableModule,
  NzDividerModule,
  NzFormModule,
  NzModalModule,
  NzListModule,
  NzTagModule,
  NzPopoverModule,
  NzSelectModule,
  NzButtonModule,
  NzInputModule
} from 'ng-zorro-antd';
import { LocationsRoutes } from './locations.routes';
import { RoomComponent } from './../room/room.component';
import { OfficeComponent } from './../office/office.component';
import { LocationsComponent } from './locations.component';

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
