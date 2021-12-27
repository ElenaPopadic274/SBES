using Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DES_Symm_Algorithm
    {
        public static void EncryptFile(string inFile, string outFile, string secretKey, CipherMode mode)
        {
            byte[] broj = null;
            byte[] encryptedBroj = null;
            /*
            byte[] header = null;   //image header (54 byte) should not be encrypted
            byte[] body = null;     //image body to be encrypted
            byte[] encryptedBody = null;

            Formatter.Decompose(File.ReadAllBytes(inFile), out header, out body);
            */
            DESCryptoServiceProvider desCryptoProvider = new DESCryptoServiceProvider();
            desCryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(secretKey);
            desCryptoProvider.Mode = mode;
            desCryptoProvider.Padding = PaddingMode.None;

            
            if (mode.Equals(CipherMode.CBC))
            {
                desCryptoProvider.GenerateIV();

                ICryptoTransform desEncryptTransform = desCryptoProvider.CreateEncryptor();
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desEncryptTransform, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(broj, 0, broj.Length);

                        encryptedBroj = desCryptoProvider.IV.Concat(memoryStream.ToArray()).ToArray();    //encrypted with IV
                    }
                }

            }

            //int outputLenght = encryptedBroj.Length;                 //header.Length + body.Length
            //Formatter.Compose(header, encryptedBody, outputLenght, outFile);
        }


        /// <summary>
        /// Function that decrypts the cipher text from inFile and stores as plaintext to outFile
        /// </summary>
        /// <param name="inFile"> filepath where cipher text is stored </param>
        /// <param name="outFile"> filepath where plain text is expected to be stored </param>
        /// <param name="secretKey"> symmetric encryption key </param>
        public static void DecryptFile(string inFile, string outFile, string secretKey, CipherMode mode)
        {
            //byte[] header = null;       //image header (54 byte) should not be decrypted
            //byte[] body = null;         //image body to be decrypted
            byte[] broj = null;
            byte[] decryptedBroj = null;

            //Formatter.Decompose(File.ReadAllBytes(inFile), out header, out body);

            DESCryptoServiceProvider desCryptoProvider = new DESCryptoServiceProvider();
            desCryptoProvider.Key = ASCIIEncoding.ASCII.GetBytes(secretKey);
            desCryptoProvider.Mode = mode;
            desCryptoProvider.Padding = PaddingMode.None;

           
            if (mode.Equals(CipherMode.CBC))
            {
                desCryptoProvider.IV = broj.Take(desCryptoProvider.BlockSize / 8).ToArray();                // take the iv off the beginning of the ciphertext message			

                ICryptoTransform desDecryptTransform = desCryptoProvider.CreateDecryptor();

                using (MemoryStream memoryStream = new MemoryStream(broj.Skip(desCryptoProvider.BlockSize / 8).ToArray()))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, desDecryptTransform, CryptoStreamMode.Read))
                    {
                        //decryptedBroj = new byte[broj.Length - desCryptoProvider.BlockSize / 8];     //decrypted image body - the same lenght as encrypted part
                        cryptoStream.Read(decryptedBroj, 0, decryptedBroj.Length);
                    }
                }
            }

            //int outputLenght = header.Length + decryptedBody.Length;

            //Formatter.Compose(header, decryptedBody, outputLenght, outFile);
        }
    }
}
