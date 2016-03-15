using Model.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Interfaces
{
    public interface IAudit
    {
        int Id { get; set; }
        EnumSubjectType SubjectType { get; }
    }
}
