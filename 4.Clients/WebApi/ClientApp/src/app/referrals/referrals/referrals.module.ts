import { NgModule, Directive } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReferralsComponent } from './referrals.component';
import { ReferralsRoutes } from './referrals.routes';
import { ReferralsCardComponent } from '../referrals-card/referrals-card.component';
import { ReferralsContactComponent } from '../referrals-contact/referrals-contact.component';
import { PipesModule } from '../../pipes/pipes.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { SharedModule } from '../../shared.module';
import { NZ_I18N, en_US, NzAvatarModule, NzStepsModule, NzToolTipModule,  NzTableModule } from 'ng-zorro-antd';
import { ReferralsListComponent } from '../referrals-list/referrals-list.component';
import { PositionAddComponent } from '../position-add/position-add.component';

@NgModule({
    declarations: [
        ReferralsComponent,
        ReferralsCardComponent,
        ReferralsListComponent,
        ReferralsContactComponent,
        PositionAddComponent
    ],
    imports: [
        RouterModule.forChild(ReferralsRoutes),
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        SlickCarouselModule,
        PipesModule,
        SharedModule,
        NzAvatarModule,
        NzStepsModule,
        NzToolTipModule,
        NzTableModule
    ],
    entryComponents: [ ReferralsContactComponent, PositionAddComponent ],
    exports: [
        ReferralsComponent,
        ReferralsCardComponent,
        ReferralsListComponent,
        ReferralsContactComponent,
        PositionAddComponent,
        NzAvatarModule,
        NzStepsModule,
        NzToolTipModule
    ],
    providers: [{ provide: NZ_I18N, useValue: en_US } ]
})

export class ReferralsModule { }
