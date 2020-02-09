import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AllUsersComponent } from './all-users/all-users.component';
import { AdminActionsComponent } from './admin-actions/admin-actions.component';
import { AdminEditCusComponent } from './admin-editcus/admin-editcus.component';
import { TransDataAnalysisComponent } from './trans-data-analysis/trans-data-analysis.component';
import { TransHistoryComponent } from './trans-history/trans-history.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AllUsersComponent,
    AdminActionsComponent,
    AdminEditCusComponent,
    TransDataAnalysisComponent,
    TransHistoryComponent
  ],
  imports: [
    NgbModule,
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: 'secureKinglion/login', component: HomeComponent, pathMatch: 'full' },
      { path: 'all-users', component: AllUsersComponent },
      { path: 'admin-actions/:customerID', component: AdminActionsComponent },
      { path: 'admin-actions/:customerID/edit', component: AdminEditCusComponent },
      { path: 'trans-data-analysis', component: TransDataAnalysisComponent },
      { path: 'trans-history', component: TransHistoryComponent },
      { path: '**', redirectTo: '/404' }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
