using System;
using System.Collections.Generic;
using WebAppMVC.Models;

namespace WebApplication1.Models;

public partial class Department
{
    public long Id { get; set; }

    public string DeptName { get; set; } = null!;

    public string DeptCode { get; set; } = null!;

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
