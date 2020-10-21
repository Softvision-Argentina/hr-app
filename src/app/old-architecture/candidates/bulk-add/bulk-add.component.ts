import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CandidateAddComponent } from '@old-architecture/candidates/add/candidate-add.component';
import { Candidate } from '@shared/models/candidate.model';
import { Community } from '@shared/models/community.model';
import { Cv } from '@shared/models/cv.model';
import { User } from '@shared/models/user.model';
import { BaseService } from '@shared/services/base.service';
import { FacadeService } from '@shared/services/facade.service';
import { NzModalService, NzUploadFile } from 'ng-zorro-antd';


@Component({
  selector: 'app-bulk-add',
  templateUrl: './bulk-add.component.html',
  styleUrls: ['./bulk-add.component.scss']
})
export class BulkAddComponent implements OnInit {

  @ViewChild('dropdown') nameDropdown;
  @ViewChild(CandidateAddComponent) candidateAdd: CandidateAddComponent;

  @Input()
  private _visible: boolean = true;
  public get visibles(): boolean {
    return this._visible;
  }
  public set visibles(value: boolean) {
    this.visible = value;
  }

  @Input() candidateSources = [];

  @Input()
  communities: Community[];
  filteredCommunity: Community[] = [];
  currentUser: User;
  @Output() refreshTableAction = new EventEmitter<Candidate>();

  form: FormGroup = this.fb.group({
    community: [null, [Validators.required]],
    file: [null, [Validators.required]],
    source: [null, [Validators.required]]
  });

  visible = true;
  fileList: NzUploadFile[] = [];
  filename : string = null;
  checkedTerms = false;

  constructor(private fb: FormBuilder, private facade: FacadeService,
    private modalService: NzModalService, private b: BaseService<Cv>, private router: Router) {
    this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
  }

  ngOnInit() {
    if (this.communities) {
      this.filteredCommunity = this.communities.sort((a, b) => (a.name.localeCompare(b.name)));
    }
    this.visible = this._visible;
  }

  saveEdit() {
    let isCompleted;
    if (this.form.invalid) {
      this.checkForm();
      isCompleted = false;
    }
    else {
      isCompleted = true;
    }

    if (isCompleted) {
      const data = new FormData();
      data.append('source', this.form.controls["source"].value);
      data.append('communityId', this.form.controls["community"].value);
      data.append('file', this.form.controls["file"].value);
      this.facade.candidateService.bulkAdd(data)
        .subscribe(res => {
          this.facade.toastrService.success('Candidates were succesfully created!');
          this.modalService.closeAll();
          this.refreshTableAction.emit();
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        });
      }
    }

    checkForm() {
      for (const i in this.form.controls) {
        this.form.controls[i].markAsDirty();
        this.form.controls[i].updateValueAndValidity();
      }
    }

    clearDataAndCloseModal() {
      this.facade.modalService.openModals[0].destroy();
    }

    beforeUpload = (file: NzUploadFile): boolean => {
      let fileExtension = file.name.split('.')[1].toLowerCase()

      if ((fileExtension === 'xls' || fileExtension === 'xlsx') && file.size <= 3000000) {
        this.form.get('file').setValue(file);
        this.filename = file.name;
      } else if (!(fileExtension === 'xls' || fileExtension === 'xlsx')) {
        this.facade.toastrService.error('File must be a Microsoft Excel workbook');
        this.filename = null;
      } else if (!(file.size <= 3000000)) {
        this.facade.toastrService.error('File must be 3MB or less');
        this.filename = null;
      }

      return false;
    };
  }




