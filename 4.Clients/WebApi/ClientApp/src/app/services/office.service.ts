import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '../app-config/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Office } from 'src/entities/office';


@Injectable()
export class OfficeService extends BaseService<Office> {

    constructor(router: Router, config: AppConfig, http: HttpClient) {
        super(router, config, http);
        this.apiUrl += 'Office';
    }
}