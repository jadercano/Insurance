import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HttpModule, RequestOptions } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { RefreshComponent } from './home/refresh.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { CustomerComponent } from './customer/customer.component';
import { CustomerDetailComponent } from './customer/detail/customer.detail.component';
import { InsuranceComponent } from './insurance/insurance.component';
import { CustomerInsuranceComponent } from './customerinsurance/customerinsurance.component';

// Services
import { CustomRequestOptions } from './api/api';
import { CustomerService } from './api/services/customer.service';
import { InsuranceService } from './api/services/insurance.service';

// Prime modules
import { TableModule } from 'primeng/table';
import { DialogModule } from 'primeng/dialog';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { CalendarModule } from 'primeng/calendar';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { FieldsetModule } from 'primeng/fieldset';

import { MessageService, ConfirmationService } from 'primeng/api';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    RefreshComponent,
    CounterComponent,
    FetchDataComponent,
    CustomerComponent, 
    CustomerDetailComponent,
    InsuranceComponent,
    CustomerInsuranceComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'refresh', component: RefreshComponent },
      { path: 'customer', component: CustomerComponent },
      { path: 'insurance', component: InsuranceComponent },
      { path: 'customer/:customerId', component: CustomerDetailComponent }
    ]),
    TableModule,
    InputTextModule,
    ButtonModule,
    DialogModule,
    ToastModule,
    ConfirmDialogModule,
    CalendarModule,
    DropdownModule,
    InputTextareaModule,
    FieldsetModule
  ],
  providers: [
    { provide: RequestOptions, useClass: CustomRequestOptions },
    CustomerService,
    InsuranceService,
    MessageService,
    ConfirmationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
