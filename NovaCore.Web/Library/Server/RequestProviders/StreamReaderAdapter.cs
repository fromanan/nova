using System.IO;
using System.Threading.Tasks;
using NovaCore.Common.Extensions;
using NovaCore.Web.Server.Interfaces;

namespace NovaCore.Web.Server.RequestProviders;

internal record StreamReaderAdapter(StreamReader Reader) : IStreamReader
{
    public async Task<string> ReadLine()
    {
        return await Reader.ReadLineAsync().ContextIndependent();
    }
    
    public async Task<byte[]> ReadBytes(int count)
    {
        char[] tempBuffer = new char[count];

        await Reader.ReadBlockAsync(tempBuffer, 0, count).ContextIndependent();

        byte[] retVal = new byte[count];

        for (int i = 0; i < tempBuffer.Length; i++)
            retVal[i] = (byte)tempBuffer[i];

        return retVal;
    }
}