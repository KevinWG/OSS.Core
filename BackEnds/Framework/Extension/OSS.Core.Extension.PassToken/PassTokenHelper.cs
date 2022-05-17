using OSS.Common.Encrypt;
using OSS.Common.Helpers;
using OSS.Core.Context;

namespace OSS.Core.Extension;
public static class PassTokenHelper
    {
        /// <summary>
        ///  生成通行令牌（PassToken）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GenerateToken(string data)
        {
            return GenerateToken(data, string.Empty);
        }

        /// <summary>
        ///  校验通行令牌
        /// </summary>
        /// <param name="data"></param>
        /// <param name="passToken"></param>
        /// <returns></returns>
        public static bool CheckToken(string data, string passToken)
        {
            if (passToken.Length < 4)
                return false;
            
            return passToken == GenerateToken(data, passToken.Substring(passToken.Length - 4));
        }

        private static string GenerateToken(string data, string randomSeed)
        {
            if (string.IsNullOrEmpty(randomSeed))
            {
                randomSeed = NumHelper.RandomNum();
            }

            var token = Md5.EncryptHexString(string.Concat(data, CoreAppContext.Identity.token, randomSeed));
            return string.Concat(token, randomSeed);
        }

    }
