import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private _loading = true;
  private _showBigImage = true;
  loadingStatus: Subject<boolean> = new Subject();
  showBigImageStatus: Subject<boolean> = new Subject();

  constructor() { }

  get loading(): boolean {
    return this._loading;
  }

  set loading(value: boolean) {
    this._loading = value;
    this.loadingStatus.next(value);
  }

  get showBigImage(): boolean {
    return this._showBigImage;
  }

  set showBigImage(value: boolean) {
    this._showBigImage = value;
  }

  startLoading() {
    this.loading = true;
  }

  stopLoading() {
    this.loading = false;
  }

  showBgImage() {
    this.showBigImage = true;
  }

  removeBgImage() {
    this.showBigImage = false;
  }
}
