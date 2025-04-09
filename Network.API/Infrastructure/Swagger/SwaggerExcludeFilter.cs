using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Network.API.Infrastructure.Swagger
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        private static readonly List<string> excludes = new List<string>()
    {
       "CreatedDateTime", "UpdatedDateTime",
       "CreatedBy", "UpdatedBy"
    };

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context == null)
                return;

            // Find all properties by name which need to be removed 
            // and not shown on the swagger spec.
            schema.Properties
                  .Where(prp => excludes.Any(exc => string.Equals(exc, prp.Key, StringComparison.OrdinalIgnoreCase)))
                  .Select(prExclude => prExclude.Key)
                  .ToList()
                  .ForEach(key => schema.Properties.Remove(key));
        }
    }
}
