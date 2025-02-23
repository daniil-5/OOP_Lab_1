using OOP_Lab_1.Core.Interfaces;

namespace OOP_Lab_1.Core.Entities;

public class EnterpriseSpecialist : User, IEnterpriseSpecialistActions
{
    public EnterpriseSpecialist(User user) : base(user)
    {
        Role = 3; // Enterprise Specialist
    }

    public void SubmitDocsForSalaryProject()
    {
        Console.WriteLine($"{FullName} (Enterprise Specialist) submitted a salary project.");
    }

    public void RequestFundTransfer()
    {
        Console.WriteLine($"{FullName} (Enterprise Specialist) requested a fund transfer.");
    }
}