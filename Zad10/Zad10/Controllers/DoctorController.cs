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
        try
        {
            await _doctorService.AddNewPrescription(prescriptionRequestDto);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }


    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetPatientDetails(int patientId)
    {
        var details = await _doctorService.GetPatientDetails(patientId);

        if (details == null)
        {
            return NotFound();
        }

        return Ok(details);
    }
    
}