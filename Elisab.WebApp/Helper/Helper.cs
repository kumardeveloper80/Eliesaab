using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Elisab.WebApp.Helper
{
  public static class Helper
  {
    public static string Encrypt(string plainText)
    {
      string EncryptionKey = Convert.ToString(ConfigurationManager.AppSettings.Get("EncryptionKey"));
      byte[] clearBytes = Encoding.Unicode.GetBytes(plainText);
      using (Aes encryptor = Aes.Create())
      {
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using (MemoryStream ms = new MemoryStream())
        {
          using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
          {
            cs.Write(clearBytes, 0, clearBytes.Length);
            cs.Close();
          }
          plainText = Convert.ToBase64String(ms.ToArray());
        }
      }
      return plainText;
    }

    public static string Decrypt(string cipherText)
    {
      string EncryptionKey = Convert.ToString(ConfigurationManager.AppSettings.Get("EncryptionKey"));
      byte[] cipherBytes = Convert.FromBase64String(cipherText);
      using (Aes encryptor = Aes.Create())
      {
        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
        encryptor.Key = pdb.GetBytes(32);
        encryptor.IV = pdb.GetBytes(16);
        using (MemoryStream ms = new MemoryStream())
        {
          using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
          {
            cs.Write(cipherBytes, 0, cipherBytes.Length);
            cs.Close();
          }
          cipherText = Encoding.Unicode.GetString(ms.ToArray());
        }
      }
      return cipherText;
    }



    public static string EncodeServerName(string serverName)
    {
      return Convert.ToBase64String(Encoding.UTF8.GetBytes(serverName));
    }

    public static string DecodeServerName(string encodedServername)
    {
      return Encoding.UTF8.GetString(Convert.FromBase64String(encodedServername));
    }


    public static Dictionary<string, string> type()
    {
      Dictionary<string, string> mediaType = new Dictionary<string, string>();
      mediaType.Add("Video", "Video");
      mediaType.Add("Text", "Text");
      mediaType.Add("Image And Text", "Image And Text");
      mediaType.Add("Image", "Image");
      return mediaType;
    }

    public static string ImageType = "Image";
    public static string ImageTxtType = "Image And Text";
    public static string VideoType = "Video";
    public static string TextType = "Text";
    static Random random = new Random();

    public static int success_code = 1;
    public static int failure_code = 0;
    public static int sessionout_code = -3;
    public static int error_code = -2;

    public static string FashionShow = "Fashion show";
    public static string MainPage = "Main page";
    public static string SecondPage = "Second page";
    public static string Section = "Section";
    public static string AddressSection = "Address section";
    public static string ImageGallery = "Image";
    public static string CountDownPage = "Count down page";

    public static string emailBody = "Please use the following username and password to access your account:<br/><br/>";
    public static string invalidLogin = "Invalid username or password";
    public static string session_out = "Session time out";
    public static string emailSent_success = "Email sent";
    public static string emailSent_error = "Email not sent";
    public static string invalidEmail = "Invalid email address";
    
    public static string recordAdd_success = " added successfully";
    public static string recordAdd_unsuccess = " added un-successfully";
    public static string recordUpdate_success = " updated successfully";
    public static string recordUpdate_unsuccess = " updated un-successfully";
    public static string recordDelete_success = " deleted successfully";
    public static string recordDelete_unsuccess = " deleted un-successfully";
    public static string recordNotFound = " not found";

    public static string GenerateRandomString(int nlength)
    {
      char[] keys = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890abcdefghijklmnopqrstuvwxyz123456789".ToCharArray();
      return Enumerable
          .Range(1, nlength) // for(i.. ) 
          .Select(k => keys[random.Next(0, keys.Length - 1)])  // generate a new random char 
          .Aggregate("", (e, c) => e + c); // join into a string
    }

    public static string UnqiueId()
    {
      return DateTime.Now.ToString("yyyyMMddHHmmssff");
    }

  }

  public class CommonResponse<T>
  {
    public int status { get; set; }
    public string message { get; set; }
    public T dataenum { get; set; }
  }
}
