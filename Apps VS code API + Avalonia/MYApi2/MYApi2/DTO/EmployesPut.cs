namespace MYApi2.DTO
{
    public class EmployesPut
    {
        public string? lastName { get; set; }
    public string? firstName { get; set; }
    public string? middleName { get; set; }
    public DateTime? birthDate { get; set; }
    public DateTime? hireDate { get; set; }
    public decimal? salary { get; set; }
    public string? phone { get; set; }
    public int? positionCode { get; set; }
    }
}
