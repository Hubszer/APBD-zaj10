using Microsoft.EntityFrameworkCore;
using Zad10.Models;

namespace Zad10.Services;

public class DoctorService : IDoctorService
{

    private readonly ApplicationContext _context;

    public DoctorService(ApplicationContext context)
    {
        _context = context;
    }


    public async Task<bool> DoesPatientExist(int patientId)
    {
        return await _context.Patients.AnyAsync(e => e.IdPatient == patientId);
    }

    public async Task<bool> DoesPrescriptionExist(int prescriptionId)
    {
        return await _context.Prescriptions.AnyAsync(e => e.IdPrescription == prescriptionId);
    }

    public async Task AddNewPrescription(PrescriptionRequestDto request)
    {
        if (request.Medicaments.Count > 10)
        {
            throw new ArgumentException("A prescription can contain a maximum of 10 medicaments.");
        }

        if (request.DueDate < request.Date)
        {
            throw new ArgumentException("DueDate must be greater than or equal to Date.");
        }

        // Check if patient exists, if not create a new one
        var patient = await _context.Patients.FirstOrDefaultAsync(p => 
            p.FirstName == request.Patient.FirstName && 
            p.LastName == request.Patient.LastName &&
            p.BirthDate == request.Patient.Birthdate);

        if (patient == null)
        {
            patient = new Patient
            {
                FirstName = request.Patient.FirstName,
                LastName = request.Patient.LastName,
                BirthDate = request.Patient.Birthdate
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
        }

        // Check if doctor exists
        var doctor = await _context.Doctors.FindAsync(request.Doctor.IdDoctor);
        if (doctor == null)
        {
            throw new ArgumentException("Doctor not found.");
        }

        // Check if all medicaments exist
        var medicamentIds = request.Medicaments.Select(m => m.IdMedicament).ToList();
        var existingMedicaments = await _context.Medicaments
            .Where(m => medicamentIds.Contains(m.IdMedicament))
            .ToListAsync();

        if (existingMedicaments.Count != request.Medicaments.Count)
        {
            throw new ArgumentException("One or more medicaments do not exist.");
        }

        // Create prescription
        var prescription = new Prescription
        {
            Date = request.Date,
            DueDate = request.DueDate,
            IdPatient = patient.IdPatient,
            IdDoctor = doctor.IdDoctor // Ensure that doctor.IdDoctor is populated correctly
        };
        _context.Prescriptions.Add(prescription);
        await _context.SaveChangesAsync();

        foreach (var med in request.Medicaments)
        {
            var prescriptioMeds = new PrescriptionMedicament()
            {
                IdMedicament = med.IdMedicament,
                IdPrescription = prescription.IdPrescription,
                Dose = med.Dose,
                Details = med.Details
            };
            _context.PrescriptionMedicaments.Add(prescriptioMeds);
        }

        await _context.SaveChangesAsync();
    }

    public async Task<PatientResponseDto> GetPrescription(int patientId)
    {
        /*var patient = await _context.Prescriptions
            .Include(e => e.Patient)
            .Include(e => e.PrescriptionMedicaments)
            .Include(e => e.Doctor)
            .ThenInclude(e => e.Prescriptions)
            .Where(e => patientId == null || e.Patient.IdPatient == patientId)
            .ToListAsync();*/
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null)
        {
            return null;
        }

        var response = new PatientResponseDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.BirthDate,
            Prescriptions = patient.Prescriptions.OrderBy(p => p.DueDate)
                .Select(pr => new PrescriptionResponseDto
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName,
                        Email = pr.Doctor.Email
                    },
                    Medicaments = pr.PrescriptionMedicaments
                        .Select(pm => new MedicamentResponseDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Medicament.Description
                        }).ToList()
                }).ToList()
        };

        return response;
    }
    

    public async Task AddNewPatient(Patient patient)
    {
        await _context.AddAsync(patient);
        await _context.SaveChangesAsync();
    }

    public async Task<PatientResponseDto> GetPatientDetails(int patientId)
    {
        var patient = await _context.Patients
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.Doctor)
            .Include(p => p.Prescriptions)
            .ThenInclude(pr => pr.PrescriptionMedicaments)
            .ThenInclude(pm => pm.Medicament)
            .FirstOrDefaultAsync(p => p.IdPatient == patientId);

        if (patient == null)
        {
            return null;    
        }
        

        var response = new PatientResponseDto
        {
            IdPatient = patient.IdPatient,
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            Birthdate = patient.BirthDate,
            Prescriptions = patient.Prescriptions.OrderBy(p => p.DueDate)
                .Select(pr => new PrescriptionResponseDto
                {
                    IdPrescription = pr.IdPrescription,
                    Date = pr.Date,
                    DueDate = pr.DueDate,
                    Doctor = new DoctorDto
                    {
                        IdDoctor = pr.Doctor.IdDoctor,
                        FirstName = pr.Doctor.FirstName,
                        LastName = pr.Doctor.LastName,
                        Email = pr.Doctor.Email
                    },
                    Medicaments = pr.PrescriptionMedicaments
                        .Select(pm => new MedicamentResponseDto
                        {
                            IdMedicament = pm.Medicament.IdMedicament,
                            Name = pm.Medicament.Name,
                            Dose = pm.Dose,
                            Description = pm.Medicament.Description
                        }).ToList()
                }).ToList()
        };

        return response;
    }
    

    public async Task<bool> DoesMedicamentExist(string name)
    {
        return await _context.Medicaments.AnyAsync(e => e.Name == name);
    }
}