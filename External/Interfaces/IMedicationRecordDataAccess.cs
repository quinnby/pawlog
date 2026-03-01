using CarCareTracker.Models;

namespace CarCareTracker.External.Interfaces
{
    public interface IMedicationRecordDataAccess
    {
        List<MedicationRecord> GetMedicationRecordsByVehicleId(int vehicleId);
        MedicationRecord GetMedicationRecordById(int medicationRecordId);
        bool DeleteMedicationRecordById(int medicationRecordId);
        bool SaveMedicationRecord(MedicationRecord medicationRecord);
        bool DeleteAllMedicationRecordsByVehicleId(int vehicleId);
    }
}
