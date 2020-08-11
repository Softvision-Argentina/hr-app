import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzDividerModule, NzDropDownModule, NzFormModule, NzGridModule, NzInputModule, NzListModule, NzModalModule, NzTableModule } from 'ng-zorro-antd';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { ReaddressReasonComponent } from './readdress-reason.component';
import { ReaddressReasonRoutes } from './readdress-reason.routing';
import { SharedModule } from '@app/shared/shared.module';


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
    NzSelectModule,
    SharedModule
  ],
  exports: [ReaddressReasonComponent]
})
export class ReaddressReasonModule { }
