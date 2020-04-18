using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cw2.Controllers
{
    public interface IDbService
    {
        public IEnumerable<Student> GetStudents();
    }
}
