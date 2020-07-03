import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TasksComponent } from './tasks.component';
import { TasksRoutes } from './tasks.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  NzSwitchModule,
  NzDropDownModule,
  NzButtonModule,
  NzEmptyModule,
  NzCardModule,
  NzGridModule,
  NzPopoverModule,
  NzIconModule,
  NzDividerModule,
  NzSkeletonModule,
  NzInputModule,
  NzFormModule,
  NzSelectModule,
  NzDatePickerModule,
  NzCheckboxModule
} from 'ng-zorro-antd';
import { PipesModule } from '../pipes/pipes.module';

@NgModule({
  declarations: [TasksComponent],
  imports: [
    RouterModule.forChild(TasksRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    PipesModule,
    NzSwitchModule,
    NzDropDownModule,
    NzButtonModule,
    NzEmptyModule,
    NzGridModule,
    NzCardModule,
    NzPopoverModule,
    NzIconModule,
    NzSkeletonModule,
    NzDividerModule,
    NzInputModule,
    NzFormModule,
    NzSelectModule,
    NzDatePickerModule,
    NzCheckboxModule
  ],
  exports: [
    TasksComponent,
    NzSwitchModule,
    NzDropDownModule,
    NzButtonModule,
    NzEmptyModule,
    NzGridModule,
    NzCardModule,
    NzPopoverModule,
    NzIconModule,
    NzSkeletonModule,
    NzDividerModule,
    NzInputModule,
    NzFormModule,
    NzSelectModule,
    NzDatePickerModule,
    NzCheckboxModule
  ]
})

export class TasksModule { }


