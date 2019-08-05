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
        public async Task<IActionResult> GetAll()
        {
            var insuranceList = await _repository.GetAll();
            return Ok(insuranceList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var insurance = await _repository.GetById(id);

            if (insurance == null)
            {
                return NotFound();
            }

            return Ok(insurance);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<InsuranceTO> Save([FromBody] InsuranceTO insuranceTO)
        {
            return await _repository.Save(insuranceTO);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }
    }
}