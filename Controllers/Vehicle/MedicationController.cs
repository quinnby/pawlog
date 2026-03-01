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
        public IActionResult GetMedicationRecordsByVehicleId(int vehicleId)
        {
            var result = _medicationRecordDataAccess.GetMedicationRecordsByVehicleId(vehicleId);
            bool _useDescending = _config.GetUserConfig(User).UseDescending;
            if (_useDescending)
            {
                result = result.OrderByDescending(x => x.Date).ToList();
            }
            else
            {
                result = result.OrderBy(x => x.Date).ToList();
            }
            return PartialView("Medication/_MedicationRecords", result);
        }

        [HttpGet]
        public IActionResult GetAddMedicationRecordPartialView()
        {
            return PartialView("Medication/_MedicationRecordModal", new MedicationRecordInput());
        }

        [HttpGet]
        public IActionResult GetMedicationRecordForEditById(int medicationRecordId)
        {
            var result = _medicationRecordDataAccess.GetMedicationRecordById(medicationRecordId);
            if (!_userLogic.UserCanEditVehicle(GetUserID(), result.VehicleId, HouseholdPermission.View))
            {
                return Redirect("/Error/Unauthorized");
            }
            var convertedResult = new MedicationRecordInput
            {
                Id = result.Id,
                VehicleId = result.VehicleId,
                Date = result.Date.ToShortDateString(),
                MedicationName = result.MedicationName,
                Dosage = result.Dosage,
                Unit = result.Unit,
                Frequency = result.Frequency,
                Route = result.Route,
                EndDate = result.EndDate,
                PrescribingVet = result.PrescribingVet,
                Purpose = result.Purpose,
                RefillDate = result.RefillDate,
                ReminderEnabled = result.ReminderEnabled,
                IsActive = result.IsActive,
                LinkedHealthRecordId = result.LinkedHealthRecordId,
                Cost = result.Cost,
                Notes = result.Notes,
                Files = result.Files,
                Tags = result.Tags,
                ExtraFields = result.ExtraFields
            };
            return PartialView("Medication/_MedicationRecordModal", convertedResult);
        }

        [HttpPost]
        public IActionResult SaveMedicationRecordToVehicleId(MedicationRecordInput medicationRecord)
        {
            if (!_userLogic.UserCanEditVehicle(GetUserID(), medicationRecord.VehicleId, HouseholdPermission.Edit))
            {
                return Json(OperationResponse.Failed("Access Denied"));
            }
            medicationRecord.Files = medicationRecord.Files
                .Select(x => new UploadedFiles
                {
                    Name = x.Name,
                    Location = _fileHelper.MoveFileFromTemp(x.Location, "documents/")
                }).ToList();

            var convertedRecord = medicationRecord.ToMedicationRecord();
            var result = _medicationRecordDataAccess.SaveMedicationRecord(convertedRecord);
            if (result)
            {
                // Phase 5 – Sync reminder when RefillDate is set and ReminderEnabled is toggled
                SyncReminderFromLinkedRecord(
                    petId: convertedRecord.VehicleId,
                    reminderEnabled: medicationRecord.ReminderEnabled,
                    dueDateString: medicationRecord.RefillDate,
                    description: $"Medication Refill: {medicationRecord.MedicationName}",
                    petReminderType: PetReminderType.MedicationRefill,
                    linkedRecordType: ReminderLinkedRecordType.Medication,
                    linkedRecordId: convertedRecord.Id);
            }
            return Json(OperationResponse.Conditional(result, string.Empty, StaticHelper.GenericErrorMessage));
        }

        [HttpPost]
        public IActionResult DeleteMedicationRecordById(int medicationRecordId)
        {
            var existingRecord = _medicationRecordDataAccess.GetMedicationRecordById(medicationRecordId);
            if (!_userLogic.UserCanEditVehicle(GetUserID(), existingRecord.VehicleId, HouseholdPermission.Delete))
            {
                return Json(OperationResponse.Failed("Access Denied"));
            }
            var result = _medicationRecordDataAccess.DeleteMedicationRecordById(existingRecord.Id);
            return Json(OperationResponse.Conditional(result, string.Empty, StaticHelper.GenericErrorMessage));
        }
    }
}
