using System;

namespace PowerToysRunMasterPassword;

internal class PasswordListGenerator
{
    private readonly IPasswordAlgorithm _passwordAlgorithm;

    private readonly byte[] _userKey;

    private readonly byte[] _userSecret;

    private PasswordListGenerator(IPasswordAlgorithm passwordAlgorithm, byte[] userKey, byte[] userSecret)
    {

        _userKey = userKey;
        _passwordAlgorithm = passwordAlgorithm;
        _userSecret = userSecret;
    }

    ~PasswordListGenerator()
    {
        Array.Fill(_userSecret, byte.MinValue);
    }

    public static PasswordListGenerator Create(AlgorithmVersion algorithmVersion, string userName, ref char[] userPassword)
    {
        var passwordAlgorithm = PasswordAlgorithmFactory.Provide(algorithmVersion);
        var userSecret = System.Text.Encoding.UTF8.GetBytes(userPassword);
        Array.Fill(userPassword, char.MinValue);
        var userKey = passwordAlgorithm.CreateUserKey(userName, userSecret);
        return new PasswordListGenerator(passwordAlgorithm, userKey, userSecret);
    }

    public System.Collections.Generic.IEnumerable<string> GenerateList(string siteName)
    {
        for (var siteCounter = (byte)1; siteCounter < byte.MaxValue; siteCounter++)
        {
            yield return _passwordAlgorithm.GeneratePassword(_userSecret, siteName, siteCounter);
        }
    }
}
