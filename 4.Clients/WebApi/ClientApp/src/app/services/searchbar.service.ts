import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';
@Injectable({
    providedIn: 'root'
  })
export class SearchbarService {
    searchChanged = new Subject<string>();
    search( route: string, value: string) {
        if (!!route && !!value) {
            this.searchChanged.next(value);
        }
    }
}
