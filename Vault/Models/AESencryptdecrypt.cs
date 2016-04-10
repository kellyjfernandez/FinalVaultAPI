using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Vault.Controllers;

namespace Vault.Models
{
    public class AESencryptdecrypt
    {

        private static Dictionary<string, string> dictionary = new Dictionary<string, string>();
        private string keyStr = System.Web.Configuration.WebConfigurationManager.AppSettings["keyStr"];
        private string ivStr = System.Web.Configuration.WebConfigurationManager.AppSettings["ivStr"];

        public string encrypt(string plainStr)
        {
            RijndaelManaged AesEncryption = new RijndaelManaged();
            AesEncryption.KeySize = 256; // 192, 256
            AesEncryption.BlockSize = 128;
            AesEncryption.Mode = CipherMode.CBC;
            AesEncryption.Padding = PaddingMode.PKCS7;

            // The key should be generated prior and also should be stored in secure repository
            // with appropriate ACL priviledges.
            //string keyStr = "k8fy3k5vcmQAAAAABAAADA==";
            //string ivStr = "gGFzc3hgdr5uhvd3j4l567==";
            byte[] ivArr = Convert.FromBase64String(ivStr);
            byte[] keyArr = Convert.FromBase64String(keyStr);
            AesEncryption.IV = ivArr;
            AesEncryption.Key = keyArr;

            // This array will contain the plain text in bytes
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(plainStr);

            // Creates Symmetric encryption and decryption objects   
            ICryptoTransform crypto = AesEncryption.CreateEncryptor();
            // The result of the encrypion and decryption
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
            dictionary[Convert.ToBase64String(cipherText)] = plainStr;
            return Convert.ToBase64String(cipherText);
        }

        public string decrypt(string cipherStr)
        {
            return dictionary[cipherStr];
        }
    }
}