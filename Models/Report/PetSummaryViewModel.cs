namespace CarCareTracker.Models
{
    /// <summary>
    /// Phase 7 – View model for the printable / exportable per-pet summary.
    /// Aggregates the most clinically relevant information a vet, boarder, or
    /// emergency contact would need in a single printable view.
    /// Reuses existing record types; no new database collections required.
    /// </summary>
    public class PetSummaryViewModel
    {
        /// <summary>The pet profile (Vehicle model reused as Pet).</summary>
        public Vehicle PetData { get; set; } = new Vehicle();

        /// <summary>All vaccination records, most-recent first.</summary>
        public List<VaccinationRecord> Vaccinations { get; set; } = new List<VaccinationRecord>();

        /// <summary>Active medication records only.</summary>
        public List<MedicationRecord> ActiveMedications { get; set; } = new List<MedicationRecord>();

        /// <summary>Known allergies – HealthRecords with AllergyReaction category.</summary>
        public List<HealthRecord> KnownAllergies { get; set; } = new List<HealthRecord>();

        /// <summary>Upcoming reminders within the next 90 days, sorted by date.</summary>
        public List<ReminderRecord> UpcomingReminders { get; set; } = new List<ReminderRecord>();

        /// <summary>Recent HealthRecords (last 12 months) for key care categories.</summary>
        public List<HealthRecord> RecentHealthRecords { get; set; } = new List<HealthRecord>();

        /// <summary>Weight history entries (WeightCheck category, WeightValue > 0), most-recent first.</summary>
        public List<HealthRecord> WeightHistory { get; set; } = new List<HealthRecord>();

        /// <summary>Date range string shown on the printed report.</summary>
        public string GeneratedDate { get; set; } = DateTime.Now.ToShortDateString();
    }
}
