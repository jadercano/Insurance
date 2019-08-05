import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Customer } from '../../domain/customer.domain';

@Injectable()
export class CustomerService {

  constructor(private http: Http, private httpClient: HttpClient) { }

  getCustomers() {
    return this.http.get('Customer')
      .toPromise()
      .then(this.extractArrayData)
      .catch(this.handleError);
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
    let result = this.http
      .delete(`Customer/${customerId}`)
      .toPromise()
      .catch(this.handleError);
    console.log('delete customer: ', result);
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
    let body = error.json();
    console.log(body);
    return Promise.reject(body.error || error);
  }
}
