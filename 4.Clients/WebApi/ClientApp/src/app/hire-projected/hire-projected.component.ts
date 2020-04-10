import { Component, OnInit, TemplateRef } from '@angular/core';
import { HireProjection } from './../../entities/hireProjection';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FacadeService } from '../services/facade.service';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-hire-projected',
  templateUrl: './hire-projected.component.html',
  styleUrls: ['./hire-projected.component.css']
})
export class HireProjectedComponent implements OnInit {

  projectionForm: FormGroup;
  hireProjections: HireProjection[] = [];
  listOfDisplayData = [...this.hireProjections];
  monthList: string[] = ["JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"];
  yearList: number[] = [];

  constructor(private fb: FormBuilder, private facade: FacadeService, private app: AppComponent) { }

  ngOnInit() {
    this.projectionForm = this.fb.group({
      value: [0, [Validators.required]],
      month: [null, [Validators.required]]
    });
    this.getHireProjections();
  }

  getHireProjections() {
    this.facade.hireProjectionService.get()
      .subscribe(res => {
        this.hireProjections = res.sort((a, b) => a.month > b.month ? 1 : -1).sort((a, b) => a.year < b.year ? 1 : -1);
        this.listOfDisplayData = res.sort((a, b) => a.month > b.month ? 1 : -1).sort((a, b) => a.year < b.year ? 1 : -1);
        res.forEach(hp => {
          if (this.yearList.filter(yl => yl === hp.year).length === 0) this.yearList.push(hp.year);
        });
      }, err => this.facade.errorHandlerService.showErrorMessage(err));
  }

  onMonthChange(result: Date) {
    if (result !== null) {
      if (this.hireProjections.filter(h => h.month === result.getMonth() + 1 && h.year === result.getFullYear()).length > 0) {
        this.projectionForm.controls['month'].setErrors({ 'exists': true });
      }
    }
  }

  exists(errors: any) {
    if (errors !== null && errors['exists']) return true;
    else return false;
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.projectionForm.reset();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New Projection',
      nzContent: modalContent,
      nzClosable: true,
      nzFooter: [
        { label: 'Cancel', shape: 'default', onClick: () => modal.destroy() },
        {
          label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            this.app.showLoading();
            modal.nzFooter[1].loading = true;
            let isCompleted: boolean = true;
            for (const i in this.projectionForm.controls) {
              this.projectionForm.controls[i].markAsDirty();
              this.projectionForm.controls[i].updateValueAndValidity();
              if ((!this.projectionForm.controls[i].valid)) isCompleted = false;
            }
            if (isCompleted) {
              let selectedDate = new Date(this.projectionForm.controls['month'].value);
              let month: number = selectedDate.getMonth() + 1;
              let year: number = selectedDate.getFullYear();
              let newProjection: HireProjection = {
                id: 0,
                value: this.projectionForm.controls['value'].value,
                month: month,
                year: year
              }
              this.facade.hireProjectionService.add(newProjection)
                .subscribe(res => {
                  this.getHireProjections();
                  this.app.hideLoading();
                  this.facade.toastrService.success("Projection was successfuly created !");
                  modal.destroy();
                }, err => {
                  this.app.hideLoading();
                  modal.nzFooter[1].loading = false;
                  this.facade.errorHandlerService.showErrorMessage(err);
                })
            }
            else modal.nzFooter[1].loading = false;
            this.app.hideLoading();
          }
        }],
    });
  }

  showEditModal(modalContent: TemplateRef<{}>, id: number): void {
    this.projectionForm.reset();
    let editedProjection: HireProjection = this.hireProjections.filter(projection => projection.id === id)[0];
    this.projectionForm.controls['value'].setValue(editedProjection.value);
    this.projectionForm.controls['month'].setValue(new Date(editedProjection.year + '-' + editedProjection.month));
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Projection',
      nzContent: modalContent,
      nzClosable: true,
      nzWrapClassName: 'vertical-center-modal',
      nzFooter: [
        { label: 'Cancel', shape: 'default', onClick: () => modal.destroy() },
        {
          label: 'Save', type: 'primary', loading: false,
          onClick: () => {
            this.app.showLoading();
            modal.nzFooter[1].loading = true;
            let isCompleted: boolean = true;
            for (const i in this.projectionForm.controls) {
              this.projectionForm.controls[i].markAsDirty();
              this.projectionForm.controls[i].updateValueAndValidity();
              if ((!this.projectionForm.controls[i].valid)) isCompleted = false;
            }
            if (isCompleted) {
              let selectedDate = new Date(this.projectionForm.controls['month'].value);
              let month: number = selectedDate.getMonth() + 1;
              let year: number = selectedDate.getFullYear();
              editedProjection = {
                id: editedProjection.id,
                value: this.projectionForm.controls['value'].value,
                month: month,
                year: year
              }
              this.facade.hireProjectionService.update(editedProjection.id, editedProjection)
                .subscribe(res => {
                  this.getHireProjections();
                  this.app.hideLoading();
                  this.facade.toastrService.success('Projection was successfully edited !');
                  modal.destroy();
                }, err => {
                  this.app.hideLoading();
                  modal.nzFooter[1].loading = false;
                  this.facade.errorHandlerService.showErrorMessage(err);
                })
            }
            else modal.nzFooter[1].loading = false;
            this.app.hideLoading();
          }
        }],
    });
  }

  showDeleteConfirm(hireProjectionId: number): void {
    let hireProjectionDelete: HireProjection = this.hireProjections.find(hireProjection => hireProjection.id === hireProjectionId);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to delete ' + this.monthList[hireProjectionDelete.month - 1] + ' of ' + hireProjectionDelete.year + ' ?',
      nzContent: 'This action will delete the projection associated with this month',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.hireProjectionService.delete(hireProjectionId)
        .subscribe(res => {
          this.getHireProjections();
          this.facade.toastrService.success('Projection was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  searchYear(year: number) {
    if (year === 0) this.listOfDisplayData = this.hireProjections;
    else {
      this.listOfDisplayData = this.hireProjections.filter(hp => hp.year === year);
    }
  }

  getMonth(projection: HireProjection): string {
    var month: number = projection.month;
    return this.monthList[month - 1];
  }

}