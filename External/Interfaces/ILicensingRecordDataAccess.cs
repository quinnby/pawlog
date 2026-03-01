using CarCareTracker.Models;

namespace CarCareTracker.External.Interfaces
{
    public interface ILicensingRecordDataAccess
    {
        List<LicensingRecord> GetLicensingRecordsByVehicleId(int vehicleId);
        LicensingRecord GetLicensingRecordById(int licensingRecordId);
        bool DeleteLicensingRecordById(int licensingRecordId);
        bool SaveLicensingRecord(LicensingRecord licensingRecord);
        bool DeleteAllLicensingRecordsByVehicleId(int vehicleId);
    }
}
