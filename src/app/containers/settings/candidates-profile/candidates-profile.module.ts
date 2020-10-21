import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { IconsProviderModule } from '@app/shared/icons-provider.module';
import { PipesModule } from '@shared/pipes/pipes.module';
import { NzButtonModule, NzDividerModule, NzFormModule, NzInputModule, NzLayoutModule, NzListModule, NzModalModule, NzPopoverModule, NzTableModule, NzTagModule } from 'ng-zorro-antd';
import { CandidatesProfileRoutes } from './candidates-profile.routing';
import { RecruInputDirective } from '@app/shared/directives/recru-input.directive';
import { CandidatesProfileComponent } from './candidates-profile.component';
import { CandidateProfilesSandbox } from './candidates-profile.sandbox';
import { SharedModule } from '@shared/shared.module';

@NgModule({
    declarations: [CandidatesProfileComponent],
    imports: [
        RouterModule.forChild(CandidatesProfileRoutes),
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        PipesModule,
        NzFormModule,
        NzTableModule,
        NzDividerModule,
        NzModalModule,
        NzListModule,
        NzTagModule,
        NzPopoverModule,
        NzInputModule,
        NzButtonModule,
        NzLayoutModule,
        IconsProviderModule,
        SharedModule
    ],
    providers: [CandidateProfilesSandbox]
})
export class CandidatesProfileModule { }
