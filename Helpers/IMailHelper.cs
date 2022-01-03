using Aprotur.Common.Models;

namespace AproturWeb.Helpers
{
    public interface IMailHelper
    {
        Response SendMail(string to, string subject, string body, string nameFrom, string nameTo);
    }
}
