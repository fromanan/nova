namespace NovaCore.Web.Server.Interfaces;

public interface ICookiesStorage : IHttpHeaders
{
    void Upsert(string key, string value);

    void Remove(string key);

    bool Touched { get; }

    string ToCookieData();
}