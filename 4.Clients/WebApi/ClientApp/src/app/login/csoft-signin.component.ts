import { Component, NgZone } from '@angular/core';
import { FacadeService } from '../services/facade.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from '../../entities/user';
import { JwtHelper } from 'angular2-jwt';
import { Router } from '@angular/router';

@Component({
  selector: 'csoft-login',
  templateUrl: './csoft-signin.component.html',
  styleUrls: ['./csoft-signin.component.css']
})
export class CSoftComponent {

  loginForm: FormGroup;
  authenticatedUser:User;

  constructor(private fb: FormBuilder, private facade: FacadeService, private jwtHelper: JwtHelper, private router: Router, public zone: NgZone) {}

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
      if(!res.user){
        this.invalidUser();
      }else{              
        this.authenticatedUser = {            
          id: res.user.id,
          name: res.user.firstName + " " + res.user.lastName,
          imgURL: "",
          email: res.user.username,
          role: res.user.role,
          token: res.token,
          userDashboards: []
        }
        localStorage.setItem('currentUser', JSON.stringify(this.authenticatedUser));
        this.facade.userService.getRoles();          
        this.facade.modalService.closeAll();
        this.router.navigate(['/']);
      }       
    }, err => {
      this.invalidUser();
    });
  }

  isUserAuthenticated(): boolean{
  let currentUser: User = JSON.parse(localStorage.getItem('currentUser'));
    if(currentUser != null && !this.jwtHelper.isTokenExpired(currentUser.token)) {
      return true;
    }
    else {
      localStorage.clear();
      return false;
    }
  }

  invalidUser(){
    this.zone.run(() => { this.router.navigate(['/unauthorized']);});
    this.facade.toastrService.error('Invalid username or password.');
  }
}