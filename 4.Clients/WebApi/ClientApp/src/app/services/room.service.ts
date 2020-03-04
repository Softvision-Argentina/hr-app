import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../app-config/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Room } from 'src/entities/room';


@Injectable()
export class RoomService extends BaseService<Room> {

  constructor(router: Router, config: AppConfig, http: HttpClient) {
    super(router, config, http);
    this.apiUrl += 'Room';
  }
}