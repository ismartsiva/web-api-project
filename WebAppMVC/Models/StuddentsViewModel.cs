namespace WebAppMVC.Models
{
    public class StuddentsViewModel
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public string RollNo { get; set; } = null!;

        public string? FatherName { get; set; }

        public string? MotherName { get; set; }

        public string? Address { get; set; }

        public DateTime? Dob { get; set; }

        public string? AdmissionNo { get; set; }

        public long? DeptmentId { get; set; }
    }
}
