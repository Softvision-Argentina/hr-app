import { NzSliderModule, } from 'ng-zorro-antd';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { SettingsRoutes } from './settings.routes';
import { RouterModule } from '@angular/router';
import { SettingsComponent } from './settings.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [SettingsComponent],
  imports: [
    RouterModule.forChild(SettingsRoutes),
    CommonModule,
    NzLayoutModule,
    NzSliderModule,
  ],
  exports: [SettingsComponent]
})
export class SettingsModule { }
