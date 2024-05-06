namespace PowerToysRunMasterPassword;

internal interface IPasswordAlgorithm
{
    byte[] CreateUserKey(string userName, byte[] userKey);

    string GeneratePassword(byte[] userSecret, string siteName, byte siteCounter);
}
