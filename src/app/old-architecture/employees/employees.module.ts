import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzAutocompleteModule } from 'ng-zorro-antd/auto-complete';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { EmployeeDetailsComponent } from './details/employee-details.component';
import { EmployeesComponent } from './employees.component';
import { EmployeesRoutes } from './employees.routing';

@NgModule({
    declarations: [EmployeesComponent, EmployeeDetailsComponent],
    imports: [
        RouterModule.forChild(EmployeesRoutes),
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        NzTableModule,
        NzDropDownModule,
        NzDividerModule,
        NzFormModule,
        NzInputModule,
        NzSelectModule,
        NzAutocompleteModule,
        NzListModule,
        NzToolTipModule,
        NzIconModule
    ],
    exports: [
        NzTableModule,
        NzDropDownModule,
        NzDividerModule,
        NzFormModule,
        NzInputModule,
        NzSelectModule,
        NzAutocompleteModule,
        NzListModule,
        NzToolTipModule,
        NzIconModule
    ]
})

export class EmployeesModule { }
