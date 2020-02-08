import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AllUsersComponent } from './all-users/all-users.component';
import { AdminActionsComponent } from './admin-actions/admin-actions.component';
import { AdminEditCusComponent } from './admin-editcus/admin-editcus.component';
import { ActionsAccountComponent } from './admin-actions-account/admin-actions-account.component';
import { TransDataAnalysisComponent } from './trans-data-analysis/trans-data-analysis.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AllUsersComponent,
    AdminActionsComponent,
    AdminEditCusComponent,
    ActionsAccountComponent,
    TransDataAnalysisComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: 'secureKinglion/login', component: HomeComponent, pathMatch: 'full' },
      { path: 'all-users', component: AllUsersComponent },
      { path: 'admin-actions/:customerID', component: AdminActionsComponent },
      { path: 'admin-actions/:customerID/edit', component: AdminEditCusComponent },
      { path: 'admin-actions/account/:accountNumber', component: ActionsAccountComponent },
      { path: 'trans-data-analysis', component: TransDataAnalysisComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
