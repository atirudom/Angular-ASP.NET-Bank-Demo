import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isLoggedInPage = false;

  constructor(private route: ActivatedRoute) {
  }

  ngOnInit() {
    if (location.pathname == '/secureKinglion/login' || location.pathname == '/') {
      this.isLoggedInPage = true;
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
