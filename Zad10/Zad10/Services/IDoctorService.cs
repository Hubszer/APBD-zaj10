using Zad10.Models;

namespace Zad10.Services;

public interface IDoctorService
{
    Task<bool> DoesPatientExist(int patientId);
    Task<bool> DoesPrescriptionExist(int presciptionId);
    Task AddNewPrescription(PrescriptionRequestDto prescriptionRequestDto);
    Task<PatientResponseDto> GetPrescription(int prescriptionId);
    Task AddNewPatient(Patient patient);
    Task<PatientResponseDto> GetPatientDetails(int patientId);
    Task<bool> DoesMedicamentExist(string name);

}