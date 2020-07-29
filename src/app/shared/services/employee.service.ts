import { Injectable, Inject } from '@angular/core';
import { AppConfig } from '@shared/utils/app.config';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Employee } from '@shared/models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService extends BaseService<Employee> {
  employees = [{ id: 1, name: "Kevin", lastName: "Zatel", email: "kevin.zatel@softvision.com" },
  { id: 2, name: "Carlos", lastName: "Hemingway", email: "soycarlitos66@gmail.com" }];
  
  http: HttpClient;
  public headers: HttpHeaders;
  //Cambiar la URL por una constante global.
  private baseUrl: string = 'http://localhost:61059/';  

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    this.http = http;
    this.apiUrl += 'Employees';
  }

  public Update(employee:Employee) {
    return this.http.post(this.baseUrl 
      + 'api/Employees/Update', employee, { headers: this.headers, observe: "response" })
  }

  public GetByDNI(dni : Number){
    return this.http.get(this.baseUrl 
      + 'api/Employees/getbydni?dni=' + dni, { headers: this.headers, observe: "response" })
  }

  public GetByEmail(email: string){
    return this.http.get(this.baseUrl
      + 'api/Employees/GetByEmail?email=' + email, { headers: this.headers, observe: "response" })
  }
}
