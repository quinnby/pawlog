namespace CarCareTracker.Models
{
    public class ReminderRecord
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public DateTime Date { get; set; }
        public int Mileage { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public bool IsRecurring { get; set; } = false;
        public bool UseCustomThresholds { get; set; } = false;
        public bool FixedIntervals { get; set; } = false;
        public ReminderUrgencyConfig CustomThresholds { get; set; } = new ReminderUrgencyConfig();
        public int CustomMileageInterval { get; set; } = 0;
        public int CustomMonthInterval { get; set; } = 0;
        public ReminderIntervalUnit CustomMonthIntervalUnit { get; set; } = ReminderIntervalUnit.Months;
        public ReminderMileageInterval ReminderMileageInterval { get; set; } = ReminderMileageInterval.FiveThousandMiles;
        public ReminderMonthInterval ReminderMonthInterval { get; set; } = ReminderMonthInterval.OneYear;
        public ReminderMetric Metric { get; set; } = ReminderMetric.Date;
        public List<string> Tags { get; set; } = new List<string>();

        // Phase 5 – Pet care reminder fields
        /// <summary>Category of pet care this reminder covers.</summary>
        public PetReminderType PetReminderType { get; set; } = PetReminderType.Custom;
        /// <summary>Which type of source record created this reminder (None = manual).</summary>
        public ReminderLinkedRecordType LinkedRecordType { get; set; } = ReminderLinkedRecordType.None;
        /// <summary>Id of the source record that created this reminder (0 = manual / unlinked).</summary>
        public int LinkedRecordId { get; set; } = 0;
    }
}
