using GAP.Insurance.TO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GAP.Insurance.Core.CustomerModule
{
    /// <summary>
    /// Contracts for Customer module
    /// </summary>
    public interface ICustomerRepository
    {
        /// <summary>
        /// Allows to get all the Customer entities
        /// </summary>
        /// <returns></returns>
        Task<List<CustomerTO>> GetAll();

        /// <summary>
        /// Allows to get a Customer entity by id
        /// </summary>
        /// <param name="id">The identifier to look for</param>
        /// <returns></returns>
        Task<CustomerTO> GetById(Guid id);

        /// <summary>
        /// Allows to save a Customer entity
        /// </summary>
        /// <param name="customer">The customer entity to be saved</param>
        /// <returns></returns>
        Task<CustomerTO> Save(CustomerTO customerTO);

        /// <summary>
        /// Allows to assign customer insurances
        /// </summary>
        /// <param name="insurancesTO">Insurances to be saved</param>
        Task AssignInsurances(List<InsuranceTO> insurancesTO);

        /// <summary>
        /// Allows to cancel customer insurances
        /// </summary>
        /// <param name="insurancesTO">Insurance to be canceled</param>
        Task CancelInsurances(List<InsuranceTO> insurancesTO);

        /// <summary>
        /// Allows to delete a Insurance entity
        /// </summary>
        /// <param name="id">The identifier of the customer entity to delete</param>
        Task Delete(Guid id);
    }
}
