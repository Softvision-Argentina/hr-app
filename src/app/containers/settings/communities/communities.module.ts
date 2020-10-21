import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SettingsModule } from '@app/containers/settings/settings.module';
import { NzButtonModule, NzDividerModule, NzFormModule, NzGridModule, NzIconModule, NzSelectModule, NzTableModule } from 'ng-zorro-antd';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { CommunitiesRoutes } from './communities.routing';
import { CommunitiesComponent } from './communities.component';
import { SharedModule } from '@app/shared/shared.module';
@NgModule({
  declarations: [CommunitiesComponent],
  imports: [
    RouterModule.forChild(CommunitiesRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzGridModule,
    NzTableModule,
    NzDividerModule,
    NzFormModule,
    NzSelectModule,
    NzButtonModule,
    NzIconModule,
    NzLayoutModule,
    SettingsModule,
    SharedModule
  ]
})
export class CommunitiesModule { }
