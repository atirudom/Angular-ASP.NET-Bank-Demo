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
  public customerID: string;

  constructor(http: HttpClient, private route: ActivatedRoute) {
    this.customerID = this.route.snapshot.paramMap.get('customerID');
    http.get<Customer>("http://localhost:63637/" + `api/customers/${this.customerID}`).subscribe(result => {
      this.customer = result;
      this.url.edit = `/admin-actions/${this.customerID}/edit`
      this.url.back = '/all-users'
      console.log()
      console.log(this.customer)
    }, error => console.error(error));
  }

  ngOnInit() { }

  lockUser() {
    $.ajax("http://localhost:63637/" + "api/logins/lock/" + this.customerID, {
      type: 'POST',
      success: (data) => {
        console.log(data);
        this.customer.Login.Status = 'Locked'
      }
    })
  }

  onClickViewCustomer(data) {
    //this._router.navigate(["/view-user"])
  }
}

interface Login {
  CustomerID: number;
  UserID: string;
  LockUntilTime: Date;
  Status: string;
}

interface Customer {
  CustomerID: number;
  Name: string;
  TFN: string;
  Address: string;
  City: string;
  State: string;
  PostCode: string;
  Phone: string;
  Login: Login;
}
