using Newtonsoft.Json;

namespace StudentAPI.Model
{
    /// <summary>
    /// Model class for Student data stored in db.
    /// </summary>
    public class Student
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string PartitionKey => Standard;

        public string Name { get; set; }
        
        public string Standard {  get; set; }

        public string Father { get; set; }

        public string Mother {  get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
