import { TruncatePipe } from './truncate.pipe';
import { SortPipe } from './taskSort.pipe';
import { PersonFilter } from './personFIlter.pipe';
import { DaysOffFilter } from './daysOffFilter.pipe';
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
