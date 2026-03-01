using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ue_mes_data.Authorization.Interface;
using ue_mes_entities.Authorization;
using ue_mes_logic.Authorization;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace ue_mes_api.Middleware
{
    public class AuthenticationMiddleware : IAuthenticationMiddleware
    {
        IUserData UserData { get; set; }

        public AuthenticationMiddleware(IUserData userData) 
        {
            UserData = userData ?? throw new ArgumentNullException(nameof(userData));
        }

        public User Authenticate(GoogleJsonWebSignature.Payload payload)
        {
            return this.FindUserOrAdd(payload);
        }

        private static IList<User> users = new List<User>();

        private User FindUserOrAdd(Payload payload)
        {
            using (var userLogic = new UserLogic(UserData))
            {
                // Need to handle inactive users at some point.  Maybe throw an exception
                var user = userLogic.GetUserByEmail(payload.Email,true);
                if (user != null)
                    return user;

                // Since the user does not exist, we will attempt to add them and then return the newly added object
                var newUser = new User()
                {
                    LoginName = payload.Email.Substring(0, payload.Email.IndexOf('@')),
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    EmailAddress = payload.Email,
                    JobTitle = string.Empty,
                    IsActive = true
                };
                userLogic.AddUser(newUser);
                users.Add(newUser);
         
                user = userLogic.GetUserByEmail(payload.Email);
                return user;
            }
        }

        public static string Encrypt(string key, string toEncrypt)
        {
            using (Aes aes = Aes.Create())
            {
                byte[] resultArray = null;
                try
                {
                    byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                    byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Create crypto stream using the CryptoStream class. This class is the key to encryption
                        // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream
                        // to encrypt
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            // Create StreamWriter and write data to a stream
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                                streamWriter.Write(toEncrypt);
                            resultArray = memoryStream.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        public static string Decrypt(string key, string toDecrypt)
        {
            using (Aes aes = Aes.Create())
            {
                string resultString = String.Empty;
                try
                {
                    byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);
                    byte[] toDecryptArray = UTF8Encoding.UTF8.GetBytes(toDecrypt);

                    keyArray = UTF8Encoding.UTF8.GetBytes(key);

                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        // Create crypto stream using the CryptoStream class. This class is the key to encryption
                        // and encrypts and decrypts data from any given stream. In this case, we will pass a memory stream
                        // to encrypt
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            // Create StreamWriter and write data to a stream
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                                resultString = streamReader.ReadToEnd();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return resultString;
            }
        }
    }
}
