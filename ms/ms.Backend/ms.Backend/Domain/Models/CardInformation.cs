namespace ms.Backend.Domain.Models
{
    public class CardInformation
    {
        public string isValidcardNumber { get; set; } = null!;
        public string profileCode { get; set; } = null!;
        public string profile { get; set; } = null!;
        public string profile_es { get; set; } = null!;
        public string bankCode { get; set; } = null!;
        public string bankName { get; set; } = null!;
        public string userName { get; set; } = null!;
        public string userLastName { get; set; } = null!;
    }
}