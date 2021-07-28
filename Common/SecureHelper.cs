using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common
{
    /// <summary>
    /// 安全帮助类
    /// </summary>
    public class SecureHelper
    {
        #region AES加密、AES解密
        /// <summary>
        /// AES密钥向量
        /// </summary>
        private static readonly byte[] _aeskeys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="encryptStr">加密字符串</param>
        /// <param name="encryptKey">密钥</param>
        /// <returns></returns>
        public static string AESEncrypt(string encryptStr, string encryptKey)
        {
            if (string.IsNullOrWhiteSpace(encryptStr))
                return string.Empty;

            encryptKey = SubString(encryptKey, 0, 32);
            encryptKey = encryptKey.PadRight(32, ' ');

            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptStr);//得到需要加密的字节数组 
            //设置密钥及密钥向量
            des.Key = Encoding.UTF8.GetBytes(encryptKey);
            des.IV = _aeskeys;
            byte[] cipherBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();//得到加密后的字节数组
                    cs.Close();
                    ms.Close();
                }
            }
            return Convert.ToBase64String(cipherBytes);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="decryptStr">解密字符串</param>
        /// <param name="decryptKey">密钥</param>
        /// <returns></returns>
        public static string AESDecrypt(string decryptStr, string decryptKey)
        {
            if (string.IsNullOrWhiteSpace(decryptStr))
                return string.Empty;

            decryptKey = SubString(decryptKey, 0, 32);
            decryptKey = decryptKey.PadRight(32, ' ');

            byte[] cipherText = Convert.FromBase64String(decryptStr);

            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(decryptKey);
            des.IV = _aeskeys;
            byte[] decryptBytes = new byte[cipherText.Length];
            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");//将字符串后尾的'\0'去掉
        }

        /// <summary>
        /// 截取字符串
        /// </summary>
        /// <param name="sourceStr">源字符串</param>
        /// <param name="startIndex">开始位置的索引</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns></returns>
        private static string SubString(string sourceStr, int startIndex, int length)
        {
            if (!string.IsNullOrEmpty(sourceStr))
            {
                if (sourceStr.Length >= (startIndex + length))
                    return sourceStr.Substring(startIndex, length);
                else
                    return sourceStr.Substring(startIndex);
            }

            return "";
        }
        #endregion

        #region DEC加密、DEC解密
        /// <summary>
        /// 加密对象秘钥
        /// </summary>
        public const string EncryptionKey = "gr16fr5f";
        /// <summary>
        /// 加密对象偏移量
        /// </summary>
        public const string EncryptionIV = "5tyr45tq";
        /// <summary>
        /// 安全密钥
        /// </summary>
        public const string SecretKey = "lq2021";
        #region DEC加密
        /// <summary>
        ///  DEC 加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt)
        {
            string result = "";
            try
            {
                pToEncrypt = pToEncrypt.Trim();

                pToEncrypt += SecretKey;
                DESCryptoServiceProvider des = new DESCryptoServiceProvider(); //把字符串放到byte数组中   
                byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);
                //byte[] inputByteArray=Encoding.Unicode.GetBytes(pToEncrypt);
                des.Key = ASCIIEncoding.ASCII.GetBytes(EncryptionKey); //建立加密对象的密钥和偏移量   
                des.IV = ASCIIEncoding.ASCII.GetBytes(EncryptionIV); //原文使用ASCIIEncoding.ASCII方法的GetBytes方法
                MemoryStream ms = new MemoryStream(); //使得输入密码必须输入英文文本   
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                StringBuilder ret = new StringBuilder();
                foreach (byte b in ms.ToArray())
                {
                    ret.AppendFormat("{0:x2}", b);
                }
                result = ret.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("SecureHelper", ex, LogPath.Logs);
                result = "";
            }
            return result;
        }
        #endregion

        #region DEC解密
        /// <summary>
        /// DEC 解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt)
        {
            pToDecrypt = pToDecrypt.Trim();
            string result = "";
            try
            {
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
                for (int x = 0; x < pToDecrypt.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(EncryptionKey); //建立加密对象的密钥和偏移量，此值重要，不能修改   
                des.IV = ASCIIEncoding.ASCII.GetBytes(EncryptionIV);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                result = System.Text.Encoding.Default.GetString(ms.ToArray()).Replace(SecretKey, "");
                //if (pToDecrypt != Encrypt(CommonHelper.FilterSql(result))) return "";  //过滤特殊字符
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("SecureHelper", ex, LogPath.Logs);
                result = "";
            }
            return result;
        }
        #endregion
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="inputStr">需要MD5加密的字符串</param>
        /// <returns></returns>
        public static string MD5(string inputStr)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashByte = md5.ComputeHash(Encoding.UTF8.GetBytes(inputStr));
            StringBuilder sb = new StringBuilder();
            foreach (byte item in hashByte)
                sb.Append(item.ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }
        #endregion
    }
}
