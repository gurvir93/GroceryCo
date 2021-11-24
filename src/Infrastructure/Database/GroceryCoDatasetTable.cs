using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GroceryCo.Infrastructure.Database
{
    public class GroceryCoDatasetTable<T> : IGroceryCoTable<T>
        where T : DataTable
    {
        private T table;

        public GroceryCoDatasetTable(T table)
        {
            this.table = table;
        }

        public void LoadDBTable(List<DataRow> rows)
        {
            table.Rows.Add(rows);
        }

        public List<DataRow> GetDBTableRows()
        {
            return table.Select().ToList();
        }

        public DataRow GetNewRow()
        {
            return table.NewRow();
        }
    }
}
