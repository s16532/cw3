using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw2.Services
{
    public interface IStudentsDal
    {
        public IEnuemrable<IDbService> GetStudents();
    }
}
