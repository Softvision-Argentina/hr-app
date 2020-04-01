import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filter',
    pure: false
})
export class FilterPipe implements PipeTransform {
    transform(items: any[], title): any {
        const result = title
            ? items.filter(item => item.title.toString().toUpperCase().indexOf(title.toString().toUpperCase()) !== -1)
            : items;

        if (result.length === 0) { return [-1]; } else { return result; }
    }
}
