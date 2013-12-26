using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace IKende.com.core
{
    public class RSACrypto
    {
        RSACryptoServiceProvider rsaProvider;
        public RSACrypto()
        {
            rsaProvider = new RSACryptoServiceProvider(1024);
        }
        public RSACrypto(int bit)
        {
            rsaProvider = new RSACryptoServiceProvider(bit);
        }
        public string PublicKey
        {
            get
            {
                return rsaProvider.ToXmlString(false);
            }
            set
            {
                rsaProvider.FromXmlString(value);
            }
        }
        public void CopyRSAParameters(bool includePrivateParameters, RSACrypto rsa)
        {
            RSAParameters parameters = rsaProvider.ExportParameters(includePrivateParameters);
            rsa.rsaProvider.ImportParameters(parameters);
        }
        public RSAParameters ExportParameters(bool includePrivateParameters)
        {
            return rsaProvider.ExportParameters(includePrivateParameters);
        }
        public void ImportParameters(RSAParameters parameters)
        {
            rsaProvider.ImportParameters(parameters);
        }
        public string PrivateKey
        {
            get
            {
                return rsaProvider.ToXmlString(true);
            }
            set
            {
                rsaProvider.FromXmlString(value);
            }
        }
        public string Sign(string data)
        {
            return Convert.ToBase64String(Sign(System.Text.Encoding.UTF8.GetBytes(data)));
        }
        public byte[] Sign(byte[] data)
        {

            return rsaProvider.SignData(data, "MD5");
        }
        public bool Verify(string data, string signature)
        {
            return Verify(System.Text.Encoding.UTF8.GetBytes(data), Convert.FromBase64String(signature));
        }
        public bool Verify(byte[] data, byte[] Signature)
        {

            return rsaProvider.VerifyData(data, "MD5", Signature);
        }
        public string Encrypt(string data)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(data)));
        }
        public string Decrypt(string data)
        {
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(data)));
        }
        public byte[] Encrypt(byte[] data)
        {

            return rsaProvider.Encrypt(data, false);
        }
        public byte[] Decrypt(byte[] data)
        {
            return rsaProvider.Decrypt(data, false);
        }
        public string GetRSAParametersData(bool includePrivateParameters)
        {
            RSAParameters parameters = rsaProvider.ExportParameters(includePrivateParameters);
            StringBuilder sb = new StringBuilder();
            sb.Append(parameters.D != null ? Convert.ToBase64String(parameters.D) : "").Append("\n");
            sb.Append(parameters.DP != null ? Convert.ToBase64String(parameters.DP) : "").Append("\n");
            sb.Append(parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : "").Append("\n");
            sb.Append(parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : "").Append("\n");
            sb.Append(parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : "").Append("\n");
            sb.Append(parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : "").Append("\n");
            sb.Append(parameters.P != null ? Convert.ToBase64String(parameters.P) : "").Append("\n");
            sb.Append(parameters.Q != null ? Convert.ToBase64String(parameters.Q) : "").Append("\n");
            return sb.ToString();
        }
        public void SetRSAParameters(string data)
        {
            RSAParameters rsa = new RSAParameters();
            string[] values = data.Split('\n');
            rsa.D = string.IsNullOrEmpty(values[0]) ? null : Convert.FromBase64String(values[0]);
            rsa.DP = string.IsNullOrEmpty(values[1]) ? null : Convert.FromBase64String(values[1]);
            rsa.DQ = string.IsNullOrEmpty(values[2]) ? null : Convert.FromBase64String(values[2]);
            rsa.Exponent = string.IsNullOrEmpty(values[3]) ? null : Convert.FromBase64String(values[3]);
            rsa.Modulus = string.IsNullOrEmpty(values[4]) ? null : Convert.FromBase64String(values[4]);
            rsa.Modulus = string.IsNullOrEmpty(values[5]) ? null : Convert.FromBase64String(values[5]);
            rsa.P = string.IsNullOrEmpty(values[6]) ? null : Convert.FromBase64String(values[6]);
            rsa.Q = string.IsNullOrEmpty(values[7]) ? null : Convert.FromBase64String(values[7]);
            rsaProvider.ImportParameters(rsa);
        }
    }
}
