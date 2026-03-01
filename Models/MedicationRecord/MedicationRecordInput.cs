namespace CarCareTracker.Models
{
    public class MedicationRecordInput
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
        public string Route { get; set; } = string.Empty;
        public string EndDate { get; set; } = string.Empty;
        public string PrescribingVet { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public string RefillDate { get; set; } = string.Empty;
        public bool ReminderEnabled { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int LinkedHealthRecordId { get; set; } = 0;
        public decimal Cost { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<UploadedFiles> Files { get; set; } = new List<UploadedFiles>();
        public List<string> Tags { get; set; } = new List<string>();
        public List<ExtraField> ExtraFields { get; set; } = new List<ExtraField>();

        public MedicationRecord ToMedicationRecord()
        {
            return new MedicationRecord
            {
                Id = Id,
                VehicleId = VehicleId,
                Date = string.IsNullOrWhiteSpace(Date) ? DateTime.Now : DateTime.Parse(Date),
                MedicationName = MedicationName,
                Dosage = Dosage,
                Unit = Unit,
                Frequency = Frequency,
                Route = Route,
                EndDate = EndDate,
                PrescribingVet = PrescribingVet,
                Purpose = Purpose,
                RefillDate = RefillDate,
                ReminderEnabled = ReminderEnabled,
                IsActive = IsActive,
                LinkedHealthRecordId = LinkedHealthRecordId,
                Cost = Cost,
                Notes = Notes,
                Files = Files,
                Tags = Tags,
                ExtraFields = ExtraFields
            };
        }
    }
}
