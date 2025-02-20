namespace OOP_Lab_1.Core.Entities;

public class Role
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<Permission> Permissions { get; set; }

    public Role(string id, string name)
    {
        Id = id;
        Name = name;
        Permissions = new List<Permission>();
    }

    public void AddPermission(Permission permission)
    {
        Permissions.Add(permission);
    }

    public void RemovePermission(Permission permission)
    {
        Permissions.Remove(permission);
    }

    public bool HasPermission(Permission permission)
    {
        return Permissions.Exists(p => p.Id == permission.Id);
    }
}