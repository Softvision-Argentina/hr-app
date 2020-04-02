import {
  NzGridModule,
  NzFormModule,
  NzDividerModule,
  NzInputModule,
  NzCardModule
} from 'ng-zorro-antd';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginComponent } from './login.component';
import { LoginRoutes } from './login.routes';
import { GoogleSigninComponent } from './google-signin.component';
import { CSoftComponent } from './csoft-signin.component';

@NgModule({
  declarations: [
    LoginComponent,
    GoogleSigninComponent,
    CSoftComponent,
  ],
  imports: [
    RouterModule.forChild(LoginRoutes),
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NzGridModule,
    NzFormModule,
    NzDividerModule,
    NzCardModule,
    NzInputModule
  ],
  exports: [LoginComponent]
})
export class LoginModule { }
