using Microsoft.AspNetCore.Mvc;
using VacationLeavesApi.Data;
using VacationLeavesApi.Interfaces;

namespace VacationLeavesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VacationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public VacationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetAllVacations")]
        public async Task<IEnumerable<Vacation>> GetVacationsAsync()
        {
            return await _unitOfWork.VacationRepository.GetAllAsync();
        }
        [HttpGet("GetVacation/{id}")]
        public async Task<Vacation> GetVacationAsync(int id)
        {
            return await _unitOfWork.VacationRepository.GetAsync(vac=>vac.Vaccode==id);
        }
        [HttpPost("RequestVacation")]
        public async Task<IActionResult> RequestVacationAsync(Vacation vacation)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest);
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
        [HttpDelete("WithdrowVacation")]
        public async Task<IActionResult> DeleteVacationAsync(int id)
        {
            await _unitOfWork.VacationRepository.DeleteAsync(vac => vac.Vaccode == id);
            await _unitOfWork.CompleteAsync();
            return StatusCode(statusCode: StatusCodes.Status200OK);
        }
    }
}
