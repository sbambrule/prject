using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Exceptions
{
    public class UserNotExistsException : ApplicationException
    {
        public UserNotExistsException() : base(){ }
        public UserNotExistsException(string message) : base(message) { }
    }
}
