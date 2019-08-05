export interface Insurance {
  insuranceId: string;
  name: string;
  startDate: Date;
  endDate: Date;
  coverageType: string;
  coverage: number;
  cost: number;
  riskType: string;
  description: string;
}
