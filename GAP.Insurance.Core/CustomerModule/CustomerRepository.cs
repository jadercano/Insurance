using AutoMapper;
using GAP.Insurance.Common.Exceptions;
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
        public async void Delete(string id)
        {
            //Validate argument
            var guid = ValidateId(id);

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var customer = await context.Customer.Where(s => s.CustomerId == guid).FirstOrDefaultAsync();
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
            List<CustomerTO> insurancesTO = null;

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var customers = await context.Customer.ToListAsync();
                insurancesTO = _mapper.Map<List<CustomerTO>>(customers.OrderBy(s => s.Name));
            }

            return insurancesTO;
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.GetById(string)"/>
        public async Task<CustomerTO> GetById(string id)
        {
            //Validate argument
            var guid = ValidateId(id);

            CustomerTO insuranceTO = null;
            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var customer = await context.Customer.Where(s => s.CustomerId == guid).FirstOrDefaultAsync();

                if (customer != null)
                {
                    insuranceTO = _mapper.Map<CustomerTO>(customer);
                }
            }

            return insuranceTO;
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.Save(CustomerTO)"/>
        public async void Save(CustomerTO insuranceTO)
        {
            if (insuranceTO == null)
                throw new ArgumentNullException("insuranceTO");

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                //Validate if is an insert or an update
                if (string.IsNullOrWhiteSpace(insuranceTO.CustomerId))
                {
                    Domain.Customer insurance = _mapper.Map<Domain.Customer>(insuranceTO);
                    context.Customer.Add(insurance);
                }
                else
                {
                    var guid = ValidateId(insuranceTO.CustomerId);
                    var customer = await context.Customer.Where(s => s.CustomerId == guid).FirstOrDefaultAsync();

                    if (customer == null)
                    {
                        throw new CustomException(_localizer.GetMessage("ERROR_EntityNotFound", insuranceTO.CustomerId));
                    }

                    customer.Name = insuranceTO.Name;
                    customer.Email = insuranceTO.Email;
                }

                await context.SaveChangesAsync();
            }
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.AssignInsurances(List{InsuranceTO})"/>
        public async void AssignInsurances(List<InsuranceTO> insurancesTO)
        {
            if (insurancesTO == null)
                throw new ArgumentNullException("insuranceTO");

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                await context.SaveChangesAsync();
            }
        }

        /// <see cref="GAP.Insurance.Core.CustomerModule.ICustomerRepository.CancelInsurances(List{InsuranceTO})"/>
        public async void CancelInsurances(List<InsuranceTO> insurancesTO)
        {
            if (insurancesTO == null)
                throw new ArgumentNullException("insuranceTO");

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                await context.SaveChangesAsync();
            }
        }

        private Guid ValidateId(string id)
        {
            var guid = Guid.Empty;
            if (!Guid.TryParse(id, out guid))
            {
                throw new CustomException(_localizer.GetMessage("ERROR_EntityNotFound", id));
            }
            return guid;
        }
    }
}
