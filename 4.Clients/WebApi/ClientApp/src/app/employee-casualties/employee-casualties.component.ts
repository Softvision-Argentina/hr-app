import { Component, OnInit, TemplateRef } from '@angular/core';
import { EmployeeCasualty } from './../../entities/employeeCasualty';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FacadeService } from '../services/facade.service';
import { AppComponent } from '../app.component';

@Component({
  selector: 'app-employee-casualties',
  templateUrl: './employee-casualties.component.html',
  styleUrls: ['./employee-casualties.component.css']
})
export class EmployeeCasualtiesComponent implements OnInit {

  casualtyForm: FormGroup;
  employeecasualties: EmployeeCasualty[] = [];
  listOfDisplayData = [...this.employeecasualties];

  monthList: string[] = ["JANUARY", "FEBRUARY", "MARCH", "APRIL", "MAY", "JUNE", "JULY", "AUGUST", "SEPTEMBER", "OCTOBER", "NOVEMBER", "DECEMBER"];

  yearList: number[] = [];

  constructor(private fb: FormBuilder, private facade: FacadeService, private app: AppComponent) { }

  ngOnInit() {
    this.casualtyForm = this.fb.group({
      value: [0, [Validators.required]],
      month: [null, [Validators.required]]
    });

    this.getEmployeeCasualties();
  }

  getEmployeeCasualties() {
    this.facade.employeeCasulatyService.get()
      .subscribe(res => {
        this.employeecasualties = res.sort((a, b) => a.month > b.month ? 1 : -1).sort((a, b) => a.year < b.year ? 1 : -1);
        this.listOfDisplayData = res.sort((a, b) => a.month > b.month ? 1 : -1).sort((a, b) => a.year < b.year ? 1 : -1);
        res.forEach(hp => {
          if (this.yearList.filter(yl => yl == hp.year).length == 0) this.yearList.push(hp.year);
        });
      }, err => console.log(err));
  }

  onMonthChange(result: Date) {
    if (result != null) {
      if (this.employeecasualties.filter(h => h.month === result.getMonth() + 1 && h.year === result.getFullYear()).length > 0) {
        this.casualtyForm.controls['month'].setErrors({ 'exists': true });
      }
    }
  }

  exists(errors: any) {
    if (errors != null && errors['exists']) return true;
    else return false;
  }

  showAddModal(modalContent: TemplateRef<{}>): void {
    this.casualtyForm.reset();
    const modal = this.facade.modalService.create({
      nzTitle: 'Add New casualty',
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
            for (const i in this.casualtyForm.controls) {
              this.casualtyForm.controls[i].markAsDirty();
              this.casualtyForm.controls[i].updateValueAndValidity();
              if ((!this.casualtyForm.controls[i].valid)) isCompleted = false;
            }
            if (isCompleted) {
              let selectedDate = new Date(this.casualtyForm.controls['month'].value);
              let month: number = selectedDate.getMonth() + 1;
              let year: number = selectedDate.getFullYear();
              let newCasualty: EmployeeCasualty = {
                id: 0,
                value: this.casualtyForm.controls['value'].value,
                month: month,
                year: year
              }
              this.facade.employeeCasulatyService.add(newCasualty)
                .subscribe(res => {
                  this.getEmployeeCasualties();
                  this.app.hideLoading();
                  this.facade.toastrService.success("Casualty was successfuly created !");
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
    this.casualtyForm.reset();
    let editedCasualty: EmployeeCasualty = this.employeecasualties.filter(casualty => casualty.id == id)[0];
    this.casualtyForm.controls['value'].setValue(editedCasualty.value);
    this.casualtyForm.controls['month'].setValue(new Date(editedCasualty.year + '-' + editedCasualty.month));
    const modal = this.facade.modalService.create({
      nzTitle: 'Edit Casualty',
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
            for (const i in this.casualtyForm.controls) {
              this.casualtyForm.controls[i].markAsDirty();
              this.casualtyForm.controls[i].updateValueAndValidity();
              if ((!this.casualtyForm.controls[i].valid)) isCompleted = false;
            }
            if (isCompleted) {
              let selectedDate = new Date(this.casualtyForm.controls['month'].value);
              let month: number = selectedDate.getMonth() + 1;
              let year: number = selectedDate.getFullYear();
              editedCasualty = {
                id: editedCasualty.id,
                value: this.casualtyForm.controls['value'].value,
                month: month,
                year: year
              }
              this.facade.employeeCasulatyService.update(editedCasualty.id, editedCasualty)
                .subscribe(res => {
                  this.getEmployeeCasualties();
                  this.app.hideLoading();
                  this.facade.toastrService.success('Casualty was successfully edited !');
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

  showDeleteConfirm(employeeCasualtyId: number): void {
    let employeeCasualtyDelete: EmployeeCasualty = this.employeecasualties.find(employeeCasualty => employeeCasualty.id == employeeCasualtyId);
    this.facade.modalService.confirm({
      nzTitle: 'Are you sure to delete ' + this.monthList[employeeCasualtyDelete.month - 1] + ' of ' + employeeCasualtyDelete.year + ' ?',
      nzContent: 'This action will delete the casualty associated with this month',
      nzOkText: 'Yes',
      nzOkType: 'danger',
      nzCancelText: 'No',
      nzOnOk: () => this.facade.employeeCasulatyService.delete(employeeCasualtyId)
        .subscribe(res => {
          this.getEmployeeCasualties();
          this.facade.toastrService.success('Casualty was deleted !');
        }, err => {
          this.facade.errorHandlerService.showErrorMessage(err);
        })
    });
  }

  searchYear(year: number) {
    if (year == 0) this.listOfDisplayData = this.employeecasualties;
    else {
      this.listOfDisplayData = this.employeecasualties.filter(hp => hp.year == year);
    }
  }

  getMonth(casualty: EmployeeCasualty): string {
    var month: number = casualty.month;
    return this.monthList[month - 1];
  }
}