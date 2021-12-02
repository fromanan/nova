namespace NovaCore.Web
{
    public struct WebQuery
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public WebQuery(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}