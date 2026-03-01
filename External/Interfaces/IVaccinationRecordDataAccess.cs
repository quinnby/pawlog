using CarCareTracker.Models;

namespace CarCareTracker.External.Interfaces
{
    public interface IVaccinationRecordDataAccess
    {
        List<VaccinationRecord> GetVaccinationRecordsByVehicleId(int vehicleId);
        VaccinationRecord GetVaccinationRecordById(int vaccinationRecordId);
        bool DeleteVaccinationRecordById(int vaccinationRecordId);
        bool SaveVaccinationRecord(VaccinationRecord vaccinationRecord);
        bool DeleteAllVaccinationRecordsByVehicleId(int vehicleId);
    }
}
