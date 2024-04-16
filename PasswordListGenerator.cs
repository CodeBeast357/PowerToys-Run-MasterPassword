using System;
using System.Collections.Generic;

namespace PowerToysRunMasterPassword
{
    internal class PasswordListGenerator
    {
        private readonly string _userName;

        private readonly AlgorithmVersion _algorithmVersion;

        private readonly byte[] _userKey;

        public PasswordListGenerator(string userName, AlgorithmVersion algorithmVersion, ref char[] userSecret)
        {

            _userName = userName;
            _algorithmVersion = algorithmVersion;
            _userKey = CreateUserKey(_algorithmVersion, ref userSecret);
        }

        ~PasswordListGenerator()
        {
            Array.Fill(_userKey, byte.MinValue);
        }

        static byte[] CreateUserKey(AlgorithmVersion algorithmVersion, ref char[] userSecret)
        {
            Array.Fill(userSecret, char.MinValue);
            return new[] { byte.MinValue };
        }

        public IEnumerable<string> Generate(string siteName)
        {
            yield break;
        }
    }
}
