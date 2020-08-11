import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NzCardModule, NzDividerModule, NzFormModule, NzGridModule, NzInputModule } from 'ng-zorro-antd';
import { CSoftComponent } from './csoft-signin.component';
import { GoogleSigninComponent } from './google-signin.component';
import { LoginComponent } from './login.component';
import { LoginRoutes } from './login.routing';
import { SharedModule } from '@app/shared/shared.module';


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
    NzInputModule,
    SharedModule
  ],
  exports: [
    LoginComponent,
    NzGridModule,
    NzFormModule,
    NzCardModule,
    NzDividerModule,
    NzInputModule
  ]
})
export class LoginModule { }
