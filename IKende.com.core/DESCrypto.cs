using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace IKende.com.core
{
    public class DESCrypto
    {


        private DESCryptoServiceProvider key = new DESCryptoServiceProvider();
        public DESCrypto()
        {
        }

        public DESInfo KeyInfo
        {
            get
            {
                DESInfo info = new DESInfo();
                if (PublicRas != null)
                {
                    info.Key = PublicRas.Encrypt(key.Key);
                    info.IV = PublicRas.Encrypt(key.IV);
                }
                else
                {
                    info.Key = key.Key;
                    info.IV = key.IV;
                }
                return info;
            }
            set
            {
                if (PrivateRas != null)
                {
                    key.Key = PrivateRas.Decrypt(value.Key);
                    key.IV = PrivateRas.Decrypt(value.IV);
                }
                else
                {
                    key.Key = value.Key;
                    key.IV = value.IV;
                }
            }
        }
        private DESCrypto mPrivateRas;
        public DESCrypto PrivateRas
        {
            get
            {
                return mPrivateRas;
            }
            set
            {
                mPrivateRas = value;
            }
        }
        private DESCrypto mPublicRas;
        public DESCrypto PublicRas
        {
            get
            {
                return mPublicRas;
            }
            set
            {
                mPublicRas = value;
            }
        }
        public class DESInfo
        {
            private byte[] mKey;
            public byte[] Key
            {
                get
                {
                    return mKey;
                }
                set
                {
                    mKey = value;
                }
            }
            private byte[] mIV;
            public byte[] IV
            {
                get
                {
                    return mIV;
                }
                set
                {
                    mIV = value;
                }
            }

        }
        public string Encrypt(string data)
        {
            return Convert.ToBase64String(Encrypt(System.Text.Encoding.UTF8.GetBytes(data)));
        }
        public byte[] Encrypt(byte[] data)
        {

            MemoryStream ms = new MemoryStream();
            CryptoStream encStream = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write);
            encStream.Write(data, 0, data.Length);
            encStream.Close();
            byte[] buffer = ms.ToArray();
            ms.Close();
            return buffer;
        }
        public string Decrypt(string data)
        {
            return System.Text.Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(data)));
        }
        public byte[] Decrypt(byte[] data)
        {

            IList<byte> result = new List<byte>();
            int value;
            MemoryStream ms = new MemoryStream(data);
            CryptoStream encStream = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read);
            value = encStream.ReadByte();
            while (value >= 0)
            {
                result.Add((byte)value);
                value = encStream.ReadByte();
            }
            encStream.Close();
            ms.Close();
            byte[] rdata = new byte[result.Count];
            result.CopyTo(rdata, 0);
            return rdata;

        }
    }
}
