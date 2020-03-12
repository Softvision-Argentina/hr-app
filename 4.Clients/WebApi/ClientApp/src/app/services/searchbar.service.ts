import { Subject } from 'rxjs';
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
@Injectable({
    providedIn: 'root'
  })
export class SearchbarService {
    searchChanged = new Subject<string>();



    search( route: string, value: string) {
        switch (route) {
            case '/tasks':
                this.searchChanged.next(value);
                break;
            default:
                break;
        }
    }
}
