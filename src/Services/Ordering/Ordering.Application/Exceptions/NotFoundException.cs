using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string message) : base(message)
        {
        }

        public NotFoundException(string name, object key) : base($"Entity > {name} < his > {key} < was not found."){}
    }
}
