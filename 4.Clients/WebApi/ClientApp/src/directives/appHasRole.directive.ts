import { Directive, TemplateRef, ViewContainerRef, OnInit, Input } from '@angular/core';
import { User } from 'src/entities/user';

@Directive({
    selector: '[appHasRole]'
})
export class HasRoleDirective implements OnInit {

    @Input() appHasRole: string;
    currentUser: User;
    constructor(
        private viewContainerRef: ViewContainerRef,
        private templateRef: TemplateRef<any>,
    ) {
        this.currentUser = JSON.parse(localStorage.getItem("currentUser"));
    }

    ngOnInit() {
        if (this.currentUser.Role == this.appHasRole) {
            this.viewContainerRef.createEmbeddedView(this.templateRef);
        }
    }


}