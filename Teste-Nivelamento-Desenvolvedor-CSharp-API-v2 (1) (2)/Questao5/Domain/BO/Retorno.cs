namespace Questao5.Domain.BO
{
    public class Retorno
    {
        public string type { get; set; }

        public string title { get; set; }

        public int status { get; set; }

        public string traceId { get; set; }

        public object errors { get; set; }

        public object Data { get; set; }
    }
}
