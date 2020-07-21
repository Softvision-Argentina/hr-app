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

import { ReaddressReasonTypeRoutes } from './readdress-reason-type.routes';
import { ReaddressReasonTypeComponent } from './readdress-reason-type.component';
import { NzIconModule } from 'ng-zorro-antd/icon';

@NgModule({
  declarations: [ReaddressReasonTypeComponent],
  imports: [
    RouterModule.forChild(ReaddressReasonTypeRoutes),
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
    NzFormModule,
    NzIconModule
  ],
  exports: [ReaddressReasonTypeComponent]
})
export class ReaddressReasonTypeModule { }
