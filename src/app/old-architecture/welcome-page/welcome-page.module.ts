import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { WelcomePageComponent } from './welcome-page.component';
import { WelcomePageRoutes } from './welcome-page.routing';

@NgModule({
    declarations: [WelcomePageComponent],
    imports: [
        RouterModule.forChild(WelcomePageRoutes),
        CommonModule
    ],
    exports: [WelcomePageComponent]
})

export class WelcomePageModule { }
