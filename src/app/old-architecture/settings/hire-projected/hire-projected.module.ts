import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzDatePickerModule, NzDividerModule, NzDropDownModule, NzFormModule, NzIconModule, NzInputModule, NzTableModule } from 'ng-zorro-antd';
import { HireProjectedComponent } from './hire-projected.component';
import { HireProjectedRoutes } from './hire-projected.routing';
import { SharedModule } from '@app/shared/shared.module';

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
    NzInputModule,
    SharedModule
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
