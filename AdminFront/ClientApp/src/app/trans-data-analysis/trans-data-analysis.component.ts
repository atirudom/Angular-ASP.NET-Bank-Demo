import { Component, Inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import * as $ from "jquery";
import { environment } from '../../environments/environment'
import { FormBuilder, FormGroup } from '@angular/forms';

import { Chart } from "chart.js";
import * as moment from "moment";

@Component({
  selector: 'app-trans-data-analysis',
  templateUrl: './trans-data-analysis.component.html'
})
export class TransDataAnalysisComponent {
  public url = { edit: '', back: '' };
  public ansResult: TransactionDateAns;
  public customerID: number;
  public fromDate: Date;
  public toDate: Date;
  public chartType: string;

  chartForm: FormGroup;

  constructor(http: HttpClient, private route: ActivatedRoute, private _fb: FormBuilder) {
    this.route.queryParams.subscribe(params => {
      this.customerID = params['customerID'];
      this.fromDate = params['fromDate'];
      this.toDate = params['toDate'];
      console.log(params);

      // Create Form
      this.chartForm = this._fb.group({
        customerID: this.customerID,
        fromDate: this.fromDate,
        toDate: this.toDate,
        chartType: this.chartType
      });

    });

    let body = {
      customerID: this.customerID,
      fromDate: this.fromDate,
      toDate: this.toDate
    }

    http.post<TransactionDateAns>(environment.adminApiUrl + `api/transactions/GetFromCustomer/${this.customerID || ""}`, body,
      {
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      }
    ).subscribe(result => {
      this.ansResult = result;
      console.log(result, this.ansResult)
      this.buildTransactionChart(this.ansResult);
    }, error => console.error(error));
  }

  ngOnInit() { }

  buildTransactionChart(transactionDates) {
    var canvas = document.getElementById("transactionChart");

    const labels = [];
    const data = [];
    for (let x of transactionDates) {
      labels.push(x.date);
      data.push({ t: moment(x.date, "DD/MM/YYYY").toDate(), y: x.transactionCount });
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

}

interface TransactionDateAns {
  date: string;
  transactionCount: number;
  totalAmount: number;
}
