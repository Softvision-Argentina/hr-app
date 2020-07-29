import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import {
  NzButtonModule, NzDatePickerModule, NzDividerModule, NzDropDownModule,
  NzFormModule,




  NzIconModule, NzInputModule, NzTableModule
} from 'ng-zorro-antd';
import { EmployeeCasualtiesComponent } from './employee-casualties.component';
import { EmployeeCasualtiesRoutes } from './employee-casualties.routing';

@NgModule({
  declarations: [EmployeeCasualtiesComponent],
  imports: [
    RouterModule.forChild(EmployeeCasualtiesRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzTableModule,
    NzDropDownModule,
    NzFormModule,
    NzDividerModule,
    NzDatePickerModule,
    NzButtonModule,
    NzInputModule,
    NzIconModule
  ],
  exports: [
    EmployeeCasualtiesComponent,
    NzTableModule,
    NzDropDownModule,
    NzFormModule,
    NzDividerModule,
    NzDatePickerModule,
    NzButtonModule,
    NzInputModule,
    NzIconModule
  ]
})
export class EmployeeCasualtiesModule { }
