import { Component, Inject, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import * as $ from "jquery";
import { environment } from '../../environments/environment'
import { FormBuilder, FormGroup, AbstractControl } from '@angular/forms';

import * as moment from "moment";

@Component({
  selector: 'app-trans-history',
  templateUrl: './trans-history.component.html'
})
export class TransHistoryComponent {
  public url = { edit: '', back: '' };
  public resultTransaction;
  public customerID: number;
  public fromDate: Date;
  public toDate: Date;

  public totalAmount = 0;
  public totalCount = 0;
  public numOfDays = 0;

  chartForm: FormGroup;
  customerDropdown: CustomerDropdown[];
  fromDateErrMessage: string;

  page = 1;
  pageSize = 50;

  constructor(private http: HttpClient, private route: ActivatedRoute,
    private _fb: FormBuilder, private _router: Router, private elementRef: ElementRef) {
    this.route.queryParams.subscribe(params => {
      this.customerID = parseInt(params['customerID']);
      this.fromDate = params['fromDate'];
      this.toDate = params['toDate'];

      this.numOfDays = getDayDiff(this.fromDate, this.toDate)

      // Create Form
      this.chartForm = this._fb.group({
        customerID: this.customerID || null,
        fromDate: this.fromDate,
        toDate: this.toDate,
      });

      let body = {
        customerID: this.customerID,
        fromDate: this.fromDate,
        toDate: this.toDate
      }

      http.post<Transaction[]>(environment.adminApiUrl + `api/transactions/GetSpecific`, body,
        {
          headers: new HttpHeaders({
            'Content-Type': 'application/json'
          })
        }
      ).subscribe(result => {
        this.resultTransaction = result;
        this.totalCount = result.length;
        result.forEach(t => {
          this.totalAmount += t.amount
        })

        console.log(this.resultTransaction)
      }, error => console.error(error));

    });
  }

  ngOnInit() {
    this.http.get<CustomerDropdown[]>(environment.adminApiUrl + 'api/customers').subscribe(result => {
      this.customerDropdown = result;
      console.log(this.customerDropdown)
    }, error => console.error(error));

    this.chartForm.statusChanges.subscribe(input => {
      this.fromDateErrMessage = getErrorDateValidation(this.chartForm.value.fromDate, this.chartForm.value.toDate)
    })
  }

  ngOndestroy() {
    this.elementRef.nativeElement.remove();
  }

  save() {
    if (getErrorDateValidation(this.chartForm.value.fromDate, this.chartForm.value.toDate)) {
      return
    }
    if (!this.chartForm.valid) {
      return;
    }
    console.log(this.chartForm.value)

    const queryParams = {
      customerID: this.chartForm.value.customerID,
      fromDate: this.chartForm.value.fromDate == "" ? null : this.chartForm.value.fromDate,
      toDate: this.chartForm.value.toDate == "" ? null : this.chartForm.value.toDate,
      chartType: this.chartForm.value.chartType,
    }
    console.log(location.pathname)
    console.log(queryParams)

    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate(
      [location.pathname],
      {
        queryParams: queryParams,
      }
    )
  }
}

function getErrorDateValidation(fromDate, toDate) {
  if (!fromDate || !toDate) {
    return
  }
  var diff = moment(fromDate).diff(moment(toDate) || moment())
  var isFromBeforeTo = diff <= 0 ? true : false
  const errMsg = isFromBeforeTo ? null : "Starting Date must be before End Date"
  return errMsg
}

export function getDayDiff(fromDate, toDate) {
  const fromD = new Date(fromDate);
  const toD = new Date(toDate)
  const diffTime = Math.abs(toD.getTime() - fromD.getTime())
  const diffDays = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
  return diffDays;
}

interface CustomerDropdown {
  customerID: number;
  name: string;
}

interface Transaction {
  transactionID: number;
  transactionType: string;
  accountNumber: number;
  destinationAccountNumber: number;
  amount: number;
  comment: string;
  transactionTime: Date;
  transactionTimeStr: string;
}
