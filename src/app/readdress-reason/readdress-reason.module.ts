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

import { NzIconModule } from 'ng-zorro-antd/icon';
import { ReaddressReasonRoutes } from './readdress-reason.routes';
import { ReaddressReasonComponent } from './readdress-reason.component';
import { NzSelectModule } from 'ng-zorro-antd/select';

@NgModule({
  declarations: [ReaddressReasonComponent],
  imports: [
    RouterModule.forChild(ReaddressReasonRoutes),
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
    NzIconModule,
    NzSelectModule
  ],
  exports: [ReaddressReasonComponent]
})
export class ReaddressReasonModule { }
