namespace OOP_Lab_1.Core.Entities;

public class Permission
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public Permission(string id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}