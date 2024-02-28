namespace StudentAPI.Model
{
    public class DbConfiguration
    {
        public int MaxConnectionLimit { get; init; }

        public int RequestTimeout { get; init; }

        public string Endpoint { get; init; }

        public string PrimaryKeySecret { get; init; }

        public string DatabaseId { get; init; }
    }
}
