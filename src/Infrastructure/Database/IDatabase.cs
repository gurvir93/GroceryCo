using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GroceryCo.Infrastructure
{
    public interface IDatabase
    {
        public void LoadDBTable(List<DataRow> rows);
        public List<DataRow> GetDBTableRows();
    }
}
