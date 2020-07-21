// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.
export const environment = {
  production: false,
  apiUrl: "http://localhost:61059/api/",
  clientId: "817485236596-gbllrhc65h8jmn14purvci98brcdq3sq.apps.googleusercontent.com",
  scopes: ["profile", "email"],
  taskDeadline: 3,
  roles: ["Admin", "HRManagement", "HRUser", "Interviewer", "TechnicalInterviewer", "Common", "CommunityManager", "Recruiter", "Employee"]
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
