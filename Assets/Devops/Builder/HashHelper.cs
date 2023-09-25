using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class HashHelper
{	
	public static string SaltName
		=> Application.productName.Replace(" ", "");

	public static string AppConfigName
		=> "appInfo";

	public static string GenerateUniqueString(string input, string salt)
	{
		if (string.IsNullOrEmpty(salt))
			return input;
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input + salt);
		using MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
		byte[] hashedBytes = provider.ComputeHash(inputBytes);
		string result = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
		UnityEngine.Debug.Log($"Salt: {salt}\nGenerated result: {result}");
		return result;
	}
	
	public static string Encrypt(string plainText, string salt)
	{
		if (string.IsNullOrEmpty(salt))
			return plainText;
		
		byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

		using (RijndaelManaged cipher = new RijndaelManaged())
		{
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes("penis", Encoding.UTF8.GetBytes(salt));

			cipher.Key = pdb.GetBytes(32);
			cipher.IV = pdb.GetBytes(16);

			using (MemoryStream ms = new MemoryStream())
			{
				using (CryptoStream cs = new CryptoStream(ms, cipher.CreateEncryptor(), CryptoStreamMode.Write))
				{
					cs.Write(plainBytes, 0, plainBytes.Length);
					cs.Close();
				}

				string cipherText = Convert.ToBase64String(ms.ToArray());

				return cipherText;
			}
		}
	}

	public static string Decrypt(string cipherText, string salt)
	{
		if (string.IsNullOrEmpty(salt))
			return cipherText;
		    
		byte[] cipherBytes = Convert.FromBase64String(cipherText);

		using (RijndaelManaged cipher = new RijndaelManaged())
		{
			Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes("penis", Encoding.UTF8.GetBytes(salt));

			cipher.Key = pdb.GetBytes(32);
			cipher.IV = pdb.GetBytes(16);

			using (MemoryStream ms = new MemoryStream())
			{
				using (CryptoStream cs = new CryptoStream(ms, cipher.CreateDecryptor(), CryptoStreamMode.Write))
				{
					cs.Write(cipherBytes, 0, cipherBytes.Length);
					cs.Close();
				}

				string plainText = Encoding.UTF8.GetString(ms.ToArray());

				return plainText;
			}
		}
	}

}