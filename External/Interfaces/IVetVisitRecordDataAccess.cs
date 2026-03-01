using CarCareTracker.Models;

namespace CarCareTracker.External.Interfaces
{
    public interface IVetVisitRecordDataAccess
    {
        List<VetVisitRecord> GetVetVisitRecordsByVehicleId(int vehicleId);
        VetVisitRecord GetVetVisitRecordById(int vetVisitRecordId);
        bool DeleteVetVisitRecordById(int vetVisitRecordId);
        bool SaveVetVisitRecord(VetVisitRecord vetVisitRecord);
        bool DeleteAllVetVisitRecordsByVehicleId(int vehicleId);
    }
}
