using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Dia.Entities;
using Dia.IBLC;
using Dia.BLL;

namespace Dia.WebPL
{
    public static class LogicProvider
    {
        public static IBLContracts diaLogic { get; }
        public static IEnumerable<User> Users { get; }
        public static IEnumerable<Section> Sections { get; }
        
        static LogicProvider()
        {
            diaLogic = new Logic();
            Users = diaLogic.GetUsers();
            Sections = diaLogic.GetSections();
        }
    }
}


