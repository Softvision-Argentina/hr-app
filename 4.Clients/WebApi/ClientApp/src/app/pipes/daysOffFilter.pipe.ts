import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'daysOffFilter',
    pure: false
})
export class DaysOffFilter implements PipeTransform {
    transform(items: any[], value, propName): any {
        let result = value 
            ? items.filter(item =>{
                if (item.employee[propName]) {
                    return item.employee[propName].toString().toUpperCase().indexOf(value.toString().toUpperCase()) !== -1
                }
            })
            : items;
        console.log(result);
        if(result.length === 0) return [-1];
        else return result;
    }
}
