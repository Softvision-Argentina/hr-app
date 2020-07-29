import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { UserService } from './user.service';
import { AppConfig } from '@shared/utils/app.config';
import { User } from '@shared/models/user.model';


@Injectable({
  providedIn: 'root'
})
export class AppService {
  private _loading = true;
  private _showBigImage = true;
  loadingStatus: Subject<boolean> = new Subject();
  showBigImageStatus: Subject<boolean> = new Subject();

  constructor(private userService: UserService, private config: AppConfig) { }

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

  isUserRole(roles: string[]): boolean {
    const currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser.role === '') {
      this.userService.getRoles();
    }
    if (roles[0] === 'ALL') {
      roles = this.config.getConfig('roles');
    }
    return roles.indexOf(currentUser.role) !== -1;
  }
}
