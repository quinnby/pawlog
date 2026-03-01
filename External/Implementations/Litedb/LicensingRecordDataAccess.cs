using CarCareTracker.External.Interfaces;
using CarCareTracker.Models;
using CarCareTracker.Helper;
using LiteDB;

namespace CarCareTracker.External.Implementations
{
    public class LicensingRecordDataAccess : ILicensingRecordDataAccess
    {
        private ILiteDBHelper _liteDB { get; set; }
        private static string tableName = "licensingrecords";

        public LicensingRecordDataAccess(ILiteDBHelper liteDB)
        {
            _liteDB = liteDB;
        }

        public List<LicensingRecord> GetLicensingRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<LicensingRecord>(tableName);
            var records = table.Find(Query.EQ(nameof(LicensingRecord.VehicleId), vehicleId));
            return records.ToList() ?? new List<LicensingRecord>();
        }

        public LicensingRecord GetLicensingRecordById(int licensingRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<LicensingRecord>(tableName);
            return table.FindById(licensingRecordId);
        }

        public bool DeleteLicensingRecordById(int licensingRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<LicensingRecord>(tableName);
            table.Delete(licensingRecordId);
            db.Checkpoint();
            return true;
        }

        public bool SaveLicensingRecord(LicensingRecord licensingRecord)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<LicensingRecord>(tableName);
            table.Upsert(licensingRecord);
            db.Checkpoint();
            return true;
        }

        public bool DeleteAllLicensingRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<LicensingRecord>(tableName);
            table.DeleteMany(Query.EQ(nameof(LicensingRecord.VehicleId), vehicleId));
            db.Checkpoint();
            return true;
        }
    }
}
