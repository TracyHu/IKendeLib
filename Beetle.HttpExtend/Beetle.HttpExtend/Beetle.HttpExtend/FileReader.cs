using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Beetle.HttpExtend
{
    public class FileReader:IDisposable
    {
        public FileReader(string filename)
        {
            mFileName = filename;
            if (System.IO.File.Exists(filename))
            {
                mFileInfo = new System.IO.FileInfo(filename);
                mReadCount = mFileInfo.Length;
                mStream = System.IO.File.Open(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
            }
        }

        private string mFileName;

        private System.IO.FileInfo mFileInfo=null;

        private long mReadCount = 0;

        private System.IO.Stream mStream;

        public System.IO.FileInfo FileInfo
        {
            get
            {
                return mFileInfo;
            }
        }

        public bool Read()
        {
            return mReadCount > 0;
        }

        public void ReadTo(HttpBody body)
        {
            long length = mReadCount >= body.Data.BufferLength ? body.Data.BufferLength : mReadCount;
            mStream.Read(body.Data.Array, 0, (int)length);
            body.Data.SetInfo(0, (int)length);
            mReadCount -= length;
            if (mReadCount == 0)
                body.Eof = true;
        }

        public void Dispose()
        {
            if (mStream != null)
                mStream.Dispose();
        }
    }
}
