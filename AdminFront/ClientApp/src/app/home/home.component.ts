import { Component, OnInit } from '@angular/core';
import * as $ from "jquery";
import { Router, ActivatedRoute } from '@angular/router';
import { environment } from '../../environments/environment'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls:['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private _router: Router, private activeRoute: ActivatedRoute) {
    // force route reload whenever params change;
    this._router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  login: { UserID, Password } = { UserID: "", Password: "" };
  invalidLogin = false;

  ngOnInit() {
  }

  loginAdmin() {
    $.ajax(environment.adminApiUrl + "api/Admin/KingLionSecLogin", {
      data: JSON.stringify(this.login),
      contentType: 'application/json',
      type: 'POST',
      success: (data) => {
        console.log(data);
        if (data.loginSuccess) {
          window.location.href = "/all-users";
        } else {
          this.invalidLogin = true
        }
      }
    })
  }
}
