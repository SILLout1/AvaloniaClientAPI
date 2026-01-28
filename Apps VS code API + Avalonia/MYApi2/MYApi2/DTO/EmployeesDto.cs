namespace MYApi2.DTO
{
    public class EmployeesDto
    {
        public int employeeId { get; set; }

        public string lastName { get; set; } = null!;

        public string firstName { get; set; } = null!;

        public string? middleName { get; set; }

        public DateOnly? birthDate { get; set; }

        public DateOnly? hireDate { get; set; }

        public decimal? salary { get; set; }

        public string? phone { get; set; }
    }
}
