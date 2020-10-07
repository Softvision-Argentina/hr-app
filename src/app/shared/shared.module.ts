import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CandidateAddComponent } from '@old-architecture/candidates/add/candidate-add.component';
import { CandidateDetailsComponent } from '@old-architecture/candidates/details/candidate-details.component';
import { ProcessContactComponent } from '@old-architecture/processes/process-contact/process-contact.component';
import { OpenPositionsComponent } from '../containers/referrals/open-positions/open-positions.component';
import { ClientStageComponent } from '@old-architecture/stages/client-stage/client-stage.component';
import { HireStageComponent } from '@old-architecture/stages/hire-stage/hire-stage.component';
import { HrStageComponent } from '@old-architecture/stages/hr-stage/hr-stage.component';
import { OfferStageComponent } from '@old-architecture/stages/offer-stage/offer-stage.component';
import { PreOfferHistory } from '@old-architecture/stages/pre-offer-history/pre-offer-history.component';
import { PreOfferStageComponent } from '@old-architecture/stages/pre-offer-stage/pre-offer-stage.component';
import { TechnicalStageComponent } from '@old-architecture/stages/technical-stage/technical-stage.component';
import { TextEditorComponent } from '@old-architecture/text-editor/text-editor.component';
import { UserDetailsComponent } from '@old-architecture/users/details/user-details.component';
import { en_US, NzBadgeModule, NzButtonModule, NzCardModule, NzCollapseModule, NzDatePickerModule, NzDividerModule, NzDropDownModule, NzFormModule, NzInputModule, NzLayoutModule, NzListModule, NzMenuModule, NzModalModule, NzPopoverModule, NzRadioModule, NzSelectModule, NzSliderModule, NzStatisticModule, NzTableModule, NzTabsModule, NzTagModule, NzTypographyModule, NzUploadModule, NZ_I18N, NzCheckboxModule } from 'ng-zorro-antd';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { FileUploadModule } from 'ng2-file-upload';
import { RecruInputDirective } from './directives/recru-input.directive';
import { IconsProviderModule } from './icons-provider.module';
import { PipesModule } from './pipes/pipes.module';
import { ReferralsContactComponent } from '@app/containers/referrals/referrals-contact/referrals-contact.component';

import { CommunitiesSandbox } from '@app/containers/settings/communities/communities.sandbox';
import { OfficeSandbox } from '@app/containers/settings/office/office.sandbox';
import { RoomSandbox } from '@app/containers/settings/room/room.sandbox';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        PipesModule,
        NzLayoutModule,
        NzMenuModule,
        FileUploadModule,
        IconsProviderModule,
        NzDividerModule,
        NzFormModule,
        NzSelectModule,
        NzDatePickerModule,
        NzRadioModule,
        NzModalModule,
        NzSliderModule,
        NzInputModule,
        NzListModule,
        NzTableModule,
        NzCollapseModule,
        NzBadgeModule,
        NzTagModule,
        NzPopoverModule,
        NzStatisticModule,
        NzCardModule,
        NzDropDownModule,
        NzTabsModule,
        NzButtonModule,
        NzTypographyModule,
        NzUploadModule,
        NzSpaceModule,
        NzCheckboxModule
    ],
    declarations: [
        CandidateDetailsComponent,
        UserDetailsComponent,
        CandidateAddComponent,
        HrStageComponent,
        HireStageComponent,
        TechnicalStageComponent,
        ClientStageComponent,
        PreOfferHistory,
        PreOfferStageComponent,
        OfferStageComponent,
        ProcessContactComponent,
        TextEditorComponent,
        OpenPositionsComponent,
        RecruInputDirective,
        ReferralsContactComponent
    ],
    exports: [
        CommonModule,
        CandidateDetailsComponent,
        UserDetailsComponent,
        CandidateAddComponent,
        HrStageComponent,
        HireStageComponent,
        TechnicalStageComponent,
        ClientStageComponent,
        PreOfferHistory,
        PreOfferStageComponent,
        OfferStageComponent,
        ProcessContactComponent,
        FileUploadModule,
        TextEditorComponent,
        OpenPositionsComponent,
        TextEditorComponent,
        NzLayoutModule,
        NzMenuModule,
        IconsProviderModule,
        NzDividerModule,
        NzFormModule,
        NzSelectModule,
        NzDatePickerModule,
        NzRadioModule,
        NzModalModule,
        NzSliderModule,
        NzInputModule,
        NzListModule,
        NzTableModule,
        NzCollapseModule,
        NzBadgeModule,
        NzTagModule,
        NzPopoverModule,
        NzStatisticModule,
        NzCardModule,
        NzDropDownModule,
        NzTabsModule,
        NzButtonModule,
        NzTypographyModule,
        NzUploadModule,
        NzSpaceModule,
        RecruInputDirective,
        ReferralsContactComponent
    ],
    providers: [CommunitiesSandbox,OfficeSandbox,RoomSandbox, { provide: NZ_I18N, useValue: en_US }]
})
export class SharedModule { }
