using System.Reflection;
using System.Runtime.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Questao5.Domain.Enumerators
{
    public class EnumDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var _listEnums = new Dictionary<string, Type>
            {
                { "TipoMovimento", typeof(TipoMovimento) }
            };

            foreach (var schema in swaggerDoc.Components.Schemas)
            {
                if (_listEnums.TryGetValue(schema.Key, out Type enumType))
                {
                    var enumNames = GetEnumNames(enumType);
                    schema.Value.Enum = enumNames;
                }
            }
        }

        private IList<IOpenApiAny> GetEnumNames(Type enumType)
        {
            var enumNames = new List<IOpenApiAny>();
            foreach (var enumValue in Enum.GetValues(enumType))
            {
                var fieldInfo = enumType.GetField(enumValue.ToString());
                var enumMemberAttribute = fieldInfo.GetCustomAttribute<EnumMemberAttribute>();
                var name = enumMemberAttribute != null ? enumMemberAttribute.Value : enumValue.ToString();
                enumNames.Add(new OpenApiString(name));
            }
            return enumNames;
        }
    }
}
