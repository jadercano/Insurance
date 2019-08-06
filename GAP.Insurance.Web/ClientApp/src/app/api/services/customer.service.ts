import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Customer } from '../../domain/customer.domain';
import { CustomerInsurance } from '../../domain/customerinsurance.domain';

@Injectable()
export class CustomerService {

  constructor(private http: Http, private httpClient: HttpClient) { }

  getCustomers() {
    return this.http.get('Customer')
      .toPromise()
      .then(this.extractArrayData)
      .catch(this.handleError);
  }

  get(customerId: string) {
    console.log('get customer - customerId: ', customerId);
    let result = this.http.get(`Customer/${customerId}`)
      .toPromise()
      .then(this.extractArrayData)
      .catch(this.handleError);
    console.log('get customer: ', result);
    return result;
  }

  save(customer: Customer): Promise<any> {
    let result = this.http
      .post('Customer', customer)
      .toPromise()
      .then(this.extractArrayData)
      .catch(this.handleError);
    console.log('save customer: ', result);
    return result;
  }

  delete(customerId: string): Promise<any> {
    console.log('delete customer - customerId: ', customerId);
    let result = this.http
      .delete(`Customer/${customerId}`)
      .toPromise()
      .catch(this.handleError);
    console.log('delete customer: ', result);
    return result;
  }

  saveInsurances(insurances: CustomerInsurance[]) {
    let result = this.http
      .post('Customer/saveInsurances', insurances)
      .toPromise()
      .catch(this.handleError);
    console.log('add insurances: ', result);
    return result;
  }

  cancelInsurances(insurances: CustomerInsurance[]) {
    let result = this.http
      .post('Customer/cancelInsurances', insurances)
      .toPromise()
      .catch(this.handleError);
    console.log('delete insurances: ', result);
    return result;
  }

  private extractArrayData(res: Response) {
    let body = res.json();
    return body || [];
  }

  private extractData(res: Response) {
    if (res.text() === 'true') {
      return {};
    }
    let body = res.json()
      .map((model: Customer) => model);
    return body || {};
  }

  private handleError(error: any): Promise<any> {
    console.log(error);
    let body = error.json();
    console.log(body);
    return Promise.reject(body.error || error);
  }
}
