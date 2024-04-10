namespace ms.Backend.Domain.Models
{
    public class GetCardBalanceResponse
    {
        public string card { get; set; }
        public Int32 balance { get; set; }
        public string balanceDate { get; set; }
        public Int32 virtualBalance { get; set; }
        public string virtualBalanceDate { get; set;}
    }
}
