import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root'
})
export class HubService extends BaseService<HubService> {
  public connection: signalR.HubConnection;

  connect() {
    if (!this.connection) {
      this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.apiUrl + 'hub')
      .build();

      this.connection.start().then(() => {
        console.log('Hub connection started');
      }).catch(err => console.log(err));
    }
  }

  disconnect() {
    if (this.connection) {
      this.connection.stop();
      this.connection = null;
    }
  }
}