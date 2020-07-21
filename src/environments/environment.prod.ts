
import * as config from '../assets/config.production.json';

export const environment = {
  production: true,
  apiUrl: config.apiUrl,
  clientId: config.clientId,
  scopes: config.scopes,
  taskDeadline: config.taskDeadline,
  roles: config.roles
};
