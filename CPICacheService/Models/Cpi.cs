namespace CPICacheService.Models
{
    public class Cpi
    {
        public string SeriesId { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public int value { get; set; }
        public List<string> Notes { get; set; } = new List<string>();
    }
}
