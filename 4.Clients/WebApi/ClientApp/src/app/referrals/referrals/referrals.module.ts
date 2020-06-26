import { NgModule, Directive } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReferralsComponent } from './referrals.component';
import { ReferralsRoutes } from './referrals.routes';
import { ReferralsCardComponent } from '../referrals-card/referrals-card.component';
import { ReferralsContactComponent } from '../referrals-contact/referrals-contact.component';
import { PipesModule } from '../../pipes/pipes.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SlickModule } from 'ngx-slick';
import { SharedModule } from '../../shared.module';
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { ReferralsListComponent } from '../referrals-list/referrals-list.component';
@NgModule({
    declarations: [
        ReferralsComponent,
        ReferralsCardComponent,
        ReferralsListComponent
    ],
    imports: [
        RouterModule.forChild(ReferralsRoutes),
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        SlickModule,
        PipesModule,
        SharedModule,
        NgZorroAntdModule
    ],
    exports: [
        ReferralsComponent,
        ReferralsCardComponent,
        ReferralsListComponent
    ],
    providers: [{ provide: NZ_I18N, useValue: en_US } ]
})

export class ReferralsModule { }
