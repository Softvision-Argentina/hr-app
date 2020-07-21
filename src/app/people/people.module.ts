import { NgModule } from '@angular/core';
import { PeopleComponent } from './people.component';
import { RouterModule } from '@angular/router';
import { PeopleRoutes } from './people.routes';
import { CommonModule } from '@angular/common';
import { CandidatesModule } from '../candidates/candidates.module';
import { SharedModule } from '../shared.module';
import { NZ_I18N, en_US } from 'ng-zorro-antd';

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
