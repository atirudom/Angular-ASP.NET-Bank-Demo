import { Component, OnInit } from '@angular/core';
import * as $ from "jquery";
import { Router } from '@angular/router';
import { environment } from '../../environments/environment'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {

  constructor(private _router: Router) {

  }

  login: { UserID, Password } = { UserID: "", Password: "" };
  invalidLogin = false;

  ngOnInit() {
  }

  loginAdmin() {
    $.ajax(environment.adminApiUrl + "api/Admin/Login", {
      data: JSON.stringify(this.login),
      contentType: 'application/json',
      type: 'POST',
      success: (data) => {
        console.log(data);
        if (data.loginSuccess) {
          this._router.navigate(["/all-users"]);
        } else {
          this.invalidLogin = true
        }
      }
    })
  }
}
