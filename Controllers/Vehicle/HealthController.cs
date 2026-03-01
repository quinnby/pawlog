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
        public IActionResult GetHealthRecordsByVehicleId(int vehicleId)
        {
            var result = _healthRecordDataAccess.GetHealthRecordsByVehicleId(vehicleId);
            bool _useDescending = _config.GetUserConfig(User).UseDescending;
            if (_useDescending)
            {
                result = result.OrderByDescending(x => x.Date).ToList();
            }
            else
            {
                result = result.OrderBy(x => x.Date).ToList();
            }
            return PartialView("Health/_HealthRecords", result);
        }

        [HttpGet]
        public IActionResult GetAddHealthRecordPartialView()
        {
            return PartialView("Health/_HealthRecordModal", new HealthRecordInput());
        }

        [HttpGet]
        public IActionResult GetHealthRecordForEditById(int healthRecordId)
        {
            var result = _healthRecordDataAccess.GetHealthRecordById(healthRecordId);
            if (!_userLogic.UserCanEditVehicle(GetUserID(), result.VehicleId, HouseholdPermission.View))
            {
                return Redirect("/Error/Unauthorized");
            }
            var convertedResult = new HealthRecordInput
            {
                Id = result.Id,
                VehicleId = result.VehicleId,
                Date = result.Date.ToShortDateString(),
                Category = result.Category,
                Title = result.Title,
                Description = result.Description,
                Cost = result.Cost,
                Notes = result.Notes,
                Provider = result.Provider,
                FollowUpRequired = result.FollowUpRequired,
                FollowUpDate = result.FollowUpDate,
                Status = result.Status,
                Files = result.Files,
                Tags = result.Tags,
                ExtraFields = result.ExtraFields,
                // Phase 7 fields
                WeightValue = result.WeightValue,
                WeightUnit = result.WeightUnit,
                Severity = result.Severity,
                AllergyType = result.AllergyType,
                Trigger = result.Trigger,
                ReminderEnabled = result.ReminderEnabled,
                ReminderDueDate = result.ReminderDueDate
            };
            return PartialView("Health/_HealthRecordModal", convertedResult);
        }

        [HttpPost]
        public IActionResult SaveHealthRecordToVehicleId(HealthRecordInput healthRecord)
        {
            if (!_userLogic.UserCanEditVehicle(GetUserID(), healthRecord.VehicleId, HouseholdPermission.Edit))
            {
                return Json(OperationResponse.Failed("Access Denied"));
            }
            // Move any newly uploaded files out of temp storage
            healthRecord.Files = healthRecord.Files
                .Select(x => new UploadedFiles
                {
                    Name = x.Name,
                    Location = _fileHelper.MoveFileFromTemp(x.Location, "documents/")
                }).ToList();

            var convertedRecord = healthRecord.ToHealthRecord();
            var result = _healthRecordDataAccess.SaveHealthRecord(convertedRecord);
            if (result && healthRecord.Category == HealthRecordCategory.PreventiveCare)
            {
                // Phase 7 – Sync a date-based reminder for preventive care records
                SyncReminderFromLinkedRecord(
                    petId: convertedRecord.VehicleId,
                    reminderEnabled: healthRecord.ReminderEnabled,
                    dueDateString: healthRecord.ReminderDueDate,
                    description: $"Preventive Care Due: {healthRecord.Title}",
                    petReminderType: PetReminderType.Custom,
                    linkedRecordType: ReminderLinkedRecordType.HealthRecord,
                    linkedRecordId: convertedRecord.Id);
            }
            return Json(OperationResponse.Conditional(result, string.Empty, StaticHelper.GenericErrorMessage));
        }

        [TypeFilter(typeof(CollaboratorFilter))]
        [HttpGet]
        public IActionResult GetWeightHistoryByVehicleId(int vehicleId)
        {
            var records = _healthRecordDataAccess
                .GetHealthRecordsByVehicleId(vehicleId)
                .Where(x => x.Category == HealthRecordCategory.WeightCheck && x.WeightValue > 0)
                .OrderBy(x => x.Date)
                .Select(x => new
                {
                    date = x.Date.ToShortDateString(),
                    value = x.WeightValue,
                    unit = string.IsNullOrWhiteSpace(x.WeightUnit) ? "lbs" : x.WeightUnit,
                    title = x.Title
                })
                .ToList();
            return Json(records);
        }

        [HttpGet]
        public IActionResult GetAddQuickHealthNotePartialView()
        {
            // Pre-populate with Informational status and IllnessSymptom category as sensible defaults
            var model = new HealthRecordInput
            {
                Category = HealthRecordCategory.IllnessSymptom,
                Status = HealthRecordStatus.Informational
            };
            return PartialView("Health/_QuickHealthNoteModal", model);
        }

        [HttpPost]
        public IActionResult DeleteHealthRecordById(int healthRecordId)
        {
            var existingRecord = _healthRecordDataAccess.GetHealthRecordById(healthRecordId);
            if (!_userLogic.UserCanEditVehicle(GetUserID(), existingRecord.VehicleId, HouseholdPermission.Delete))
            {
                return Json(OperationResponse.Failed("Access Denied"));
            }
            var result = _healthRecordDataAccess.DeleteHealthRecordById(existingRecord.Id);
            return Json(OperationResponse.Conditional(result, string.Empty, StaticHelper.GenericErrorMessage));
        }
    }
}
