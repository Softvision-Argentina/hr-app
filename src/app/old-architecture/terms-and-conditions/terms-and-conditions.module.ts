import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { TermsAndConditionsComponent } from './terms-and-conditions.component';
import { TermsAndConditionsRoutes } from './terms-and-conditions.routing';
import { NzDividerModule } from 'ng-zorro-antd/divider';

@NgModule({
    declarations: [TermsAndConditionsComponent],
    imports: [
        RouterModule.forChild(TermsAndConditionsRoutes),
        CommonModule,
        NzDividerModule
    ],
    exports: [TermsAndConditionsComponent]
})

export class TermsAndConditionsModule { }