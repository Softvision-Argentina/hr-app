import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class RegisterService {
  http: HttpClient;
  private headers: HttpHeaders;
  private baseUrl: string;

  constructor(@Inject('BASE_URL') baseUrl: string, http: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
    this.http = http;
    this.baseUrl = baseUrl;
  }

  register(user) {
    return this.http.post(this.baseUrl + 'api/Register', user, { headers: this.headers }).subscribe(data => { });
  }
}
