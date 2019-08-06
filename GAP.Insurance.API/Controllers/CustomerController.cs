using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GAP.Insurance.Core.CustomerModule;
using GAP.Insurance.TO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GAP.Insurance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerRepository _repository;

        /// <summary>
        /// Allows to initialize a new instance of the <see cref="CustomerController"/> type
        /// </summary>
        /// <param name="repository">Repository injected by DI</param>
        public CustomerController(ICustomerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CustomerTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var insuranceList = await _repository.GetAll();
            return Ok(insuranceList);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CustomerTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _repository.GetById(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CustomerTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Save([FromBody] CustomerTO sampleTO)
        {
            var customer = await _repository.Save(sampleTO);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task Delete(Guid id)
        {
            await _repository.Delete(id);
        }
        
        [HttpPost("saveInsurances/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveInsurances(Guid id, [FromBody] List<CustomerInsuranceTO> customerInsurances)
        {
            await _repository.SaveInsurances(id, customerInsurances);
            return Ok();
        }

        [HttpPost("cancelInsurances/{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CancelInsurances(Guid id, [FromBody] List<CustomerInsuranceTO> customerInsurances)
        {
            await _repository.CancelInsurances(id, customerInsurances);
            return Ok();
        }
    }
}