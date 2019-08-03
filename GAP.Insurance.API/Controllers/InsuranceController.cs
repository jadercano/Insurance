using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAP.Insurance.Core.InsuranceModule;
using GAP.Insurance.TO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAP.Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceController : ControllerBase
    {
        private IInsuranceRepository _repository;

        /// <summary>
        /// Allows to initialize a new instance of the <see cref="InsuranceController"/> type
        /// </summary>
        /// <param name="repository">Repository injected by DI</param>
        public InsuranceController(IInsuranceRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<InsuranceTO>), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var insuranceList = _repository.GetAll();
            return Ok(insuranceList);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var insurance = _repository.GetById(id);
            return Ok(insurance);
        }

        [HttpPost]
        public void Save([FromBody] InsuranceTO sampleTO)
        {
            _repository.Save(sampleTO);
        }

        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _repository.Delete(id);
        }
    }
}