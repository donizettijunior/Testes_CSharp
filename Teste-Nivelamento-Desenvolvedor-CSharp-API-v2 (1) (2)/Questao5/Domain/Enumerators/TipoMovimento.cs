using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Questao5.Domain.Enumerators
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TipoMovimento
    {
        C,
        D
    }

    public class TipoMovimentoConverter : StringEnumConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TipoMovimento);
        }
    }
}
