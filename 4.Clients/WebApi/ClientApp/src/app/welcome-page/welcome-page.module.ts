import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { WelcomePageComponent } from './welcome-page.component';
import { WelcomePageRoutes } from './welcome-page.routes';

@NgModule({
    declarations: [WelcomePageComponent],
    imports: [
        RouterModule.forChild(WelcomePageRoutes),
        CommonModule
    ],
    exports: [WelcomePageComponent]
})

export class WelcomePageModule { }
