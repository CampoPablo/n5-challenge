namespace n5.Application.Dto;

public class PermissionDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }    
    public Guid PermissionTypeId { get; set; }
    public string FirstName { get; set; }   
    public string LastName { get; set; }    
    public string Department { get; set; }
    public string IdentificationDocument { get; set; }  
    public string CodePermission { get; set; }    
    public string DescriptionPermission { get; set; } 
}
