import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CandidateDetailsComponent } from 'src/app/candidates/details/candidate-details.component';
import { ReferralsContactComponent } from './referrals-contact.component';
import { ReferralsContactRoutes } from './referrals-contact.routes';

@NgModule({
    declarations: [ReferralsContactComponent, CandidateDetailsComponent],
    imports: [
        RouterModule.forChild(ReferralsContactRoutes),
        CommonModule
    ],
    providers: [CandidateDetailsComponent],
    exports: [ReferralsContactComponent]
})

export class ReferralsContactModule { }
