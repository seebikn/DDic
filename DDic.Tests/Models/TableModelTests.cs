using Xunit;
using DDic.Models;

namespace DDic.Tests.Models
{
    public class TableModelTests
    {
        [Fact]
        public void TableModel_ShouldInitializeWithRequiredProperties()
        {
            // Arrange
            var projectName = "SampleProject";
            var tableId = "Table123";

            // Act
            var tableModel = new TableModel
            {
                ProjectName = projectName,
                TableID = tableId
            };

            // Assert
            Assert.Equal(projectName, tableModel.ProjectName);
            Assert.Equal(tableId, tableModel.TableID);
            Assert.Null(tableModel.TableName);
            Assert.Null(tableModel.Description);
        }

        [Fact]
        public void TableModel_ShouldAllowSettingOptionalProperties()
        {
            // Arrange
            var projectName = "SampleProject";
            var tableId = "Table123";
            var tableName = "SampleTable";
            var description = "This is a sample table.";

            // Act
            var tableModel = new TableModel
            {
                ProjectName = projectName,
                TableID = tableId,
                TableName = tableName,
                Description = description
            };

            // Assert
            Assert.Equal(projectName, tableModel.ProjectName);
            Assert.Equal(tableId, tableModel.TableID);
            Assert.Equal(tableName, tableModel.TableName);
            Assert.Equal(description, tableModel.Description);
        }
    }
}
