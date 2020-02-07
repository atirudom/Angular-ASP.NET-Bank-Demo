import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import * as $ from "jquery";
import { environment } from '../../environments/environment'

@Component({
  selector: 'app-admin-actions-account',
  templateUrl: './admin-actions-account.component.html'
})
export class ActionsAccountComponent {
  public url = { edit: '', back: '' };
  public customer: Customer;
  public accounts: Account[];
  public customerID: string;

  constructor(http: HttpClient, private route: ActivatedRoute) {
    this.customerID = this.route.snapshot.paramMap.get('customerID');
    http.get<Customer>(environment.adminApiUrl + `api/customers/${this.customerID}`).subscribe(result => {
      this.customer = result;
      this.accounts = this.customer.accounts
      this.url.edit = `/admin-actions/${this.customerID}/edit`
      this.url.back = '/all-users'
      console.log(this.customer)
    }, error => console.error(error));
  }

  ngOnInit() { }

  lockUser() {
    $.ajax(environment.adminApiUrl + "api/logins/lock/" + this.customerID, {
      type: 'POST',
      success: (data) => {
        console.log(data);
        this.customer.login.status = 'Locked'
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
