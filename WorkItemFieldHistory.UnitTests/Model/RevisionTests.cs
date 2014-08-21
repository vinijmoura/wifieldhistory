using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using Lambda3.WorkItemFieldHistory.Models;

namespace WorkItemFieldHistory.UnitTests
{
    [TestClass]
    public class RevisionTests
    {
        private Revision revision;

        [TestInitialize]
        public void Setup()
        {
            revision = new Revision
            {
                Index = 3,
                Fields = new Dictionary<string, Field>
                {
                    { "Field1", new Field { Name = "Field1"} },
                    { "Field2", new Field { Name = "Field2", IsChangedInRevision = true, OriginalValue = "AAA", Value = null} },
                    { "Field3", new Field { Name = "Field3", IsChangedInRevision = true, OriginalValue = null, Value = "AAA"} },
                    { "Changed By", new Field { Name = "Changed By"} },
                    { "Changed Date", new Field { Name = "Changed Date" } },
                }
            };
        }

        [TestMethod]
        public void ShouldGetFieldAtRevision()
        {
            var field = revision.GetField("Field1");

            field.FieldName.Should().Be("Field1");
        }

        [TestMethod]
        public void OldValueShouldBeEmptyAtRevisionWhenFieldOriginalValueIsNull()
        {
            var field = revision.GetField("Field3");

            field.OldValue.Should().Be(string.Empty);
        }

        [TestMethod]
        public void NewValueShouldBeEmptyAtRevisionWhenFieldValueIsNull()
        {
            var field = revision.GetField("Field2");

            field.NewValue.Should().Be(string.Empty);
        }

        [TestMethod]
        public void ShouldCalculateRevisionNumber()
        {
            var field = revision.GetField("Field1");

            field.RevisionNumber.Should().Be(4);
        }
    }
}
