import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html'
})
export class AllUsersComponent {
  public logins: Login[];

  constructor(http: HttpClient, private _router: Router) {
    http.get<Login[]>("http://localhost:63637/" + 'api/logins').subscribe(result => {
      this.logins = result;
      console.log(this.logins)
    }, error => console.error(error));
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
  Customer: Customer;
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
}

