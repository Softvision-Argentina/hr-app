import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filter',
    pure: false
})
export class FilterPipe implements PipeTransform {
    transform(items: any[], value, propName): any {
        let result = value 
            ? items.filter(item => item[propName].toString().toUpperCase().indexOf(value.toString().toUpperCase()) !== -1)
            : items;

        if(result.length === 0) return [-1];
        else return result;
    }
}
