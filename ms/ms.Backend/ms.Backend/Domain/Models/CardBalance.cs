namespace ms.Backend.Domain.Models
{
    public class CardBalance
    {
        public string card { get; set; } = null!;
        public Int32 balance { get; set; } 
        public string balanceDate { get; set; } = null!;
        public Int32 virtualBalance { get; set; } 
        public string virtualBalanceDate { get; set;} = null!;
    }
}
