import { SystemConstants } from './../../core/common/system.constants';
import { ProjectTypesConstants } from './../../core/common/projectType.constants';
import { Response } from '@angular/http';
import { NotificationService } from './../../core/services/notification.service';
import { DataService } from './../../core/services/data.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DatePipe } from '@angular/common';

declare var moment : any;

@Component({
  selector: 'app-quarter',
  templateUrl: './quarter.component.html',
  styleUrls: ['./quarter.component.css']
})
export class QuarterComponent implements OnInit {

  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  public quarters: any[];
  public quarter: any;
  public isClicked: boolean;
  public queryResult: any = {};

  isLoadData: boolean;
  isLoading: boolean;
  query: any = {
    pageSize: SystemConstants.PAGE_SIZE
  };

  constructor(private _dataService: DataService, private _notificationService: NotificationService) {
    this.isClicked = false;
    this.isLoadData = false;
    this.isLoading = false;
  }

  public dateOptions: any = {
    locale: { format: 'DD/MM/YYYY' },
    alwaysShowCalendars: false,
    singleDatePicker: true
  };

  ngOnInit() {
    this.loadData();
  }

  handler(type: string, $event: ModalDirective) {
    if (type === "onHide" || type === "onHidden") {
      this.quarter = [];
      this.isLoading = false;
    }
  }

  loadData() {
    this._dataService.get("/api/quarters/getall" + "?" + this.toQueryString(this.query)).subscribe((response: any) => {
      this.queryResult = response;
      this.isLoadData = true;
    });
  }

  public selectedStartDate(value: any) {
    this.quarter.quarterStart = moment(value.end._d).format('YYYY/MM/DD');
  }
  public selectedEndDate(value: any) {
    this.quarter.quarterEnd = moment(value.end._d).format('YYYY/MM/DD');
  }

  //Create method
  showAddModal() {
    this.quarter = {};
    this.isLoading = true;
    this.modalAddEdit.show();
  }

  //Edit method
  showEditModal(id: any) {
    this.loadquarter(id);
    this.modalAddEdit.show();
  }

  //Get Role with Id
  loadquarter(id: any) {
    this._dataService.get('/api/quarters/getquarter/' + id)
      .subscribe((response: any) => {
        this.quarter = response;
        this.isLoading = true;
      });
  }

  saveChange(valid: boolean) {
    if (valid) {
      this.isClicked = true;
      console.log(this.quarter);
      if (this.quarter.quarterId == undefined) {
        this._dataService.post('/api/quarters/add', JSON.stringify(this.quarter))
          .subscribe((response: any) => {
            this.loadData();
            this.modalAddEdit.hide();
            this._notificationService.printSuccessMessage("Add Success");
            this.isClicked = false;
          }, error => this._dataService.handleError(error));
      }
      else {
        this._dataService.put('/api/quarters/update/' + this.quarter.quarterId, JSON.stringify(this.quarter))
          .subscribe((response: any) => {
            this.loadData();
            this.modalAddEdit.hide();
            this._notificationService.printSuccessMessage("Update Success");
            this.isClicked = false;
          }, error => this._dataService.handleError(error));
      }
    }
  }

  deleteQuarter(id: any) {
    this._notificationService.printConfirmationDialog("Delete confirm", () => this.deleteConfirm(id));
  }

  deleteConfirm(id: any) {
    this._dataService.delete('/api/quarters/delete/' + id)
      .subscribe((response: Response) => {
        this._notificationService.printSuccessMessage("Delete Success");
        this.loadData();
      });
  }

  toQueryString(obj) {
    var parts = [];
    for (var property in obj) {
      var value = obj[property];
      if (value != null && value != undefined)
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
    }

    return parts.join('&');
  }
  onPageChange(page) {
    this.isLoadData = false;
    this.query.page = page;
    this.loadData();
  }

}
