import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CandidateDetailsComponent } from './candidates/details/candidate-details.component';
import { CandidateAddComponent } from './candidates/add/candidate-add.component';
import { UserDetailsComponent } from './users/details/user-details.component';
import { HrStageComponent } from './stages/hr-stage/hr-stage.component';
import { HireStageComponent } from './stages/hire-stage/hire-stage.component';
import { TechnicalStageComponent } from './stages/technical-stage/technical-stage.component';
import { ClientStageComponent } from './stages/client-stage/client-stage.component';
import { PreOfferHistory } from './stages/pre-offer-history/pre-offer-history.component';
import { OfferStageComponent } from './stages/offer-stage/offer-stage.component';
import { PreOfferStageComponent } from './stages/pre-offer-stage/pre-offer-stage.component';
import { ProcessContactComponent } from './processes/process-contact/process-contact.component';
import { CommunitiesComponent } from './communities/communities.component';
import { CandidatesProfileComponent } from './candidates-profile/candidates-profile.component';
import { PipesModule } from './pipes/pipes.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NZ_I18N, en_US, NzLayoutModule, NzMenuModule, NzDividerModule, NzFormModule, NzSelectModule, NzDatePickerModule, NzRadioModule, NzModalModule, NzSliderModule, NzInputModule, NzListModule, NzTableModule, NzCollapseModule, NzBadgeModule, NzTagModule, NzPopoverModule, NzStatisticModule, NzCardModule, NzDropDownModule, NzTabsModule, NzButtonModule, NzTypographyModule, NzUploadModule } from 'ng-zorro-antd';
import { FileUploadModule } from 'ng2-file-upload';
import { TextEditorComponent } from './text-editor/text-editor.component';
import { OpenPositionsComponent } from './referrals/open-positions/open-positions.component';
import { BrowserModule } from '@angular/platform-browser';
import { IconsProviderModule } from './icons-provider.module';
import { NzSpaceModule } from 'ng-zorro-antd/space';


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
