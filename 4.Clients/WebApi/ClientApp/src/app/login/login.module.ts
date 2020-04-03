import { LoginComponent } from './login.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GoogleSigninComponent } from './google-signin.component';
import { CSoftComponent } from './csoft-signin.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzFormModule, NzGridModule, NzCardModule, NzDividerModule, NzInputModule } from 'ng-zorro-antd';
import { LoginRoutes } from './login.routes';

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
    NzCardModule,
    NzDividerModule,
    NzInputModule
  ],
  exports: [LoginComponent]
})
export class LoginModule { }
