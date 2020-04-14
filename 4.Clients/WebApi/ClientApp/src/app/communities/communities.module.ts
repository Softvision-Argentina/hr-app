import { CommunitiesComponent } from './communities.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzGridModule, NzTableModule, NzDividerModule, NzFormModule, NzSelectModule, NzButtonModule, NzIconModule } from 'ng-zorro-antd';
import { ComminunitiesRoutes } from './communities.routes';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

@NgModule({
  declarations: [CommunitiesComponent],
  imports: [
    RouterModule.forChild(ComminunitiesRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzGridModule,
    NzTableModule,
    NzDividerModule,
    NzFormModule,
    NzSelectModule,
    NzButtonModule,
    NzIconModule
  ],
  exports: [CommunitiesComponent]
})
export class CommunitiesModule { }
