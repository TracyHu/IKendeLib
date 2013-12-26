using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFileStorage
{
    public class FMException:Exception
    {
        public FMException(string error)
            : base(error)
        {

        }
        public FMException(string error, Exception innererror)
            : base(error, innererror)
        {

        }
    }
}
