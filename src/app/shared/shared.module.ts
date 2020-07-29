import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { en_US, NzBadgeModule, NzButtonModule, NzCardModule, NzCollapseModule, NzDatePickerModule, NzDividerModule, NzDropDownModule, NzFormModule, NzInputModule, NzLayoutModule, NzListModule, NzMenuModule, NzModalModule, NzPopoverModule, NzRadioModule, NzSelectModule, NzSliderModule, NzStatisticModule, NzTableModule, NzTabsModule, NzTagModule, NzTypographyModule, NzUploadModule, NZ_I18N } from 'ng-zorro-antd';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { FileUploadModule } from 'ng2-file-upload';
import { CandidatesProfileComponent } from '../old-architecture/candidates-profile/candidates-profile.component';
import { CandidateAddComponent } from '../old-architecture/candidates/add/candidate-add.component';
import { CandidateDetailsComponent } from '../old-architecture/candidates/details/candidate-details.component';
import { CommunitiesComponent } from '../old-architecture/communities/communities.component';
import { IconsProviderModule } from './icons-provider.module';
import { ProcessContactComponent } from '../old-architecture/processes/process-contact/process-contact.component';
import { OpenPositionsComponent } from '../old-architecture/referrals/open-positions/open-positions.component';
import { PipesModule } from './pipes/pipes.module';
import { ClientStageComponent } from '../old-architecture/stages/client-stage/client-stage.component';
import { HireStageComponent } from '../old-architecture/stages/hire-stage/hire-stage.component';
import { HrStageComponent } from '../old-architecture/stages/hr-stage/hr-stage.component';
import { OfferStageComponent } from '../old-architecture/stages/offer-stage/offer-stage.component';
import { PreOfferHistory } from '../old-architecture/stages/pre-offer-history/pre-offer-history.component';
import { PreOfferStageComponent } from '../old-architecture/stages/pre-offer-stage/pre-offer-stage.component';
import { TechnicalStageComponent } from '../old-architecture/stages/technical-stage/technical-stage.component';
import { TextEditorComponent } from '../old-architecture/text-editor/text-editor.component';
import { UserDetailsComponent } from '../old-architecture/users/details/user-details.component';


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
        NzSpaceModule
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
        CommunitiesComponent,
        CandidatesProfileComponent,
        TextEditorComponent,
        OpenPositionsComponent        
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
        CommunitiesComponent,
        CandidatesProfileComponent,
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
        NzSpaceModule        
    ],
    providers: [{ provide: NZ_I18N, useValue: en_US }]
})
export class SharedModule { }
