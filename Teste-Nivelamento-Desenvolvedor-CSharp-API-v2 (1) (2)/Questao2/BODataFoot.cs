namespace Questao2
{
    public class BODataFoot
    {
        public int page { get; set; }

        public int per_page { get; set; }

        public int total { get; set; }

        public int total_pages { get; set; }

        public List<BODataFootGames> data { get; set; }
        
    }
}
