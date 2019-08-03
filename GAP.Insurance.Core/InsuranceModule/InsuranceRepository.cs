using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GAP.Insurance.Common.Exceptions;
using GAP.Insurance.Common.Infrastructure;
using GAP.Insurance.Domain;
using GAP.Insurance.TO;
using Microsoft.EntityFrameworkCore;

namespace GAP.Insurance.Core.InsuranceModule
{
    /// <summary>
    /// Class that implements the methods of the Insurance module
    /// </summary>
    public class InsuranceRepository : IInsuranceRepository
    {
        DbContextOptions<DBInsuranceContext> _contextOptions;
        IMapper _mapper; //Allows to map entities between model and transfer objects
        ILocalizationService _localizer;

        /// <summary>
        /// Creates a new instance of the <see cref="SampleRepository"/> class
        /// </summary>
        /// <param name="contextOptions">Context options injected by DI engine</param>
        /// <param name="mapperConfiguration">Mapper configuration injected by DI engince</param>
        /// <param name="localizer">Localizer injected by DI</param>
        public InsuranceRepository(DbContextOptions<DBInsuranceContext> contextOptions, MapperConfiguration mapperConfiguration, ILocalizationService localizer)
        {
            _contextOptions = contextOptions;
            _mapper = mapperConfiguration.CreateMapper();
            _localizer = localizer;
        }

        /// <see cref="GAP.Insurance.Core.InsuranceModule.IInsuranceRepository.Delete(string)"/>
        public async void Delete(string id)
        {
            //Validate argument
            var guid = ValidateId(id);

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var insurance = await context.Insurance.Where(s => s.InsuranceId == guid).FirstOrDefaultAsync();
                if (insurance != null)
                {
                    context.Remove(insurance);
                    await context.SaveChangesAsync();
                }
            }
        }

        /// <see cref="GAP.Insurance.Core.InsuranceModule.IInsuranceRepository.GetAll"/>
        public async Task<List<InsuranceTO>> GetAll()
        {
            List<InsuranceTO> insurancesTO = null;

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var insurances = await context.Insurance.ToListAsync();
                insurancesTO = _mapper.Map<List<InsuranceTO>>(insurances.OrderBy(s => s.Name));
            }

            return insurancesTO;
        }

        /// <see cref="GAP.Insurance.Core.InsuranceModule.IInsuranceRepository.GetById(string)"/>
        public async Task<InsuranceTO> GetById(string id)
        {
            //Validate argument
            var guid = ValidateId(id);

            InsuranceTO insuranceTO = null;
            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var insurance = await context.Insurance.Where(s => s.InsuranceId == guid).FirstOrDefaultAsync();

                if (insurance != null)
                {
                    insuranceTO = _mapper.Map<InsuranceTO>(insurance);
                }
            }

            return insuranceTO;
        }

        /// <see cref="GAP.Insurance.Core.InsuranceModule.IInsuranceRepository.Save(InsuranceTO)"/>
        public async void Save(InsuranceTO insuranceTO)
        {
            if (insuranceTO == null)
                throw new ArgumentNullException("insuranceTO");

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                //Validate if is an insert or an update
                if (string.IsNullOrWhiteSpace(insuranceTO.InsuranceId))
                {
                    Domain.Insurance insurance = _mapper.Map<Domain.Insurance>(insuranceTO);
                    context.Insurance.Add(insurance);
                }
                else
                {
                    var guid = ValidateId(insuranceTO.InsuranceId);
                    var insurance = await context.Insurance.Where(s => s.InsuranceId == guid).FirstOrDefaultAsync();

                    if (insurance == null)
                    {
                        throw new CustomException(_localizer.GetMessage("ERROR_EntityNotFound", insuranceTO.InsuranceId));
                    }

                    insurance.Name = insuranceTO.Name;
                    insurance.StartDate = insuranceTO.EndDate;
                    insurance.CoverageType = insuranceTO.CoverageType;
                    insurance.Coverage = insuranceTO.Coverage;
                    insurance.Cost = insuranceTO.Cost;
                    insurance.RiskType = insuranceTO.RiskType;
                    insurance.Description = insuranceTO.Description;
                }

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
