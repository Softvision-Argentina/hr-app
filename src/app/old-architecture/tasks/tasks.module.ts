import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzCardModule, NzCheckboxModule, NzDatePickerModule, NzDividerModule, NzDropDownModule, NzEmptyModule, NzFormModule, NzGridModule, NzIconModule, NzInputModule, NzPopoverModule, NzSelectModule, NzSkeletonModule, NzSwitchModule } from 'ng-zorro-antd';
import { PipesModule } from '@app/shared/pipes/pipes.module';
import { TasksComponent } from './tasks.component';
import { TasksRoutes } from './tasks.routing';

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


