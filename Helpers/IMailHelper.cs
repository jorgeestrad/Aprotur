using GeoPlus.Common.Models;

namespace GeoPlus.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body, string nameFrom, string nameTo);
    }
}
