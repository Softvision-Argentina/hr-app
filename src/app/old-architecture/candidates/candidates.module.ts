import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzSliderModule } from 'ng-zorro-antd/slider';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { SharedModule } from '@app/shared/shared.module';
import { PipesModule } from '@app/shared/pipes/pipes.module';
import { CandidatesComponent } from './candidates.component';
import { CandidatesRoutes } from './candidates.routing';
import { CandidateDetailsComponent } from './details/candidate-details.component';

@NgModule({
    declarations: [CandidatesComponent],
    imports: [
        RouterModule.forChild(CandidatesRoutes),
        CommonModule,
        PipesModule,
        FormsModule,
        ReactiveFormsModule,
        NzTableModule,
        NzDropDownModule,
        NzSelectModule,
        NzDividerModule,
        NzFormModule,
        NzRadioModule,
        NzButtonModule,
        NzListModule,
        NzIconModule,
        NzToolTipModule,
        NzInputModule,
        NzSliderModule,
        NzGridModule,
        SharedModule
    ],
    providers: [ CandidateDetailsComponent ],
    exports: [
        CandidatesComponent,
        FormsModule,
        NzTableModule,
        NzDropDownModule,
        NzSelectModule,
        NzDividerModule,
        NzFormModule,
        NzRadioModule,
        NzButtonModule,
        NzListModule,
        NzIconModule,
        NzToolTipModule,
        NzInputModule,
        NzSliderModule,
        NzGridModule,
    ]
})

export class CandidatesModule { }
