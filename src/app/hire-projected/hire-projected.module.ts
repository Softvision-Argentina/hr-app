import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NzTableModule,
  NzDropDownModule,
  NzDividerModule,
  NzFormModule,
  NzDatePickerModule,
  NzButtonModule,
  NzIconModule,
  NzInputModule
} from 'ng-zorro-antd';
import { HireProjectedRoutes } from './hire-projected.routes';
import { HireProjectedComponent } from './hire-projected.component';

@NgModule({
  declarations: [HireProjectedComponent],
  imports: [
    RouterModule.forChild(HireProjectedRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzTableModule,
    NzDropDownModule,
    NzDividerModule,
    NzDatePickerModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule
  ],
  exports: [
    HireProjectedComponent,
    NzFormModule,
    NzTableModule,
    NzDropDownModule,
    NzDividerModule,
    NzDatePickerModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule
  ]
})
export class HireProjectedModule { }
