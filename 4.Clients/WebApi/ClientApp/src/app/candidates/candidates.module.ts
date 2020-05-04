import { NgModule } from '@angular/core';
import { CandidatesComponent } from './candidates.component';
import { RouterModule } from '@angular/router';
import { CandidatesRoutes } from './candidates.routes';
import { CommonModule } from '@angular/common';
import { CandidateDetailsComponent } from './details/candidate-details.component';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PipesModule } from '../pipes/pipes.module';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzRadioModule } from 'ng-zorro-antd/radio';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSliderModule } from 'ng-zorro-antd/slider';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { SharedModule } from '../shared.module';

@NgModule({
    declarations: [CandidatesComponent],
    imports: [
        RouterModule.forChild(CandidatesRoutes),
        CommonModule,
        NzTableModule,
        NzDropDownModule,
        NzSelectModule,
        FormsModule,
        PipesModule,
        NzDividerModule,
        ReactiveFormsModule,
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
    providers: [CandidateDetailsComponent],
    exports: [CandidatesComponent]
})

export class CandidatesModule { }
