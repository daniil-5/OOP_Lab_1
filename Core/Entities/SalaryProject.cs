
namespace OOP_Lab_1.Core.Entities;

public class SalaryProject
{
        public int Id { get; set; }
        public bool IsProcessed { get; set; }
        public int EnterpriseId { get; set; }
        public DateTime ApprovedDate { get; set; }
        public int Period { get; set; }
        
        public string EnterpriseName { get; set; }
}