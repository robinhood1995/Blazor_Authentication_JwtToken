
namespace BaseLibrary.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        
        public string? Name { get; set; }

        public string? SSN { get; set; }
        
        public string? FileNumber { get; set; }
        
        public string? FirstName { get; set; }
        
        public string? MiddleName { get; set; }
        
        public string? LastName { get; set; }
        
        public string? PersonalEmail { get; set; }
        
        public string? Street { get; set; }

        public string? ZipCode { get; set; }

        public Guid? TenantId { get; set; }

        //Relationship : Many to One
        public GeneralDepartment? GeneralDepartment { get; set; }
        public Guid GeneralDepartmentId { get; set; }

        public Department? Department { get; set; }
        public Guid DepartmentId { get; set; }

        public Branch? Branch { get; set; }
        public Guid BranchId { get; set; }

        public City? City { get; set; }
        public Guid CityId { get; set; }

    }
}