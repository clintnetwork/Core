using System;
using System.Security.Cryptography;
using System.Text;

// namespace TypeDB.Security
// {
//     internal static class ObjectSigning
//     {
//         public static void test()
//         {
//             ASCIIEncoding ByteConverter = new ASCIIEncoding();

//             string dataString = "Data to Sign";

//             // Create byte arrays to hold original, encrypted, and decrypted data.
//             byte[] originalData = ByteConverter.GetBytes(dataString);
//             byte[] signedData;

//             // Create a new instance of the RSACryptoServiceProvider class 
//             // and automatically create a new key-pair.
//             RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

//             // Export the key information to an RSAParameters object.
//             // You must pass true to export the private key for signing.
//             // However, you do not need to export the private key
//             // for verification.
//             RSAParameters Key = RSAalg.ExportParameters(true);

//             Console.WriteLine(Key.);

//             // Hash and sign the data.
//             signedData = HashAndSignBytes(originalData, Key);

//             // Inflate the original datas
//             originalData = Encoding.UTF8.GetBytes("data to sign");
            
//             // Verify the data and display the result to the 
//             // console.
//             if (VerifySignedHash(originalData, signedData, Key))
//             {
//                 Console.WriteLine("The data was verified.");
//             }
//             else
//             {
//                 Console.WriteLine("The data does not match the signature.");
//             }
//         }
//         public static byte[] HashAndSignBytes(byte[] DataToSign, RSAParameters Key)
//         {
//             try
//             {
//                 // Create a new instance of RSACryptoServiceProvider using the 
//                 // key from RSAParameters.  
//                 RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

//                 RSAalg.ImportParameters(Key);

//                 // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
//                 // to specify the use of SHA1 for hashing.
//                 return RSAalg.SignData(DataToSign, new SHA1CryptoServiceProvider());
//             }
//             catch (CryptographicException e)
//             {
//                 Console.WriteLine(e.Message);

//                 return null;
//             }
//         }

//         public static bool VerifySignedHash(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
//         {
//             try
//             {
//                 // Create a new instance of RSACryptoServiceProvider using the 
//                 // key from RSAParameters.
//                 RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

//                 RSAalg.ImportParameters(Key);

//                 // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
//                 // to specify the use of SHA1 for hashing.
//                 return RSAalg.VerifyData(DataToVerify, new SHA1CryptoServiceProvider(), SignedData);

//             }
//             catch (CryptographicException e)
//             {
//                 Console.WriteLine(e.Message);

//                 return false;
//             }
//         }
//     }
// }