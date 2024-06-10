using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Zad10.Services;

namespace Zad10;
[ApiController]
[Route("api/[controller]")]
public class DoctorController : ControllerBase
{
    private readonly DoctorService _doctorService;

    public DoctorController(DoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription(PrescriptionRequestDto prescriptionRequestDto)
    {
        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _doctorService.AddNewPrescription(prescriptionRequestDto);
            
            scope.Complete();
        }

        /*return Created();*/
        return Ok();

    }


    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetPatientDetails(int patientId)
    {

        if (!await _doctorService.DoesPatientExist(patientId))
        {
            return NotFound();
        }
        
        var details = await _doctorService.GetPatientDetails(patientId);

        if (details == null)
        {
            return NotFound();
        }

        return Ok(details);
    }
    
}