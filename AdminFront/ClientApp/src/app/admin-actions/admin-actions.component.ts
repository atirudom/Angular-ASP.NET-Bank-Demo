import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import * as $ from "jquery";

@Component({
  selector: 'app-admin-actions',
  templateUrl: './admin-actions.component.html'
})
export class AdminActionsComponent {
  public url = { edit: '', back: '' };
  public customer: Customer;
  public accounts: Account[];
  public billPays: BillPay[];
  public customerID: string;

  constructor(http: HttpClient, private route: ActivatedRoute) {
    this.customerID = this.route.snapshot.paramMap.get('customerID');
    http.get<Customer>("http://localhost:63637/" + `api/customers/${this.customerID}`).subscribe(result => {
      this.customer = result;
      this.accounts = this.customer.accounts
      this.url.edit = `/admin-actions/${this.customerID}/edit`
      this.url.back = '/all-users'
      console.log()
      console.log(this.customer)
    }, error => console.error(error));
  }

  ngOnInit() { }

  viewBill(accNumber) {
    $.ajax("http://localhost:63637/" + "api/BillPays/FromAccount/" + accNumber, {
      success: (data) => {
        console.log(data);
        this.billPays = data
      }
    })
  }

  lockUser() {
    $.ajax("http://localhost:63637/" + "api/logins/lock/" + this.customerID, {
      type: 'POST',
      success: (data) => {
        console.log(data);
        this.customer.login.status = 'Locked'
      }
    })
  }

  blockBillPay(billPayID) {
    console.log(billPayID);

    $.ajax("http://localhost:63637/" + "api/BillPays/block/" + billPayID, {
      type: 'PUT',
      success: (data) => {
        console.log(data);

        // Does not trigger update
        //this.billPays.forEach(bill => {
        //  if (bill.billPayID == billPayID) bill.status = 'Normal'
        //})
      }
    })
  }

  unblockBillPay(billPayID) {
    console.log(billPayID);
    $.ajax("http://localhost:63637/" + "api/BillPays/unblock/" + billPayID, {
      type: 'PUT',
      success: (data) => {
        console.log(data);

        //this.billPays.forEach(bill => {
        //  if (bill.billPayID == billPayID) bill.status = 'Blocked'
        //})
      }
    })
  }
}

interface Login {
  customerID: number;
  userID: string;
  lockUntilTime: Date;
  status: string;
}

interface Customer {
  customerID: number;
  name: string;
  tfn: string;
  address: string;
  city: string;
  state: string;
  postCode: string;
  phone: string;
  login: Login;
  accounts: Account[];
}

interface Account {
  accountNumber: number;
  accountType: string;
  customerID: number;
  balance: number;
}

interface BillPay {
  billPayID: number;
  accountNumber: number;
  amount: number;
  scheduleDate: Date;
  status: string;
}
