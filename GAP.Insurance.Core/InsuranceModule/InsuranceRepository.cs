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
        IMapper _mapper;
        ILocalizationService _localizer;

        /// <summary>
        /// Creates a new instance of the <see cref="InsuranceRepository"/> class
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
        public async Task Delete(Guid id)
        {
            //Validate argument
            ValidateId(id);

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var insurance = await context.Insurance.Where(s => s.InsuranceId == id).FirstOrDefaultAsync();
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
        public async Task<InsuranceTO> GetById(Guid id)
        {
            //Validate argument
            ValidateId(id);

            InsuranceTO insuranceTO = null;
            using (var context = new DBInsuranceContext(_contextOptions))
            {
                var insurance = await context.Insurance.Where(s => s.InsuranceId == id).FirstOrDefaultAsync();

                if (insurance != null)
                {
                    insuranceTO = _mapper.Map<InsuranceTO>(insurance);
                }
            }

            return insuranceTO;
        }

        /// <see cref="GAP.Insurance.Core.InsuranceModule.IInsuranceRepository.Save(InsuranceTO)"/>
        public async Task<InsuranceTO> Save(InsuranceTO insuranceTO)
        {
            Validate(insuranceTO);

            using (var context = new DBInsuranceContext(_contextOptions))
            {
                //Validate if is an insert or an update
                if (insuranceTO.InsuranceId == Guid.Empty)
                {
                    insuranceTO.InsuranceId = Guid.NewGuid();
                    Domain.Insurance insurance = _mapper.Map<Domain.Insurance>(insuranceTO);
                    context.Insurance.Add(insurance);
                }
                else
                {
                    ValidateId(insuranceTO.InsuranceId);
                    var insurance = await context.Insurance.Where(s => s.InsuranceId == insuranceTO.InsuranceId).FirstOrDefaultAsync();

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
            return insuranceTO;
        }

        private void ValidateId(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new CustomException(_localizer.GetMessage("ERROR_EntityNotFound", id));
            }
        }

        private void Validate(InsuranceTO insuranceTO)
        {
            if (insuranceTO == null)
            {
                throw new ArgumentNullException(nameof(insuranceTO));
            }

            //Validate name
            if (string.IsNullOrWhiteSpace(insuranceTO.Name))
            {
                throw new ArgumentNullException(nameof(insuranceTO.Name));
            }
            else if (insuranceTO.Name.Length > 50)
            {
                throw new InvalidOperationException(_localizer.GetMessage("Customer_Validate_Name"));
            }

            //Validate start date
            if (insuranceTO.StartDate == default(DateTime))
            {
                throw new ArgumentNullException(nameof(insuranceTO.StartDate));
            }

            //Validate end date
            if (insuranceTO.EndDate == default(DateTime))
            {
                throw new ArgumentNullException(nameof(insuranceTO.EndDate));
            }

            //Validate coverage type
            if (string.IsNullOrWhiteSpace(insuranceTO.CoverageType))
            {
                throw new ArgumentNullException(nameof(insuranceTO.CoverageType));
            }

            //Validate risk type
            if (string.IsNullOrWhiteSpace(insuranceTO.RiskType))
            {
                throw new ArgumentNullException(nameof(insuranceTO.RiskType));
            }

            //Validate risk type and coverage
            if (insuranceTO.RiskType.Equals("High") && insuranceTO.Coverage > 50)
            {
                throw new CustomException(_localizer.GetMessage("Insurance_Validate_RiskType_Coverage"));
            }
        }
    }
}
