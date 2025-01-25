using Xunit;
using DDic.Models;

namespace DDic.Tests
{
    public class ColumnModelTests
    {
        [Fact]
        public void ColumnModel_PropertyAssignments_ShouldBeSetCorrectly()
        {
            // Arrange
            var columnModel = new ColumnModel
            {
                ParentProjectName = "TestProject",
                ParentTableID = "Table123",
                ColumnNo = 1,
                LogicalName = "Test Logical Name",
                PhysicalName = "TestPhysicalName",
                PrimaryKey = "Yes",
                Nullable = "No",
                DataType = "int",
                DataSize = "4",
                Remarks = "Test remarks"
            };

            // Act & Assert
            Assert.Equal("TestProject", columnModel.ParentProjectName);
            Assert.Equal("Table123", columnModel.ParentTableID);
            Assert.Equal(1, columnModel.ColumnNo);
            Assert.Equal("Test Logical Name", columnModel.LogicalName);
            Assert.Equal("TestPhysicalName", columnModel.PhysicalName);
            Assert.Equal("Yes", columnModel.PrimaryKey);
            Assert.Equal("No", columnModel.Nullable);
            Assert.Equal("int", columnModel.DataType);
            Assert.Equal("4", columnModel.DataSize);
            Assert.Equal("Test remarks", columnModel.Remarks);
        }

        [Fact]
        public void ColumnModel_DefaultValues_ShouldBeNullOrDefault()
        {
            // Arrange
            var columnModel = new ColumnModel
            {
                ParentProjectName = "TestProject",
                ParentTableID = "Table123",
                PhysicalName = "TestPhysicalName"
            };

            // Act & Assert
            Assert.Null(columnModel.LogicalName);
            Assert.Null(columnModel.PrimaryKey);
            Assert.Null(columnModel.Nullable);
            Assert.Null(columnModel.DataType);
            Assert.Null(columnModel.DataSize);
            Assert.Null(columnModel.Remarks);
            Assert.Equal(0, columnModel.ColumnNo); // int のデフォルト値は 0
        }
    }
}
