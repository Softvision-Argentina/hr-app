import { CandidateDetailsComponent } from 'src/app/candidates/details/candidate-details.component';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import {
  NzCollapseModule,
  NzFormModule,
  NzSliderModule,
  NzSelectModule,
  NzTableModule,
  NzDropDownModule,
  NzInputModule,
  NzCardModule,
  NzGridModule,
  NzStatisticModule
} from 'ng-zorro-antd';
import { ReportsRoutes } from './reports.routes';
import { ReportsComponent } from './reports.component';

@NgModule({
  declarations: [
    ReportsComponent,
    CandidateDetailsComponent
  ],
  imports: [
    RouterModule.forChild(ReportsRoutes),
    CommonModule,
    ReactiveFormsModule,
    FormsModule,
    NzCollapseModule,
    NzFormModule,
    NzSelectModule,
    NzSliderModule,
    NzTableModule,
    NzDropDownModule,
    NzInputModule,
    NzCardModule,
    NzGridModule,
    NzStatisticModule
  ],
  exports: [ReportsComponent]
})
export class ReportsModule { }
