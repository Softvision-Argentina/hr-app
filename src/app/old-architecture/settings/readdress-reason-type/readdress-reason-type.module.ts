import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzDividerModule, NzDropDownModule, NzFormModule, NzGridModule, NzInputModule, NzListModule, NzModalModule, NzTableModule } from 'ng-zorro-antd';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { ReaddressReasonTypeComponent } from './readdress-reason-type.component';
import { ReaddressReasonTypeRoutes } from './readdress-reason-type.routing';
import { SharedModule } from '@app/shared/shared.module';


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
    NzIconModule,
    SharedModule
  ],
  exports: [ReaddressReasonTypeComponent]
})
export class ReaddressReasonTypeModule { }
