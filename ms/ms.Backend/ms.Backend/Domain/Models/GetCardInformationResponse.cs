namespace ms.Backend.Domain.Models
{
    public class GetCardInformationResponse
    {
        public string isValidcardNumber { get; set; }
        public string profileCode { get; set; }
        public string profile { get; set; }
        public string profile_es { get; set; }
        public string bankCode { get; set; }
        public string bankName { get; set; }       
        public string userName { get; set; }        
        public string userLastName { get; set; }
    }
}
