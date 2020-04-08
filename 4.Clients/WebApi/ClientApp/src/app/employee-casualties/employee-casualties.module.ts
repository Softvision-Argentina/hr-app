import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  NzTableModule,
  NzDropDownModule,
  NzFormModule,
  NzDividerModule,
  NzDatePickerModule,
  NzButtonModule,
  NzInputModule,
  NzIconModule
} from 'ng-zorro-antd';
import { EmployeeCasualtiesRoutes } from './employee-casualties.routes';
import { RouterModule } from '@angular/router';
import { EmployeeCasualtiesComponent } from './employee-casualties.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [EmployeeCasualtiesComponent],
  imports: [
    RouterModule.forChild(EmployeeCasualtiesRoutes),
    CommonModule,
    NzTableModule,
    NzDropDownModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzDividerModule,
    NzDatePickerModule,
    NzButtonModule,
    NzInputModule,
    NzIconModule
  ],
  exports: [EmployeeCasualtiesComponent]
})
export class EmployeeCasualtiesModule { }
