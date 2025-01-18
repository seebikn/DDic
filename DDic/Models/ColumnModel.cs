namespace DDic.Models
{
    internal class ColumnModel
    {
        public required string ParentProjectName { get; set; }
        public required string ParentTableID { get; set; }
        public int ColumnNo { get; set; }
        public string? LogicalName { get; set; }
        public required string PhysicalName { get; set; }
        public string? PrimaryKey { get; set; }
        public string? Nullable { get; set; }
        public string? DataType { get; set; }
        public string? DataSize { get; set; }
        public string? Remarks { get; set; }
    }
}
