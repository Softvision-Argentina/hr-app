import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutes } from './dashboard.routes';
import {
    NzGridModule,
    NzCardModule,
    NzSkeletonModule,
    NzIconModule,
    NzListModule,
    NzStatisticModule,
    NzCarouselModule,
    NzButtonModule,
    NzDatePickerModule,
} from 'ng-zorro-antd';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { ReportProcessesComponent } from './report-processes/report-processes.component';
import { ReportSkillsComponent } from './report-skills/report-skills.component';
import { ReportCompletedProcessesComponent } from './report-completed-processes/report-completed-processes.component';
import { ReportProgressProcessesComponent } from './report-progress-processes/report-progress-processes.component';
import { ReportHireProjectionComponent } from './report-hire-projection/report-hire-projection.component';
import { ReportHireCasualtiesComponent } from './report-hire-casualties/report-hire-casualties.component';
import { ReportWeeklyCandidatesComponent } from './report-weekly-candidates/report-weekly-candidates.component';
import { ReportTimetofill2Component } from './report-timetofill2/report-timetofill2.component';
import { ReportTimetofill1Component } from './report-timetofill1/report-timetofill1.component';
import { ReportDeclineReasonsComponent } from './report-decline-reasons/report-decline-reasons.component';

@NgModule({
    declarations: [
        DashboardComponent,
        ReportProcessesComponent,
        ReportSkillsComponent,
        ReportCompletedProcessesComponent,
        ReportProgressProcessesComponent,
        ReportHireProjectionComponent,
        ReportHireCasualtiesComponent,
        ReportWeeklyCandidatesComponent,
        ReportTimetofill2Component,
        ReportTimetofill1Component,
        ReportDeclineReasonsComponent,
    ],
    imports: [
        RouterModule.forChild(DashboardRoutes),
        CommonModule,
        NzGridModule,
        NzCardModule,
        NzSkeletonModule,
        ChartsModule,
        NzIconModule,
        NzListModule,
        NzStatisticModule,
        NzCarouselModule,
        NzButtonModule,
        NzDatePickerModule,
        ReactiveFormsModule,
        FormsModule
    ],
    exports: [DashboardComponent]
})

export class DashboardModule { }
