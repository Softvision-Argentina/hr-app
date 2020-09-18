import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzCardModule, NzCheckboxModule, NzDatePickerModule, NzDividerModule, NzDropDownModule, NzEmptyModule, NzFormModule, NzGridModule, NzIconModule, NzInputModule, NzPopoverModule, NzSelectModule, NzSkeletonModule, NzSwitchModule } from 'ng-zorro-antd';
import { PipesModule } from '@app/shared/pipes/pipes.module';
import { TasksComponent } from './tasks.component';
import { TasksRoutes } from './tasks.routing';
import { SharedModule } from '@app/shared/shared.module';
import { StoreModule } from '@ngrx/store';
import { taskReducer } from './store/task.reducer';
import { EffectsModule } from '@ngrx/effects';
import { TasksEffects } from './store/tasks.effects';
import { TasksSandbox } from './tasks.sandbox';
import { TasksService } from './tasks.service';

//borrar uno de los tasksService!!!!!

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
        NzCheckboxModule,
        SharedModule,
        StoreModule.forFeature('tasks', taskReducer),
        EffectsModule.forFeature([TasksEffects])
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
    ],
    providers: [
        TasksSandbox,
        TasksService
    ],
    entryComponents: [TasksComponent]
})

export class TasksModule { }