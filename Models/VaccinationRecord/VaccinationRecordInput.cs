namespace CarCareTracker.Models
{
    public class VaccinationRecordInput
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        public string VaccineName { get; set; } = string.Empty;
        public string NextDueDate { get; set; } = string.Empty;
        public string LotNumber { get; set; } = string.Empty;
        public string AdministeredBy { get; set; } = string.Empty;
        public string Clinic { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public string Notes { get; set; } = string.Empty;
        public bool ReminderEnabled { get; set; } = false;
        public int LinkedHealthRecordId { get; set; } = 0;
        public List<UploadedFiles> Files { get; set; } = new List<UploadedFiles>();
        public List<string> Tags { get; set; } = new List<string>();
        public List<ExtraField> ExtraFields { get; set; } = new List<ExtraField>();

        public VaccinationRecord ToVaccinationRecord()
        {
            return new VaccinationRecord
            {
                Id = Id,
                VehicleId = VehicleId,
                Date = string.IsNullOrWhiteSpace(Date) ? DateTime.Now : DateTime.Parse(Date),
                VaccineName = VaccineName,
                NextDueDate = NextDueDate,
                LotNumber = LotNumber,
                AdministeredBy = AdministeredBy,
                Clinic = Clinic,
                Cost = Cost,
                Notes = Notes,
                ReminderEnabled = ReminderEnabled,
                LinkedHealthRecordId = LinkedHealthRecordId,
                Files = Files,
                Tags = Tags,
                ExtraFields = ExtraFields
            };
        }
    }
}
