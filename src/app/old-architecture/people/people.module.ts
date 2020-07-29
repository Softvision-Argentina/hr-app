import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { en_US, NZ_I18N } from 'ng-zorro-antd';
import { CandidatesModule } from '../candidates/candidates.module';
import { SharedModule } from '@app/shared/shared.module';
import { PeopleComponent } from './people.component';
import { PeopleRoutes } from './people.routing';

@NgModule({
    declarations: [PeopleComponent],
    imports: [
        RouterModule.forChild(PeopleRoutes),
        CommonModule,
        CandidatesModule,
        SharedModule
    ],
    exports: [PeopleComponent],
    providers: [{ provide: NZ_I18N, useValue: en_US } ]
})

export class PeopleModule { }
