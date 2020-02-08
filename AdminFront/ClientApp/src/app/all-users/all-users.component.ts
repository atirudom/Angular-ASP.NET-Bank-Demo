import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Component({
  selector: 'app-all-users',
  templateUrl: './all-users.component.html',
  styleUrls: ['./all-users.component.css']
})
export class AllUsersComponent {
  public customers: Customer[];

  constructor(http: HttpClient, private _router: Router) {
    http.get<Customer[]>(environment.adminApiUrl + 'api/customers').subscribe(result => {
      this.customers = result;
      console.log(this.customers)
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

