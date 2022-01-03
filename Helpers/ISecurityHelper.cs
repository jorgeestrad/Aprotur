namespace GeoPlus.Helpers
{
    public interface ISecurityHelper
    {
        string GetHashSha256(byte[] data);
    }
}
