import { NgModule } from '@angular/core';
import { EmployeesComponent } from './employees.component';
import { EmployeesRoutes } from './employees.routes';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzDividerModule } from 'ng-zorro-antd/divider';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzAutocompleteModule } from 'ng-zorro-antd/auto-complete';
import { EmployeeDetailsComponent } from './details/employee-details.component';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzIconModule } from 'ng-zorro-antd/icon';

@NgModule({
    declarations: [EmployeesComponent, EmployeeDetailsComponent],
    imports: [
        RouterModule.forChild(EmployeesRoutes),
        CommonModule,
        NzTableModule,
        NzDropDownModule,
        FormsModule,
        NzDividerModule,
        ReactiveFormsModule,
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
