import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable()
export class EmptyCacheService {
    emptyCache = false;

    public _cacheInfoSource = new BehaviorSubject<boolean>(this.emptyCache);
    _cacheInfo$ = this._cacheInfoSource.asObservable();

    public clearCache(intruction: boolean) {
        this._cacheInfoSource.next(intruction);
    }

}
