
//This file should be removed in future version and rework this method.

import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';

@Injectable({ providedIn: 'root' })
export class AppConfig {

    private config: Object = null;

    constructor() {
        this.config = environment;
    }

    /**
     * This method gets required property value from the configurations in environment file.
     */
    public getConfig(key: any) {
        return this.config[key];
    }
}