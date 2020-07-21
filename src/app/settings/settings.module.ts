import { NzSliderModule, NzMenuModule, } from 'ng-zorro-antd';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { SettingsRoutes } from './settings.routes';
import { RouterModule } from '@angular/router';
import { SettingsComponent } from './settings.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IconsProviderModule } from '@app/icons-provider.module';

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
