using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KFileStorage
{
    public interface IDataFormater
    {
        ArraySegment<byte> Write(object data,byte[] recordBuffer);

        object Read(ArraySegment<byte> data);
    }
}
