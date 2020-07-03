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
  NzFormModule,
  NzIconModule
} from 'ng-zorro-antd';

import { DeclineReasonsRoutes } from './decline-reasons.routes';
import { DeclineReasonComponent } from './decline-reasons.component';
import { IconsProviderModule } from '@app/icons-provider.module';

@NgModule({
  declarations: [DeclineReasonComponent],
  imports: [
    RouterModule.forChild(DeclineReasonsRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzGridModule,
    NzTableModule,
    NzDropDownModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule,
    IconsProviderModule
  ],
  exports: [
    DeclineReasonComponent,
    NzTableModule,
    NzButtonModule,
    NzDropDownModule,
    NzDividerModule,
    NzGridModule,
    NzInputModule,
    NzModalModule,
    NzListModule,
    NzFormModule,
    NzIconModule,
    IconsProviderModule
  ]
})
export class DeclineReasonsModule { }
