import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'sort',
  pure: false
})
export class SortPipe implements PipeTransform {
  transform(array: any, fn: Function = (a, b) => a.creationDate > b.creationDate ? 1 : -1): any {
    return array.sort(fn);
  }
}
