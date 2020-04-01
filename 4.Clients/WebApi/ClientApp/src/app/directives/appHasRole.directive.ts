import { Directive, TemplateRef, ViewContainerRef, OnInit, Input } from '@angular/core';
import { User } from 'src/entities/user';
import { JwtHelper } from 'angular2-jwt';

@Directive({
    selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {

    @Input() appHasRole: string[];
    currentUser: User;
    constructor(
        private viewContainerRef: ViewContainerRef,
        private templateRef: TemplateRef<any>,
        private jwtHelper: JwtHelper
    ) {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }

    ngOnInit() {
        if (this.appHasRole.indexOf(this.currentUser.role) !== -1 && this.isUserAuthenticated()) {
            this.viewContainerRef.createEmbeddedView(this.templateRef);
        }
    }

  isUserAuthenticated(): boolean {
    let currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser != null && !this.jwtHelper.isTokenExpired(currentUser.token)) {
      return true;
    }
    else {
      localStorage.clear();
      return false;
    }
  }


}
