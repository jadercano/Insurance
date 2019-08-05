import { Component, OnInit } from '@angular/core';
import { Insurance } from '../domain/insurance.domain';
import { InsuranceService } from '../api/services/insurance.service';
import { MessageService, ConfirmationService, SelectItem } from 'primeng/api';

@Component({
  selector: 'app-insurance',
  templateUrl: './insurance.component.html'
})
export class InsuranceComponent implements OnInit {

  displayDialog: boolean;

  insurance: Insurance = {} as Insurance;

  selectedInsurance: Insurance;

  newInsurance: boolean;

  insurances: Insurance[];

  coverageTypes: SelectItem[];

  riskTypes: SelectItem[];

  cols: any[];

  constructor(private insuranceService: InsuranceService, private messageService: MessageService, private confirmationService: ConfirmationService) { }

  ngOnInit() {
    this.loadInsurances();

    this.cols = [
      { field: 'name', header: 'Name' },
      { field: 'startDate', header: 'Start date' },
      { field: 'endDate', header: 'End date' },
      { field: 'coverageType', header: 'Coverage type' },
      { field: 'coverage', header: 'Coverage' },
      { field: 'cost', header: 'Cost' },
      { field: 'riskType', header: 'Risk type' }
    ];

    this.coverageTypes = [
      { label: "Select coverage type", value: null },
      { label: "Earthquake", value: "Earthquake" },
      { label: "Fire", value: "Fire" },
      { label: "Theft", value: "Theft" },
      { label: "Loss", value: "Loss" },
    ];

    this.riskTypes = [
      { label: "Select risk type", value: null },
      { label: "Low", value: "Low" },
      { label: "Medium", value: "Medium" },
      { label: "Medium-High", value: "Medium-High" },
      { label: "High", value: "High" },
    ];
  }

  loadInsurances() {
    this.insuranceService.getInsurances().then(insurances => this.insurances = insurances);
  }

  showDialogToAdd() {
    this.newInsurance = true;
    this.insurance = {} as Insurance;
    this.displayDialog = true;
  }

  save() {
    this.insuranceService.save(this.insurance)
      .then(data => {
        this.insurance = null;
        this.displayDialog = false;
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Insurance has been saved.' });
      })
      .then(this.loadInsurances.bind(this))
      .catch((error) => {
        this.messageService.add({ severity: 'error', summary: 'Error Message', detail: error });
      });
  }

  edit(insurance: Insurance) {
    this.newInsurance = false;
    this.insurance = this.cloneInsurance(insurance);
    this.displayDialog = true;
  }

  delete(insurance: Insurance) {
    this.insurance = insurance;

    this.confirmationService.confirm({
      message: 'Are you sure?',
      accept: this.deleteConfirmed.bind(this),
      reject: () => {
        this.insurance = null;
      }
    });

  }

  deleteConfirmed() {
    this.insuranceService.delete(this.insurance.insuranceId)
      .then(() => {
        this.insurance = null;
        this.displayDialog = false;
        this.messageService.add({ severity: 'success', summary: 'Success Message', detail: 'Insurance has been deleted.' });
      })
      .then(this.loadInsurances.bind(this));
  }

  onRowSelect(event) {
    this.newInsurance = false;
    this.insurance = this.cloneInsurance(event.data);
    this.displayDialog = true;
  }

  cloneInsurance(c: Insurance): Insurance {
    let insurance = {};
    for (let prop in c) {
      insurance[prop] = c[prop];
    }
    return insurance as Insurance;
  }
}
