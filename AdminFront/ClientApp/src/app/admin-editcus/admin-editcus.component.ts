import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import * as $ from "jquery";
import { environment } from '../../environments/environment'

@Component({
  selector: 'app-admin-editcus',
  templateUrl: './admin-editcus.component.html'
})
export class AdminEditCusComponent {
  customerForm: FormGroup;
  sendSuccess = false;

  public url = { edit: '', back: '' };
  public customer: Customer;
  public customerID: number;

  constructor(private _fb: FormBuilder, http: HttpClient, private route: ActivatedRoute, private _router: Router) {
    this.customerID = parseInt(this.route.snapshot.paramMap.get('customerID'));
    http.get<Customer>(environment.adminApiUrl + `api/customers/${this.customerID}`).subscribe(result => {
      this.customer = result;
      this.url.back = `/admin-actions/${this.customerID}`
      console.log(this.customer)

      let cus = this.customer
      this.customerForm = this._fb.group({
        customerID: this.customerID,
        name: [cus.name, [Validators.required]],
        tfn: [cus.tfn],
        address: [cus.address, [Validators.required]],
        city: [cus.city, [Validators.required]],
        state: cus.state,
        postCode: cus.postCode,
        phone: cus.phone,
      });

    }, error => console.error(error));
  }

  ngOnInit() {
    
  }

  save() {
    if (!this.customerForm.valid) {
      return;
    }
    console.log(this.customerForm.value);
    $.ajax(environment.adminApiUrl + "api/customers/" + this.customerID, {
      data: JSON.stringify(this.customerForm.value),
      contentType: 'application/json',
      type: 'PUT',
      success: (data) => {
        console.log(data);
        if (data.success) {
          this._router.navigate([this.url.back]);
        } else {
          this.sendSuccess = false;
        }
      }
    })
  }

  cancel() {
    this._router.navigate([this.url.back]);
  }

  get name() {
    return this.customerForm.get('name')
  }
  get address() {
    return this.customerForm.get('address')
  }
  get city() {
    return this.customerForm.get('city')
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
}
