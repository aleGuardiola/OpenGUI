using System;
using System.Collections.Generic;
using System.Text;

namespace OpenGui.Exceptions
{
    public class MemberNotFoundException : Exception
    {
        public MemberNotFoundException(string memberName) : base($"The member {memberName} cant be found.")
        {

        }
    }
}
