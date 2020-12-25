using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dia.IDAC;
using Dia.DAL;

namespace Dia.BLL
{
    internal static class DataAccessProvider
    {
        internal static IDAContracts DBAccessor { get; }
        static DataAccessProvider()
        {
            DBAccessor = new DBAccessor();
        }
    }
}
