using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PUPFMIS.Models
{
    public enum RequestStatus
    {
        NewRequest,
        Approved,
        Disapproved,
        Released,
        Cancelled
    }
}