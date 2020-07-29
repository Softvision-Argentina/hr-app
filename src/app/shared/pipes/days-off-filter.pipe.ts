import { Pipe, PipeTransform } from '@angular/core';
import { filterTransform } from '../utils/filter-transform.util';
@Pipe({
    name: 'daysOffFilter',
    pure: false
})
export class DaysOffFilter implements PipeTransform {
    transform(items: any[], value, propName): any {
        return filterTransform( items, value, propName, 'employee');
    }
}
