using System.Text.Json.Serialization;

namespace CarCareTracker.Models
{
    /// <summary>
    /// Phase 4 – Pet licensing / registration record.
    /// Date from GenericRecord is the issue date of the license.
    /// </summary>
    public class LicensingRecord : GenericRecord
    {
        /// <summary>Official license or tag number issued by the municipality / authority.</summary>
        public string LicenseNumber { get; set; } = string.Empty;

        /// <summary>Issuing authority or municipality (e.g. "City of Austin", "King County").</summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>Date the license expires or must be renewed.</summary>
        [JsonConverter(typeof(FromDateOptional))]
        public string ExpiryDate { get; set; } = string.Empty;

        /// <summary>Whether a reminder should fire before the ExpiryDate.</summary>
        public bool RenewalReminderEnabled { get; set; } = false;

        /// <summary>
        /// Optional reference back to a generic HealthRecord on the main timeline.
        /// 0 means no linked record.
        /// </summary>
        public int LinkedHealthRecordId { get; set; } = 0;
    }
}
