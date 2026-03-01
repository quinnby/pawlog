using System.Text.Json.Serialization;

namespace CarCareTracker.Models
{
    /// <summary>
    /// Phase 4 – Structured vaccination record for a pet.
    /// Extends GenericRecord to reuse Id, VehicleId (PetId), Date, Cost, Notes, Files, Tags, ExtraFields.
    /// Mileage inherited from GenericRecord is unused / kept at 0.
    /// </summary>
    public class VaccinationRecord : GenericRecord
    {
        /// <summary>Name of the vaccine administered (e.g. "Rabies", "DHPP").</summary>
        public string VaccineName { get; set; } = string.Empty;

        /// <summary>Date the next booster / dose is due.</summary>
        [JsonConverter(typeof(FromDateOptional))]
        public string NextDueDate { get; set; } = string.Empty;

        /// <summary>Vaccine lot or batch number (optional).</summary>
        public string LotNumber { get; set; } = string.Empty;

        /// <summary>Veterinarian or technician who administered the vaccine.</summary>
        public string AdministeredBy { get; set; } = string.Empty;

        /// <summary>Clinic or practice where vaccination occurred.</summary>
        public string Clinic { get; set; } = string.Empty;

        /// <summary>Whether a renewal reminder should be created when NextDueDate is set.</summary>
        public bool ReminderEnabled { get; set; } = false;

        /// <summary>
        /// Optional reference back to a generic HealthRecord on the main timeline.
        /// 0 means no linked record.
        /// </summary>
        public int LinkedHealthRecordId { get; set; } = 0;
    }
}
