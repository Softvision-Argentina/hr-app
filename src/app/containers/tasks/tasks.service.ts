import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class TasksService {
    public headers: HttpHeaders;
    public headersWithAuth: HttpHeaders;
    public apiUrl: string;
    token: string;

    constructor(private httpClient: HttpClient) {
        this.headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
        const user = JSON.parse(localStorage.getItem('currentUser'));
        this.token = user !== null ? user.token : null;
        this.headersWithAuth = new HttpHeaders({
            Authorization: 'Bearer ' + this.token,
            'Content-Type': 'application/json',
        });
        this.apiUrl = 'http://localhost:61059/api/tasks';
    }
    add(task: any) {
        return this.httpClient.post(this.apiUrl, task);
    }
    get(userId: number) {
        const url = `${this.apiUrl}/${userId}`;
        return this.httpClient.get(url);
    }
    delete(taskId: any) {
        const url = `${this.apiUrl}/${taskId}`;
        return this.httpClient.delete(url);
    }
}
