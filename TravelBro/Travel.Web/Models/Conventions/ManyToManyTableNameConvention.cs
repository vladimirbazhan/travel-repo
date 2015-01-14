using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Web;

namespace WebApplication1.Models.Conventions
{
    public class ManyToManyTableNameConvention : IStoreModelConvention<EntitySet>
    {
        public void Apply(EntitySet set, DbModel model)
        {
            var properties = set.ElementType.Properties;
            if (properties.Count == 2 &&
                properties.All(x => x.Name.EndsWith("_ID", StringComparison.OrdinalIgnoreCase)))
            {
                set.Table = properties.Aggregate(new StringBuilder(), (sb, next) =>
                {
                    sb = sb.Length == 0 ? sb : sb.Append("To");
                    return sb.Append(next.Name.Substring(0, next.Name.Length - 3)).Append('s');
                }).ToString();
            }
        }
    }
}