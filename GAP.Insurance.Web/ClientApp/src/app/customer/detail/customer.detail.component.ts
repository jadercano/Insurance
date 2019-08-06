import { Component, Input } from '@angular/core';
import { Customer } from '../../domain/customer.domain';

@Component({
  selector: 'app-customer-detail',
  templateUrl: './customer.detail.component.html'
})
export class CustomerDetailComponent {

  @Input() customer: Customer;

}
