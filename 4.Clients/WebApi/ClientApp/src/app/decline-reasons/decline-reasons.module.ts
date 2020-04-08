import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  NzTableModule,
  NzButtonModule,
  NzDropDownModule,
  NzDividerModule,
  NzGridModule,
  NzInputModule,
  NzModalModule,
  NzListModule,
  NzFormModule
} from 'ng-zorro-antd';

import { DeclineReasonsRoutes } from './decline-reasons.routes';
import { DeclineReasonComponent } from './decline-reasons.component';

@NgModule({
  declarations: [DeclineReasonComponent],
  imports: [
    RouterModule.forChild(DeclineReasonsRoutes),
    CommonModule,
    FormsModule,
    NzTableModule,
    NzButtonModule,
    NzDropDownModule,
    NzDividerModule,
    NzGridModule,
    ReactiveFormsModule,
    NzInputModule,
    NzModalModule,
    NzListModule,
    NzFormModule
  ],
  exports: [DeclineReasonComponent]
})
export class DeclineReasonsModule { }
