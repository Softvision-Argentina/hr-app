import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from '@shared/utils/app.config';
import { BaseService } from './base.service';
import { Router } from '@angular/router';
import { Role } from '@shared/models/role.model';


@Injectable()
export class RoleService extends BaseService<Role> {

    constructor(router: Router, config: AppConfig, http: HttpClient) {
        super(router, config, http);
        this.apiUrl += 'Role';
    }
}