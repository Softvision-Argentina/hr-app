import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Community } from '@shared/models/community.model';

@Injectable()
export class CommunitiesService {
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
        this.apiUrl = 'http://localhost:61059/api/settings/profiles/communities';
    }
    add(community: any) {
        return this.httpClient.post(this.apiUrl, community);
    }
    delete(communityId: any) {
        const url = `${this.apiUrl}/${communityId}`;
        return this.httpClient.delete(url);
    }
    update(community: Community) {
        const url = `${this.apiUrl}/${community.id}`;
        return this.httpClient.put(url, community);
    }
}
