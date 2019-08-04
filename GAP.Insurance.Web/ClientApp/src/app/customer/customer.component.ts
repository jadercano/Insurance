import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html'
})
export class CustomerComponent {
  public customers: Customer[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Customer[]>('http://localhost:3025/api/Customer').subscribe(result => {
      this.customers = result;
    }, error => console.error(error));
  }
}

interface Customer {
  name: string;
  email: string;
}
