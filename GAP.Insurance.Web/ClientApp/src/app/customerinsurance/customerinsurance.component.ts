import { Component, OnInit, Input } from '@angular/core';
import { Customer } from '../domain/customer.domain';
import { Insurance } from '../domain/insurance.domain';
import { CustomerInsurance } from '../domain/customerinsurance.domain';
import { CustomerService } from '../api/services/customer.service';
import { InsuranceService } from '../api/services/insurance.service';
import { MessageService, ConfirmationService, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-customer-customerInsurance',
  templateUrl: './customerinsurance.component.html'
})
export class CustomerInsuranceComponent implements OnInit {

  @Input() customerId: string;

  customer: Customer;

  insuranceList: Insurance[];

  displayDialog: boolean;

  customerInsurance: CustomerInsurance = {} as CustomerInsurance;

  selectedInsurances: CustomerInsurance[];

  newInsurance: boolean;

  insurances: CustomerInsurance[];

  cols: any[];

  constructor(private insuranceService: InsuranceService, private customerService: CustomerService, private messageService: MessageService, private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.getDetail();

    this.loadInsurances();

    this.cols = [
      { field: 'customerInsurance.name', header: 'Name' },
      { field: 'startDate', header: 'Start date' },
      { field: 'endDate', header: 'End date' }
    ];
  }

  getDetail() {
    this.customerService.get(this.customerId).then(customer => this.customer = customer);
  }

  loadInsurances() {
    this.insuranceService.getInsurances().then(insurances => this.insuranceList = insurances);
  }

  showDialogToAdd() {
    this.newInsurance = true;
    this.customerInsurance = {} as CustomerInsurance;
    this.displayDialog = true;
  }

  add() {
    this.insurances.push(this.customerInsurance)
  }

  save() {
    this.customerService.saveInsurances(this.selectedInsurances)
      .then(data => {
        this.customerInsurance = null;
        this.displayDialog = false;
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Insurance has been saved.' });
      })
      .then(this.getDetail.bind(this))
      .catch((error) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error });
      });
  }

  edit(customerInsurance: CustomerInsurance) {
    this.newInsurance = false;
    this.customerInsurance = this.cloneInsurance(customerInsurance);
    this.displayDialog = true;
  }

  delete() {
    this.confirmationService.confirm({
      message: 'Are you sure?',
      accept: this.deleteConfirmed.bind(this),
      reject: () => {
        this.customerInsurance = null;
      }
    });

  }

  deleteConfirmed() {
    this.customerService.deleteInsurances(this.selectedInsurances)
      .then(() => {
        this.customerInsurance = null;
        this.displayDialog = false;
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Insurance has been deleted.' });
      })
      .then(this.getDetail.bind(this));
  }

  onRowSelect(event) {
    this.newInsurance = false;
    this.customerInsurance = this.cloneInsurance(event.data);
    this.displayDialog = true;
  }

  cloneInsurance(c: CustomerInsurance): CustomerInsurance {
    let customerInsurance = {};
    for (let prop in c) {
      customerInsurance[prop] = c[prop];
    }
    return customerInsurance as CustomerInsurance;
  }
}
