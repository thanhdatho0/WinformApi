using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    [Table("Employees")]
    public class Employee : Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [DefaultValue("")]
        public string EmployeeCode { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public required string Enail { get; set; }
        public string? Avatar { get; set; }
        public List<Order> Orders { get; set; } = [];
    }
}