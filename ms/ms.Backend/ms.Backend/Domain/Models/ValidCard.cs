namespace ms.Backend.Domain.Models
{
    public class ValidCard
    {
        public string card { get; set; }
        public bool isValid { get; set; }
        public string status { get; set; }
        public Int32 statusCode { get; set; }
    }
}
