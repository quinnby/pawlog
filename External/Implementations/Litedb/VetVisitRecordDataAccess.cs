using CarCareTracker.External.Interfaces;
using CarCareTracker.Models;
using CarCareTracker.Helper;
using LiteDB;

namespace CarCareTracker.External.Implementations
{
    public class VetVisitRecordDataAccess : IVetVisitRecordDataAccess
    {
        private ILiteDBHelper _liteDB { get; set; }
        private static string tableName = "vetvisitrecords";

        public VetVisitRecordDataAccess(ILiteDBHelper liteDB)
        {
            _liteDB = liteDB;
        }

        public List<VetVisitRecord> GetVetVisitRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VetVisitRecord>(tableName);
            var records = table.Find(Query.EQ(nameof(VetVisitRecord.VehicleId), vehicleId));
            return records.ToList() ?? new List<VetVisitRecord>();
        }

        public VetVisitRecord GetVetVisitRecordById(int vetVisitRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VetVisitRecord>(tableName);
            return table.FindById(vetVisitRecordId);
        }

        public bool DeleteVetVisitRecordById(int vetVisitRecordId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VetVisitRecord>(tableName);
            table.Delete(vetVisitRecordId);
            db.Checkpoint();
            return true;
        }

        public bool SaveVetVisitRecord(VetVisitRecord vetVisitRecord)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VetVisitRecord>(tableName);
            table.Upsert(vetVisitRecord);
            db.Checkpoint();
            return true;
        }

        public bool DeleteAllVetVisitRecordsByVehicleId(int vehicleId)
        {
            var db = _liteDB.GetLiteDB();
            var table = db.GetCollection<VetVisitRecord>(tableName);
            table.DeleteMany(Query.EQ(nameof(VetVisitRecord.VehicleId), vehicleId));
            db.Checkpoint();
            return true;
        }
    }
}
