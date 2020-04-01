import { Pipe, PipeTransform } from '@angular/core';
import { filterTransform } from './filterTransform';
@Pipe({
    name: 'filter',
})
export class FilterPipe implements PipeTransform {
    transform(items: any[], value, propName): any {
        return filterTransform(items, value, propName);
    }
}
