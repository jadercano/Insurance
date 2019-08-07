using AutoMapper;
using GAP.Insurance.Common.Exceptions;
using GAP.Insurance.Common.Helpers;
using GAP.Insurance.Common.Infrastructure;
using GAP.Insurance.Domain;
using GAP.Insurance.TO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAP.Insurance.Core.CustomerModule
{
    /// <summary>
    /// Class that implements the methods of the Customer module
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        DbContextOptions<DBInsuranceContext> _contextOptions;
        IMapper _mapper;
        ILocalizationService _localizer;

        /// <summary>
        /// Creates a new instance of the <see cref="CustomerRepository"/> class
        /// </summary>
        /// <param name="contextOptions">Context options injected by DI engine</param>
        /// <param name="mapperConfiguration">Mapper configuration injected by DI engince</param>
        /// <param name="localizer">Localizer injected by DI</param>
        public CustomerRepository(DbContextOptions<DBInsuranceContext> contextOptions, MapperConfiguration mapperConfiguration, ILocalizationService localizer)
        {
            _contextOptions = contextOptions;
            _mapper = mapperConfiguration.CreateMapper();
            _localizer = localizer;
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.Delete(string)"/>
        public async Task Delete(Guid id)
        {
            //Validate argument
            ValidateId(id);

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var customer = await context.Customer.Where(s => s.CustomerId == id).FirstOrDefaultAsync();
                if (customer != null)
                {
                    context.Remove(customer);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.GetAll"/>
        public async Task<List<CustomerTO>> GetAll()
        {
            List<CustomerTO> customersTO = null;

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var customers = await context.Customer.ToListAsync();
                customersTO = _mapper.Map<List<CustomerTO>>(customers.OrderBy(s => s.Name));
            }

            return customersTO;
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.GetById(string)"/>
        public async Task<CustomerTO> GetById(Guid id)
        {
            CustomerTO customerTO = null;
            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var customer = await ValidateCustomer(context, id);

                var customerInsurances = await context.CustomerInsurance
                    .Include(c => c.Insurance)
                    .Where(c => c.CustomerId == id && c.Status != CustomerInsuranceStatus.Canceled.ToString()).ToListAsync();
                
                customerTO = _mapper.Map<CustomerTO>(customer);
                customerTO.Insurances = _mapper.Map<List<CustomerInsuranceTO>>(customerInsurances);
            }

            return customerTO;
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.Save(CustomerTO)"/>
        public async Task<CustomerTO> Save(CustomerTO customerTO)
        {
            Validate(customerTO);

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                //Validate if is an insert or an update
                if (customerTO.CustomerId == Guid.Empty)
                {
                    customerTO.CustomerId = Guid.NewGuid();
                    Domain.Customer insurance = _mapper.Map<Domain.Customer>(customerTO);
                    context.Customer.Add(insurance);
                }
                else
                {
                    var customer = await ValidateCustomer(context, customerTO.CustomerId);

                    customer.Name = customerTO.Name;
                    customer.Email = customerTO.Email;
                }

                await context.SaveChangesAsync();
            }
            return customerTO;
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.SaveInsurances(Guid, List{CustomerInsuranceTO})(List{InsuranceTO})"/>
        public async Task SaveInsurances(Guid id, List<CustomerInsuranceTO> insurancesTO)
        {
            if (insurancesTO == null)
                throw new ArgumentNullException(nameof(insurancesTO));

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var customer = await ValidateCustomer(context, id);

                foreach (var insuranceTO in insurancesTO)
                {
                    var customerInsurance = await context.CustomerInsurance.FirstOrDefaultAsync(ci => ci.CustomerId == customer.CustomerId
                        && ci.InsuranceId == insuranceTO.Insurance.InsuranceId
                        && ci.StartDate == insuranceTO.StartDate);

                    if (customerInsurance == null)
                    {
                        customerInsurance = new CustomerInsurance
                        {
                            CustomerId = id,
                            InsuranceId = insuranceTO.Insurance.InsuranceId,
                            StartDate = insuranceTO.StartDate,
                            EndDate = insuranceTO.EndDate,
                            Status = CustomerInsuranceStatus.Active.ToString()
                        };
                        context.CustomerInsurance.Add(customerInsurance);
                    }

                    customerInsurance.EndDate = insuranceTO.EndDate;
                    customerInsurance.Status = CustomerInsuranceStatus.Active.ToString();
                }

                await context.SaveChangesAsync();
            }
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.CancelInsurances(Guid id, List{CustomerInsuranceTO})(List{InsuranceTO})"/>
        public async Task CancelInsurances(Guid id, List<CustomerInsuranceTO> insurancesTO)
        {
            if (insurancesTO == null)
                throw new ArgumentNullException(nameof(insurancesTO));

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var customer = await ValidateCustomer(context, id);

                foreach (var insuranceTO in insurancesTO)
                {
                    var customerInsurance = await context.CustomerInsurance.FirstOrDefaultAsync(ci => ci.CustomerId == customer.CustomerId
                        && ci.InsuranceId == insuranceTO.Insurance.InsuranceId
                        && ci.StartDate == insuranceTO.StartDate);

                    if (customerInsurance == null)
                    {
                        continue;
                    }

                    customerInsurance.Status = CustomerInsuranceStatus.Canceled.ToString();
                }
                
                await context.SaveChangesAsync();
            }
        }

        private void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new CustomException(_localizer.GetMessage("ERROR_EntityNotFound", id));
            }
        }

        private async Task<Customer> ValidateCustomer(DBInsuranceContext context, Guid id)
        {
            ValidateId(id);
            var customer = await context.Customer.Where(s => s.CustomerId == id).FirstOrDefaultAsync();

            if (customer == null)
            {
                throw new CustomException(_localizer.GetMessage("ERROR_EntityNotFound", id));
            }

            return customer;
        }

        private void Validate(CustomerTO customerTO)
        {
            if (customerTO == null)
            {
                throw new ArgumentNullException(nameof(customerTO));
            }

            //Validate name
            if (string.IsNullOrWhiteSpace(customerTO.Name))
            {
                throw new ArgumentNullException(nameof(customerTO.Name));
            }
            else if (customerTO.Name.Length > 50)
            {
                throw new CustomException(_localizer.GetMessage("Customer_Validate_Name"));
            }

            //Validate email
            if (string.IsNullOrWhiteSpace(customerTO.Email))
            {
                throw new ArgumentNullException(nameof(customerTO.Email));
            }
        }
    }
}
