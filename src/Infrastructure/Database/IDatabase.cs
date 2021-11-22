using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GroceryCo.Infrastructure.Database
{
    public interface IDatabase<T>
    {
        void LoadDBTable(List<DataRow> rows);
        List<DataRow> GetDBTableRows();
        DataRow GetNewRow();
    }
}
