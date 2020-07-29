import { TruncatePipe } from './truncate.pipe';
import { SortPipe } from './task-sort.pipe';
import { PersonFilter } from './person-fIlter.pipe';
import { DaysOffFilter } from './days-off-filter.pipe';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FilterPipe } from './filter.pipe';

@NgModule({
  declarations: [
    DaysOffFilter,
    FilterPipe,
    PersonFilter,
    SortPipe,
    TruncatePipe
  ],
  imports: [
    CommonModule
  ],
  exports: [
    DaysOffFilter,
    FilterPipe,
    PersonFilter,
    SortPipe,
    TruncatePipe
  ]
})
export class PipesModule { }
