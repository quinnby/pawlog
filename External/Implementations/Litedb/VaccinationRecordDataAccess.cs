using CarCareTracker.External.Interfaces;
using CarCareTracker.Models;
using CarCareTracker.Helper;
using LiteDB;

namespace CarCareTracker.External.Implementations
{
    public class VaccinationRecordDataAccess : IVaccinationRecordDataAccess
    {
        private ILiteDBHelper _liteDB { get; set; }
        private static string tableName = "vaccinationrecords";

        public VaccinationRecordDataAccess(ILiteDBHelper liteDB)
        {
            _liteDB = liteDB;
        }

        public List<VaccinationRecord> GetVaccinationRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VaccinationRecord>(tableName);
            var records = table.Find(Query.EQ(nameof(VaccinationRecord.VehicleId), vehicleId));
            return records.ToList() ?? new List<VaccinationRecord>();
        }

        public VaccinationRecord GetVaccinationRecordById(int vaccinationRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VaccinationRecord>(tableName);
            return table.FindById(vaccinationRecordId);
        }

        public bool DeleteVaccinationRecordById(int vaccinationRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VaccinationRecord>(tableName);
            table.Delete(vaccinationRecordId);
            db.Checkpoint();
            return true;
        }

        public bool SaveVaccinationRecord(VaccinationRecord vaccinationRecord)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VaccinationRecord>(tableName);
            table.Upsert(vaccinationRecord);
            db.Checkpoint();
            return true;
        }

        public bool DeleteAllVaccinationRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VaccinationRecord>(tableName);
            table.DeleteMany(Query.EQ(nameof(VaccinationRecord.VehicleId), vehicleId));
            db.Checkpoint();
            return true;
        }
    }
}
