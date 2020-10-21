import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { NzButtonModule, NzDividerModule, NzDropDownModule, NzFormModule, NzGridModule, NzIconModule, NzInputModule, NzListModule, NzModalModule, NzTableModule } from 'ng-zorro-antd';
import { DeclineReasonComponent } from './decline-reasons.component';
import { DeclineReasonsRoutes } from './decline-reasons.routing';
import { SharedModule } from '@app/shared/shared.module';
import { DeclineReasonSandbox } from './decline-reasons.sandbox';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { reducer } from './store/decline-reasons.reducer';
import { DeclineReasonEffects } from './store/decline-reasons.effects';


@NgModule({
  declarations: [DeclineReasonComponent],
  imports: [
    RouterModule.forChild(DeclineReasonsRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzFormModule,
    NzGridModule,
    NzTableModule,
    NzDropDownModule,
    NzDividerModule,
    NzModalModule,
    NzListModule,
    NzButtonModule,
    NzIconModule,
    NzInputModule,
    IconsProviderModule,
    SharedModule,
    StoreModule.forFeature('declineReasons', reducer),
    EffectsModule.forFeature([DeclineReasonEffects])
  ],
  providers: [DeclineReasonSandbox]
})
export class DeclineReasonsModule { }
