import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzButtonModule, NzCardModule, NzCarouselModule, NzDatePickerModule, NzGridModule, NzIconModule, NzListModule, NzSkeletonModule, NzStatisticModule } from 'ng-zorro-antd';
import { ChartsModule } from 'ng2-charts';
import { DashboardComponent } from './dashboard.component';
import { DashboardRoutes } from './dashboard.routing';
import { ReportCompletedProcessesComponent } from './report-completed-processes/report-completed-processes.component';
import { ReportDeclineReasonsComponent } from './report-decline-reasons/report-decline-reasons.component';
import { ReportHireCasualtiesComponent } from './report-hire-casualties/report-hire-casualties.component';
import { ReportHireProjectionComponent } from './report-hire-projection/report-hire-projection.component';
import { ReportProcessesComponent } from './report-processes/report-processes.component';
import { ReportProgressProcessesComponent } from './report-progress-processes/report-progress-processes.component';
import { ReportSkillsComponent } from './report-skills/report-skills.component';
import { ReportTimetofill1Component } from './report-timetofill1/report-timetofill1.component';
import { ReportTimetofill2Component } from './report-timetofill2/report-timetofill2.component';
import { ReportWeeklyCandidatesComponent } from './report-weekly-candidates/report-weekly-candidates.component';

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
        ChartsModule,
        NzGridModule,
        NzCardModule,
        NzSkeletonModule,
        NzIconModule,
        NzListModule,
        NzStatisticModule,
        NzCarouselModule,
        NzButtonModule,
        NzDatePickerModule,
        ReactiveFormsModule,
        FormsModule
    ],
    exports: [
        DashboardComponent,
        NzGridModule,
        NzCardModule,
        NzSkeletonModule,
        NzIconModule,
        NzListModule,
        NzStatisticModule,
        NzCarouselModule,
        NzButtonModule,
        NzDatePickerModule,
    ]
})

export class DashboardModule { }
