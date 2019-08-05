using GAP.Insurance.TO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GAP.Insurance.Core.InsuranceModule
{
    /// <summary>
    /// Contracts for Insurance module
    /// </summary>
    public interface IInsuranceRepository
    {
        /// <summary>
        /// Allows to get all the Insurance entities
        /// </summary>
        /// <returns></returns>
        Task<List<InsuranceTO>> GetAll();

        /// <summary>
        /// Allows to get a Insurance entity by id
        /// </summary>
        /// <param name="id">The identifier to look for</param>
        /// <returns></returns>
        Task<InsuranceTO> GetById(Guid id);

        /// <summary>
        /// Allows to save a Insurance entity
        /// </summary>
        /// <param name="insurance">The insurance entity to be saved</param>
        Task<InsuranceTO> Save(InsuranceTO insuranceTO);

        /// <summary>
        /// Allows to delete a Insurance entity
        /// </summary>
        /// <param name="id">The identifier of the insurance entity to delete</param>
        Task Delete(Guid id);
    }
}
