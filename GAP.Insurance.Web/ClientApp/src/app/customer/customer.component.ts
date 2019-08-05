import { Component, OnInit } from '@angular/core';
import { Customer } from '../domain/customer.domain';
import { CustomerService } from '../api/services/customer.service';
import { MessageService, ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html'
})
export class CustomerComponent implements OnInit {

  displayDialog: boolean;

  customer: Customer = {} as Customer;

  selectedCustomer: Customer;

  newCustomer: boolean;

  customers: Customer[];

  cols: any[];

  constructor(private customerService: CustomerService, private messageService: MessageService, private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.loadCustomers();

    this.cols = [
      { field: 'name', header: 'Name' },
      { field: 'email', header: 'Email' }
    ];
  }

  loadCustomers() {
    this.customerService.getCustomers().then(customers => this.customers = customers);
  }

  showDialogToAdd() {
    this.newCustomer = true;
    this.customer = {} as Customer;
    this.displayDialog = true;
  }

  save() {
    this.customerService.save(this.customer)
      .then(data => {
        this.customer = null;
        this.displayDialog = false;
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Customer has been saved.' });
      })
      .then(this.loadCustomers.bind(this))
      .catch((error) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error });
      });
  }

  edit(customer: Customer) {
    this.newCustomer = false;
    this.customer = this.cloneCustomer(customer);
    this.displayDialog = true;
  }

  delete(customer: Customer) {
    this.customer = customer;

    this.confirmationService.confirm({
      message: 'Are you sure?',
      accept: this.deleteConfirmed.bind(this),
      reject: () => {
        this.customer = null;
      }
    });

  }

  deleteConfirmed() {
    this.customerService.delete(this.customer.customerId)
      .then(() => {
        this.customer = null;
        this.displayDialog = false;
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Customer has been deleted.' });
      })
      .then(this.loadCustomers.bind(this));
  }

  onRowSelect(event) {
    this.newCustomer = false;
    this.customer = this.cloneCustomer(event.data);
    this.displayDialog = true;
  }

  cloneCustomer(c: Customer): Customer {
    let customer = {};
    for (let prop in c) {
      customer[prop] = c[prop];
    }
    return customer as Customer;
  }
}
