import { Component, Inject, ElementRef } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import * as $ from "jquery";
import { environment } from '../../environments/environment'
import { FormBuilder, FormGroup, AbstractControl } from '@angular/forms';

import { Chart } from "chart.js";
import * as moment from "moment";

@Component({
  selector: 'app-trans-data-analysis',
  templateUrl: './trans-data-analysis.component.html'
})
export class TransDataAnalysisComponent {
  public url = { edit: '', back: '' };
  public ansResult;
  public customerID: number;
  public fromDate: Date;
  public toDate: Date;
  //public chartType: string;
  public chartType: string;

  chartForm: FormGroup;
  customerDropdown: CustomerDropdown[];
  fromDateErrMessage: string;

  constructor(private http: HttpClient, private route: ActivatedRoute,
    private _fb: FormBuilder, private _router: Router, private elementRef: ElementRef) {
    this.route.queryParams.subscribe(params => {
      this.customerID = parseInt(params['customerID']);
      this.fromDate = params['fromDate'];
      this.toDate = params['toDate'];
      this.chartType = params['chartType'] || "transactionCount";

      // Create Form
      this.chartForm = this._fb.group({
        customerID: this.customerID || null,
        fromDate: this.fromDate,
        toDate: this.toDate,
        chartType: this.chartType || "transactionCount"
      });

      let body = {
        customerID: this.customerID,
        fromDate: this.fromDate,
        toDate: this.toDate
      }

      let actionUrl = this.chartType == "amountPerType" ? "GetAnsAmtType" : "GetAnsDailyData"
      http.post(environment.adminApiUrl + `api/transactions/${actionUrl}`, body,
        {
          headers: new HttpHeaders({
            'Content-Type': 'application/json'
          })
        }
      ).subscribe(result => {
        this.ansResult = result;
        console.log(result, this.ansResult)
        this.buildTransactionChart(this.ansResult, this.chartType);
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
      fromDate: this.chartForm.value.fromDate,
      toDate: this.chartForm.value.toDate,
      chartType: this.chartForm.value.chartType,
    }
    console.log(location.pathname)
    console.log(queryParams)

    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
    this._router.onSameUrlNavigation = 'reload';
    this._router.navigate(
      [location.pathname],
      {
        //relativeTo: this.route,
        queryParams: queryParams,

      }
    )

    //this._router.navigate(
    //  [location.pathname],
    //  {
    //    //relativeTo: this.route,
    //    queryParams: queryParams,

    //  }
    //)
  }

  buildTransactionChart(resultsAns, chartType) {
    var canvas = document.getElementById("transactionChart");

    switch (chartType) {
      case "transactionCount":
        buildTransactionCountChart(resultsAns, canvas)
        break;
      case "totalAmount":
        buildTransTotalAmountChart(resultsAns, canvas)
        break;
      case "amountPerType":
        buildAmountPerTypeChart(resultsAns, canvas)
        break;
    }

  }

}

function buildTransactionCountChart(transactionDates, canvas) {

  const labels = [];
  const data = [];
  for (let x of transactionDates) {
    labels.push(x.date);
    data.push({
      t: moment(x.date, "DD/MM/YYYY").toDate(), y: x.transactionCount
    });
  }

  const transactionChart = new Chart(canvas, {
    type: "bar",
    data: {
      labels: labels,
      datasets: [{
        label: "Transaction count",
        data: data,
        options: {
          scales: {
            xAxes: [{
              type: "time",
              time: {
                unit: "day"
              }
            }]
          }
        },
        backgroundColor: "rgba(255, 99, 132, 0.2)",
        borderColor: "rgba(255, 99, 132, 1)",
        borderWidth: 1
      }]
    }
  });
}

function buildTransTotalAmountChart(transactionDates, canvas) {
  const labels = [];
  const data = [];
  for (let x of transactionDates) {
    labels.push(x.date);
    data.push({
      t: moment(x.date, "DD/MM/YYYY").toDate(), y: x.totalAmount
    });
  }

  const transactionChart = new Chart(canvas, {
    type: "line",
    data: {
      labels: labels,
      datasets: [{
        label: "Total amount",
        data: data,
        options: {
          scales: {
            xAxes: [{
              type: "time",
              time: {
                unit: "day"
              }
            }]
          }
        },
        backgroundColor: "rgba(255, 99, 132, 0.2)",
        borderColor: "rgba(255, 99, 132, 1)",
        borderWidth: 1
      }]
    }
  });
}

function buildAmountPerTypeChart(resultsAns, canvas) {
  const labels = [];
  const data = [];
  for (let r of resultsAns) {
    labels.push(r.type);
    data.push(r.totalAmount);
  }

  const transactionChart = new Chart(canvas, {
    type: "doughnut",
    data: {
      labels: labels,
      datasets: [{
        data: data,
        backgroundColor: [
          "rgba(255, 99, 123, 0.8)",
          "rgba(3, 99, 1, 0.8)",
          "rgba(233, 123, 22, 0.8)",
          "rgba(32, 123, 175, 0.8)",
          "rgba(255, 5, 22, 0.8)"
        ]
      }],
    }
  });
}

function getErrorDateValidation(fromDate, toDate) {
  var diff = moment(fromDate).diff(moment(toDate) || moment())
  var isFromBeforeTo = diff <= 0 ? true : false
  const errMsg = isFromBeforeTo ? null : "Starting Date must be before End Date"
  return errMsg
}

interface TransactionDateAns {
  date: string;
  transactionCount: number;
  totalAmount: number;
}

interface CustomerDropdown {
  customerID: number;
  name: string;
}
