using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GroceryCo.Domain.Interfaces
{
    public interface IDatabase
    {
        public void LoadDBTable(List<DataRow> rows);
        public List<DataRow> GetDBTableRows();
    }
}
