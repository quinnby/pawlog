using System.Text.Json.Serialization;

namespace CarCareTracker.Models
{
    /// <summary>
    /// Phase 4 – Structured medication record for a pet.
    /// Date from GenericRecord is used as the start/administration date.
    /// </summary>
    public class MedicationRecord : GenericRecord
    {
        /// <summary>Name of the medication (e.g. "Apoquel", "Heartgard").</summary>
        public string MedicationName { get; set; } = string.Empty;

        /// <summary>Dosage amount (e.g. "16", "1 tablet").</summary>
        public string Dosage { get; set; } = string.Empty;

        /// <summary>Dosage unit (e.g. "mg", "mL", "tablet").</summary>
        public string Unit { get; set; } = string.Empty;

        /// <summary>How often to give the medication (e.g. "Once daily", "Every 12 hours").</summary>
        public string Frequency { get; set; } = string.Empty;

        /// <summary>Route of administration (e.g. "Oral", "Topical", "Injectable").</summary>
        public string Route { get; set; } = string.Empty;

        /// <summary>Date the medication course ends (optional).</summary>
        [JsonConverter(typeof(FromDateOptional))]
        public string EndDate { get; set; } = string.Empty;

        /// <summary>Veterinarian who prescribed the medication.</summary>
        public string PrescribingVet { get; set; } = string.Empty;

        /// <summary>Reason or clinical indication for the medication.</summary>
        public string Purpose { get; set; } = string.Empty;

        /// <summary>Date when a refill is needed (optional).</summary>
        [JsonConverter(typeof(FromDateOptional))]
        public string RefillDate { get; set; } = string.Empty;

        /// <summary>Whether a refill reminder should fire on RefillDate.</summary>
        public bool ReminderEnabled { get; set; } = false;

        /// <summary>Whether this medication is currently being administered.</summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Optional reference back to a generic HealthRecord on the main timeline.
        /// 0 means no linked record.
        /// </summary>
        public int LinkedHealthRecordId { get; set; } = 0;
    }
}
