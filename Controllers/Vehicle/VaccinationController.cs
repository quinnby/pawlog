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
        public IActionResult GetVaccinationRecordsByVehicleId(int vehicleId)
        {
            var result = _vaccinationRecordDataAccess.GetVaccinationRecordsByVehicleId(vehicleId);
            bool _useDescending = _config.GetUserConfig(User).UseDescending;
            if (_useDescending)
            {
                result = result.OrderByDescending(x => x.Date).ToList();
            }
            else
            {
                result = result.OrderBy(x => x.Date).ToList();
            }
            return PartialView("Vaccination/_VaccinationRecords", result);
        }

        [HttpGet]
        public IActionResult GetAddVaccinationRecordPartialView()
        {
            return PartialView("Vaccination/_VaccinationRecordModal", new VaccinationRecordInput());
        }

        [HttpGet]
        public IActionResult GetVaccinationRecordForEditById(int vaccinationRecordId)
        {
            var result = _vaccinationRecordDataAccess.GetVaccinationRecordById(vaccinationRecordId);
            if (!_userLogic.UserCanEditVehicle(GetUserID(), result.VehicleId, HouseholdPermission.View))
            {
                return Redirect("/Error/Unauthorized");
            }
            var convertedResult = new VaccinationRecordInput
            {
                Id = result.Id,
                VehicleId = result.VehicleId,
                Date = result.Date.ToShortDateString(),
                VaccineName = result.VaccineName,
                NextDueDate = result.NextDueDate,
                LotNumber = result.LotNumber,
                AdministeredBy = result.AdministeredBy,
                Clinic = result.Clinic,
                Cost = result.Cost,
                Notes = result.Notes,
                ReminderEnabled = result.ReminderEnabled,
                LinkedHealthRecordId = result.LinkedHealthRecordId,
                Files = result.Files,
                Tags = result.Tags,
                ExtraFields = result.ExtraFields
            };
            return PartialView("Vaccination/_VaccinationRecordModal", convertedResult);
        }

        [HttpPost]
        public IActionResult SaveVaccinationRecordToVehicleId(VaccinationRecordInput vaccinationRecord)
        {
            if (!_userLogic.UserCanEditVehicle(GetUserID(), vaccinationRecord.VehicleId, HouseholdPermission.Edit))
            {
                return Json(OperationResponse.Failed("Access Denied"));
            }
            vaccinationRecord.Files = vaccinationRecord.Files
                .Select(x => new UploadedFiles
                {
                    Name = x.Name,
                    Location = _fileHelper.MoveFileFromTemp(x.Location, "documents/")
                }).ToList();

            var convertedRecord = vaccinationRecord.ToVaccinationRecord();
            var result = _vaccinationRecordDataAccess.SaveVaccinationRecord(convertedRecord);
            if (result)
            {
                // Phase 5 – Sync reminder when NextDueDate is set and ReminderEnabled is toggled
                SyncReminderFromLinkedRecord(
                    petId: convertedRecord.VehicleId,
                    reminderEnabled: vaccinationRecord.ReminderEnabled,
                    dueDateString: vaccinationRecord.NextDueDate,
                    description: $"Vaccination Due: {vaccinationRecord.VaccineName}",
                    petReminderType: PetReminderType.VaccinationDue,
                    linkedRecordType: ReminderLinkedRecordType.Vaccination,
                    linkedRecordId: convertedRecord.Id);
            }
            return Json(OperationResponse.Conditional(result, string.Empty, StaticHelper.GenericErrorMessage));
        }

        [HttpPost]
        public IActionResult DeleteVaccinationRecordById(int vaccinationRecordId)
        {
            var existingRecord = _vaccinationRecordDataAccess.GetVaccinationRecordById(vaccinationRecordId);
            if (!_userLogic.UserCanEditVehicle(GetUserID(), existingRecord.VehicleId, HouseholdPermission.Delete))
            {
                return Json(OperationResponse.Failed("Access Denied"));
            }
            var result = _vaccinationRecordDataAccess.DeleteVaccinationRecordById(existingRecord.Id);
            return Json(OperationResponse.Conditional(result, string.Empty, StaticHelper.GenericErrorMessage));
        }
    }
}
