import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'personFilter'
})
export class PersonFilter implements PipeTransform {
    transform(items: any, value: any): any {
        let result;
        if (!isNaN(value)) {
            result = items.filter(item => {
                if (!!item.dni || item.dni === 0) {
                    return item.dni.toString().toUpperCase().indexOf(value.toString().toUpperCase()) !== -1;
                } else {
                    return item;
                }
            });
        } else if (isNaN(value)) {
            result = items.filter(item => {
                const fullName = item.name + item.lastName;
                return fullName.toString().toUpperCase().indexOf(value.toString().toUpperCase()) !== -1;
            });
        }
        if(!!result && result.length === 0) return [-1];
        else return result;

    }
}
