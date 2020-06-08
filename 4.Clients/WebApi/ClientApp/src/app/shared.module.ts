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
import { NgZorroAntdModule, NZ_I18N, en_US } from 'ng-zorro-antd';
import { FileUploadModule } from 'ng2-file-upload';
import { ReferralsContactComponent } from './referrals/referrals-contact/referrals-contact.component';
import { TextEditorComponent } from './text-editor/text-editor.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        PipesModule,
        NgZorroAntdModule,
        FileUploadModule
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
        ReferralsContactComponent,
        TextEditorComponent
    ],
    exports: [
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
        ReferralsContactComponent,
        FileUploadModule,
        TextEditorComponent
    ],
    providers: [ { provide: NZ_I18N, useValue: en_US }]
})
export class SharedModule { }
