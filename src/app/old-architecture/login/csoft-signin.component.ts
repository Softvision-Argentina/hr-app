import { Component, NgZone } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { User } from '@shared/models/user.model';
import { FacadeService } from '@shared/services/facade.service';

@Component({
  selector: 'csoft-login',
  templateUrl: './csoft-signin.component.html',
  styleUrls: ['./csoft-signin.component.scss']
})
export class CSoftComponent {

  loginForm: FormGroup;
  authenticatedUser: User;
  inputEmpty: boolean = true;

  constructor(
    private fb: FormBuilder,
    private facade: FacadeService,
    private jwtHelperService: JwtHelperService,
    private router: Router,
    public zone: NgZone
  ) { }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      userName: [null, [Validators.required]],
      password: [null, [Validators.required]]
    });
  }

  ngAfterViewInit() {
    this.isUserAuthenticated();
    document.getElementById('username').focus();
  }

  submitForm(): void {
    for (const i in this.loginForm.controls) {
      this.loginForm.controls[i].markAsDirty();
      this.loginForm.controls[i].updateValueAndValidity();
    }
    if (this.loginForm.valid) {
      this.authenticateUser(this.loginForm.controls.userName.value, this.loginForm.controls.password.value);
    }
  }

  authenticateUser(userName: string, password: string) {
    this.facade.authService.authenticate(userName, password)
      .subscribe(res => {
        if (!res.user) {
          this.invalidUser();
        } else {
          this.authenticatedUser = {
            id: res.user.id,
            firstName: res.user.firstName,
            lastName: res.user.lastName,
            imgURL: "",
            username: res.user.username,
            role: res.user.role,
            token: res.token,
            community: res.user.community,
            userDashboards: []
          }
          localStorage.setItem('currentUser', JSON.stringify(this.authenticatedUser));
          this.facade.userService.getRoles();
          this.facade.modalService.closeAll();
          this.navigateByRole(this.authenticatedUser.role);
        }
      }, err => {
        this.invalidUser();
      });
  }

  isUserAuthenticated(): boolean {
    let currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
    if (currentUser && !this.jwtHelperService.isTokenExpired(currentUser.token)) {
      return true;
    } else {
      localStorage.clear();
      return false;
    }
  }
  navigateByRole(role: string) {

    if (role === 'Common') {
      this.router.navigate(['/referrals']);
    } else if (role === 'Employee') {
      this.router.navigate(['/welcome']);
    } else {
      this.router.navigate(['/']);
    }
  }
  invalidUser() {
    this.zone.run(() => { this.router.navigate(['/unauthorized']); });
    this.facade.toastrService.error('Invalid username or password.');
  }

  checkDirtyandErrors(controlName: string): boolean {
    let control = this.loginForm.get(controlName);
    return control ? control.dirty && control.errors !== null : false;
  }
}
