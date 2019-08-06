import { Insurance } from '../domain/insurance.domain';

export interface CustomerInsurance {
  insurance: Insurance;
  startDate: Date;
  endDate: Date;
  status: string;
}
