import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StageEditComponent } from './stage-edit.component';
import { StageEditRoutes } from './stage-edit.routes';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
    declarations: [StageEditComponent],
    imports: [
        RouterModule.forChild(StageEditRoutes),
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        MatAutocompleteModule,
        MatFormFieldModule,
        MatProgressSpinnerModule
    ],
    exports: [StageEditComponent]
})

export class StageEditModule { }

