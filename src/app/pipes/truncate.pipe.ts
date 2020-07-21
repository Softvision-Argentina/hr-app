import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncate',
  pure: false
})
export class TruncatePipe implements PipeTransform {
  transform(value: string, length: number): string {
    if (!value) {
      return '';
    } else if (value.length < length) {
      return value;
    } else {
      return value.substring(0, length).concat(' ...');
    }
  }
}
