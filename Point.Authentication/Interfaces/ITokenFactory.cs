namespace Point.Authentication.Interfaces;

public interface ITokenFactory
{
    byte[] Bytes(IIdentity identity);
}