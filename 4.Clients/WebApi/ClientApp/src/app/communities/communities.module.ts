import { CommunitiesComponent } from './communities.component';
import { SettingsComponent } from './../settings/settings.component'
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzGridModule, NzTableModule, NzDividerModule, NzFormModule, NzSelectModule, NzButtonModule, NzIconModule } from 'ng-zorro-antd';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { ComminunitiesRoutes } from './communities.routes';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [CommunitiesComponent, SettingsComponent],
  imports: [
    RouterModule.forChild(ComminunitiesRoutes),
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
    NzLayoutModule
  ],
  providers: [SettingsComponent],
  exports: [CommunitiesComponent]
})
export class CommunitiesModule { }
