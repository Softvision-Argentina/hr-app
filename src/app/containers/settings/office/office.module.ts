import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzButtonModule, NzDividerModule, NzFormModule, NzInputModule, NzListModule, NzModalModule, NzPopoverModule, NzSelectModule, NzTableModule, NzTagModule } from 'ng-zorro-antd';
import { OfficeComponent } from './office.component';
import { OfficeRoutes } from './office.routing';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { SharedModule } from '@app/shared/shared.module';

@NgModule({
  declarations: [
    OfficeComponent,
  ],
  imports: [
    RouterModule.forChild(OfficeRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzModalModule,
    NzTableModule,
    NzDividerModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
    NzSelectModule,
    NzButtonModule,
    NzInputModule,
    PipesModule,
    IconsProviderModule,
    SharedModule
  ],
  exports: [
    OfficeComponent,
    NzFormModule,
    NzModalModule,
    NzTableModule,
    NzDividerModule,
    NzListModule,
    NzTagModule,
    NzPopoverModule,
    NzSelectModule,
    NzButtonModule,
    NzInputModule,
    IconsProviderModule
  ]
})
export class OfficeModule { }
