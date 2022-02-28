using RestSharp;

namespace NovaCore.Web.Extensions
{
    public static class RestRequestExtensions
    {
        public static void AddParameters(this RestRequest request, params WebParameter[] parameters)
        {
            foreach (WebParameter parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }
        }
    }
}