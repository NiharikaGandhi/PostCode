namespace PostCodeAPI.Message
{
    public class PostCodeGetResponse
    {
        public int Id { get; set; }
        public int PostCode { get; set; }
        public string Suburb { get; set; }
    }
}