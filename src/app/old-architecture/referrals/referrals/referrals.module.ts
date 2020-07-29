import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { en_US, NzAvatarModule, NzStepsModule, NzTableModule, NzToolTipModule, NZ_I18N } from 'ng-zorro-antd';
import { SlickCarouselModule } from 'ngx-slick-carousel';
import { SharedModule } from '@app/shared/shared.module';
import { PipesModule } from '@app/shared/pipes/pipes.module';
import { PositionAddComponent } from '../position-add/position-add.component';
import { ReferralsCardComponent } from '../referrals-card/referrals-card.component';
import { ReferralsContactComponent } from '../referrals-contact/referrals-contact.component';
import { ReferralsListComponent } from '../referrals-list/referrals-list.component';
import { ReferralsComponent } from './referrals.component';
import { ReferralsRoutes } from './referrals.routing';

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
