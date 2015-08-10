using Lambda3.WorkItemFieldHistory.Models;
using System.Collections.Generic;
using System.Linq;
using TFS = Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Lambda3.WorkItemFieldHistory.Extensions
{
    public static class MapperExtensions
    {
        public static WorkItem MapToModel(this TFS.WorkItem workItem)
        {
            return new WorkItem
            {
                Id = workItem.Id,
                Title = workItem.Title,
                TypeName = workItem.Type.Name,
                Fields = workItem.Fields.MapToModel(),
                Revisions = workItem.Revisions.MapToModel()
            };
        }

        private static IEnumerable<Revision> MapToModel(this TFS.RevisionCollection revisions)
        {
            return revisions
                .Cast<TFS.Revision>()
                .Select(revision => new Revision
                {
                    Index = revision.Index,
                    Fields = revision.Fields
                        .MapToModel()
                        .ToDictionary(field => field.Name, field => field)
                });
        }

        private static IEnumerable<Field> MapToModel(this TFS.FieldCollection fields)
        {
            return fields
                .Cast<TFS.Field>()
                .Select(field => new Field
                {
                    Name = field.Name,
                    ReferenceName = field.FieldDefinition.ReferenceName,
                    Value = field.Value,
                    Type = field.FieldDefinition.FieldType,
                    OriginalValue = field.OriginalValue,
                    IsChangedInRevision = field.IsChangedInRevision
                });
        }
    }
}
