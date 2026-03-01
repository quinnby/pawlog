using CarCareTracker.External.Interfaces;
using CarCareTracker.Models;
using CarCareTracker.Helper;
using LiteDB;

namespace CarCareTracker.External.Implementations
{
    public class MedicationRecordDataAccess : IMedicationRecordDataAccess
    {
        private ILiteDBHelper _liteDB { get; set; }
        private static string tableName = "medicationrecords";

        public MedicationRecordDataAccess(ILiteDBHelper liteDB)
        {
            _liteDB = liteDB;
        }

        public List<MedicationRecord> GetMedicationRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<MedicationRecord>(tableName);
            var records = table.Find(Query.EQ(nameof(MedicationRecord.VehicleId), vehicleId));
            return records.ToList() ?? new List<MedicationRecord>();
        }

        public MedicationRecord GetMedicationRecordById(int medicationRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<MedicationRecord>(tableName);
            return table.FindById(medicationRecordId);
        }

        public bool DeleteMedicationRecordById(int medicationRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<MedicationRecord>(tableName);
            table.Delete(medicationRecordId);
            db.Checkpoint();
            return true;
        }

        public bool SaveMedicationRecord(MedicationRecord medicationRecord)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<MedicationRecord>(tableName);
            table.Upsert(medicationRecord);
            db.Checkpoint();
            return true;
        }

        public bool DeleteAllMedicationRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<MedicationRecord>(tableName);
            table.DeleteMany(Query.EQ(nameof(MedicationRecord.VehicleId), vehicleId));
            db.Checkpoint();
            return true;
        }
    }
}
