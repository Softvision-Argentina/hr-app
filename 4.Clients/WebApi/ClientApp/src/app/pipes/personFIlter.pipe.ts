import { Pipe, PipeTransform } from '@angular/core';
import { filterTransform } from './filterTransform';
@Pipe({
    name: 'personFilter'
})
export class PersonFilter implements PipeTransform {
    transform(items: any, value: any): any {
        let result;
        if (!!value) {
            result = items.filter(item => {
                const itemValue = item.name + item.lastName;
                value = value.toString().toUpperCase();
                return itemValue.toString().toUpperCase().indexOf(value) !== -1;
            });
        } else {
            result = items;
        }
    
        if (result.length === 0) {
            return [-1];
        } else {
            return result;
        }
    }
}
