namespace CarCareTracker.Models
{
    public class LicensingRecordInput
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public string Date { get; set; } = DateTime.Now.ToShortDateString();
        public string LicenseNumber { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public bool RenewalReminderEnabled { get; set; } = false;
        public int LinkedHealthRecordId { get; set; } = 0;
        public decimal Cost { get; set; }
        public string Notes { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<UploadedFiles> Files { get; set; } = new List<UploadedFiles>();
        public List<string> Tags { get; set; } = new List<string>();
        public List<ExtraField> ExtraFields { get; set; } = new List<ExtraField>();

        public LicensingRecord ToLicensingRecord()
        {
            return new LicensingRecord
            {
                Id = Id,
                VehicleId = VehicleId,
                Date = string.IsNullOrWhiteSpace(Date) ? DateTime.Now : DateTime.Parse(Date),
                LicenseNumber = LicenseNumber,
                Issuer = Issuer,
                ExpiryDate = ExpiryDate,
                RenewalReminderEnabled = RenewalReminderEnabled,
                LinkedHealthRecordId = LinkedHealthRecordId,
                Cost = Cost,
                Notes = Notes,
                Description = Description,
                Files = Files,
                Tags = Tags,
                ExtraFields = ExtraFields
            };
        }
    }
}
