import { Component, NgZone } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { JwtHelper } from 'angular2-jwt';
import { Router } from '@angular/router';
import { FacadeService } from '../services/facade.service';
import { User } from '../../entities/user';

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
    private jwtHelper: JwtHelper,
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
    if (currentUser && !this.jwtHelper.isTokenExpired(currentUser.token)) {
      return true;
    } else {
      localStorage.clear();
      return false;
    }
  }
  navigateByRole(role: string) {

    if (role === 'Common') {
      this.router.navigate(['/referrals']);
    }
    else {
      this.router.navigate(['/'])
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

  checkInputContent(input: HTMLInputElement) {
    debugger;
    input.parentElement.classList.add("active");
    if (input.value == "") {
      input.parentElement.classList.remove("active");
    }
  }
}
