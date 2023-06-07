using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using VacationLeavesApi.Brokers;
using VacationLeavesApi.Data;
using VacationLeavesApi.Interfaces;

namespace VacationLeavesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPubSub _rabbitmq;
        private readonly ILogger<VacationController> _logger;
        public VacationController(IUnitOfWork unitOfWork, IPubSub rabbitmq, ILogger<VacationController> logger)
        {
            _unitOfWork = unitOfWork;
            _rabbitmq = rabbitmq;
            _logger = logger;
        }
        [HttpGet("GetAllVacations")]
        public async Task<IEnumerable<Vacation>> GetVacationsAsync()
        {
            return await _unitOfWork.VacationRepository.GetAllAsync();
        }
        [HttpGet("GetVacation/{id}")]
        public async Task<Vacation> GetVacationAsync(int id)
        {
            return await _unitOfWork.VacationRepository.GetAsync(vac => vac.Vaccode == id);
        }
        [HttpPost("RequestVacation")]
        public async Task<IActionResult> RequestVacationAsync(Vacation vacation)
        {
            //if (!ModelState.IsValid)
            //    return StatusCode(StatusCodes.Status400BadRequest);
           string message= _rabbitmq.RecieveMessage();
            if (string.IsNullOrEmpty(message))
                return StatusCode(StatusCodes.Status404NotFound);
            var data = JsonConvert.DeserializeObject<Vacation>(message);
            _logger.LogInformation(message);
            await _unitOfWork.VacationRepository.CreateAsync(entity: vacation);
            await _unitOfWork.CompleteAsync();
            return StatusCode(statusCode: StatusCodes.Status200OK);
        }
        [HttpPut("EditVacation")]
        public async Task<IActionResult> EditVacation(Vacation vacation)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest);
            _unitOfWork.VacationRepository.Update(vacation);
            await _unitOfWork.CompleteAsync();
            return StatusCode(statusCode: StatusCodes.Status200OK);
        }
        [HttpPatch("ModifyVacation/{id}")]
        public async Task<IActionResult> ModifyVacationAsync(int id, [FromBody] JsonPatchDocument<Vacation> patchdoc)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest);
            var entity = await _unitOfWork.VacationRepository.GetAsync(v => v.Vaccode == id);
            patchdoc.ApplyTo(entity,ModelState);
            _unitOfWork.VacationRepository.Update(entity);
            await _unitOfWork.CompleteAsync();
            return StatusCode(statusCode: StatusCodes.Status200OK);
        }
        [HttpDelete("WithdrowVacation")]
        public async Task<IActionResult> DeleteVacationAsync(int id)
        {
            await _unitOfWork.VacationRepository.DeleteAsync(vac => vac.Vaccode == id);
            await _unitOfWork.CompleteAsync();
            return StatusCode(statusCode: StatusCodes.Status200OK);
        }
    }
}
