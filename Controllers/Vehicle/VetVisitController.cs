using CarCareTracker.Filter;
using CarCareTracker.Helper;
using CarCareTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace CarCareTracker.Controllers
{
    public partial class VehicleController
    {
        [TypeFilter(typeof(CollaboratorFilter))]
        [HttpGet]
        public IActionResult GetVetVisitRecordsByVehicleId(int vehicleId)
        {
            var result = _vetVisitRecordDataAccess.GetVetVisitRecordsByVehicleId(vehicleId);
            bool _useDescending = _config.GetUserConfig(User).UseDescending;
            if (_useDescending)
            {
                result = result.OrderByDescending(x => x.Date).ToList();
            }
            else
            {
                result = result.OrderBy(x => x.Date).ToList();
            }
            return PartialView("VetVisit/_VetVisitRecords", result);
        }

        [HttpGet]
        public IActionResult GetAddVetVisitRecordPartialView()
        {
            return PartialView("VetVisit/_VetVisitRecordModal", new VetVisitRecordInput());
        }

        [HttpGet]
        public IActionResult GetVetVisitRecordForEditById(int vetVisitRecordId)
        {
            var result = _vetVisitRecordDataAccess.GetVetVisitRecordById(vetVisitRecordId);
            if (!_userLogic.UserCanEditVehicle(GetUserID(), result.VehicleId, HouseholdPermission.View))
            {
                return Redirect("/Error/Unauthorized");
            }
            var convertedResult = new VetVisitRecordInput
            {
                Id = result.Id,
                VehicleId = result.VehicleId,
                Date = result.Date.ToShortDateString(),
                Clinic = result.Clinic,
                Veterinarian = result.Veterinarian,
                ReasonForVisit = result.ReasonForVisit,
                SymptomsReported = result.SymptomsReported,
                Diagnosis = result.Diagnosis,
                TreatmentProvided = result.TreatmentProvided,
                FollowUpNeeded = result.FollowUpNeeded,
                FollowUpDate = result.FollowUpDate,
                LinkedHealthRecordId = result.LinkedHealthRecordId,
                Cost = result.Cost,
                Notes = result.Notes,
                Description = result.Description,
                Files = result.Files,
                Tags = result.Tags,
                ExtraFields = result.ExtraFields
            };
            return PartialView("VetVisit/_VetVisitRecordModal", convertedResult);
        }

        [HttpPost]
        public IActionResult SaveVetVisitRecordToVehicleId(VetVisitRecordInput vetVisitRecord)
        {
            if (!_userLogic.UserCanEditVehicle(GetUserID(), vetVisitRecord.VehicleId, HouseholdPermission.Edit))
            {
                return Json(OperationResponse.Failed("Access Denied"));
            }
            vetVisitRecord.Files = vetVisitRecord.Files
                .Select(x => new UploadedFiles
                {
                    Name = x.Name,
                    Location = _fileHelper.MoveFileFromTemp(x.Location, "documents/")
                }).ToList();

            var convertedRecord = vetVisitRecord.ToVetVisitRecord();
            var result = _vetVisitRecordDataAccess.SaveVetVisitRecord(convertedRecord);
            return Json(OperationResponse.Conditional(result, string.Empty, StaticHelper.GenericErrorMessage));
        }

        [HttpPost]
        public IActionResult DeleteVetVisitRecordById(int vetVisitRecordId)
        {
            var existingRecord = _vetVisitRecordDataAccess.GetVetVisitRecordById(vetVisitRecordId);
            if (!_userLogic.UserCanEditVehicle(GetUserID(), existingRecord.VehicleId, HouseholdPermission.Delete))
            {
                return Json(OperationResponse.Failed("Access Denied"));
            }
            var result = _vetVisitRecordDataAccess.DeleteVetVisitRecordById(existingRecord.Id);
            return Json(OperationResponse.Conditional(result, string.Empty, StaticHelper.GenericErrorMessage));
        }
    }
}
