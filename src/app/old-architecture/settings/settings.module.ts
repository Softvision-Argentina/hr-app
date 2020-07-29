import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { NzMenuModule, NzSliderModule } from 'ng-zorro-antd';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { SettingsComponent } from './settings.component';
import { SettingsRoutes } from './settings.routing';

@NgModule({
  declarations: [SettingsComponent],
  imports: [
    RouterModule.forChild(SettingsRoutes),
    CommonModule,
    NzLayoutModule,
    NzSliderModule,
    NzMenuModule,
    IconsProviderModule
  ],
  exports: [
    SettingsComponent,
    NzLayoutModule,
    NzSliderModule,
    NzMenuModule,
    IconsProviderModule
  ]
})
export class SettingsModule { }
