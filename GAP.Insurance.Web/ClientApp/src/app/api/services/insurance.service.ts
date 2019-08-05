import { Http, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Insurance } from '../../domain/insurance.domain';

@Injectable()
export class InsuranceService {

  constructor(private http: Http, private httpClient: HttpClient) { }

  getInsurances() {
    return this.http.get('Insurance')
      .toPromise()
      .then(this.extractArrayData)
      .catch(this.handleError);
  }

  save(insurance: Insurance): Promise<any> {
    let result = this.http
      .post('Insurance', insurance)
      .toPromise()
      .then(this.extractArrayData)
      .catch(this.handleError);
    console.log('save insurance: ', result);
    return result;
  }

  delete(insuranceId: string): Promise<any> {
    let result = this.http
      .delete(`Insurance/${insuranceId}`)
      .toPromise()
      .catch(this.handleError);
    console.log('delete insurance: ', result);
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
      .map((model: Insurance) => model);
    return body || {};
  }

  private handleError(error: any): Promise<any> {
    let body = error.json();
    console.log(body);
    return Promise.reject(body.error || error);
  }
}
