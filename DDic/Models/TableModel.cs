using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDic.Models
{
    internal class TableModel
    {
        public required string ProjectName { get; set; }
        public required string TableID { get; set; }
        public string? TableName { get; set; }
        public string? Description { get; set; }
    }
}
