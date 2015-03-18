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
                // CommentsTo and PhotosTo should go the first place in the table name
                List<string> tmpProps = new List<string>(properties.Select(x => x.Name));
                

                bool moved = MovePropertyToFirstPlace("comment_id", tmpProps);
                if (!moved)
                {
                    MovePropertyToFirstPlace("photo_id", tmpProps);
                }

                set.Table = tmpProps.Aggregate(new StringBuilder(), (sb, next) =>
                {
                    sb = sb.Length == 0 ? sb : sb.Append("To");
                    return sb.Append(next.Substring(0, next.Length - 3)).Append('s');
                }).ToString();
            }
        }

        private bool MovePropertyToFirstPlace(string propId, List<string> props)
        {
            int reqPropIndex =
                props.FindIndex(
                    x => (new[] { propId }).Contains(x, StringComparer.OrdinalIgnoreCase));
            if (reqPropIndex == 1)
            {
                props.Reverse();
            }
            return reqPropIndex != -1;
        }
    }
}