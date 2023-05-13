using System.Threading.Tasks;

namespace NovaCore.Web.Server.Interfaces;

public interface IStreamReader
{
    Task<string> ReadLine();

    Task<byte[]> ReadBytes(int count);
}