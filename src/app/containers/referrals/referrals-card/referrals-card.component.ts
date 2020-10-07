import { Component, Input, OnInit } from '@angular/core';
import { Candidate } from '@shared/models/candidate.model';
import { Cv } from '@shared/models/cv.model';
import { BaseService } from '@shared/services/base.service';
import { FileUploader } from 'ng2-file-upload';

@Component({
  selector: 'app-referrals-card',
  templateUrl: './referrals-card.component.html',
  styleUrls: ['./referrals-card.component.scss']
})
export class ReferralsCardComponent implements OnInit {
  @Input()
  cand: Candidate;
  cv: Cv;
  uploader: FileUploader;
  response: string;
  hasFile: boolean = false;

  constructor(private b: BaseService<Cv>) { }

  ngOnInit() {
    this.initializeUploader();
  }
  
  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.b.apiUrl + 'Cv/' + this.cand.id,
      authToken: 'Bearer ' + localStorage.getItem('token'),
      isHTML5: true,
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    
    this.response = '';
    
    this.uploader.response.subscribe( res => this.response = res );
    
    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false; this.hasFile = true;};
    
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const res: Cv = JSON.parse(response);
        const cv = {
          id: res.id,
          url: res.url,
          file: res.file
        };
      }
    };
  }
}
