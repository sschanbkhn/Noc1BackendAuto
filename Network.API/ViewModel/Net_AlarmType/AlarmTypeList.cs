using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SysAccountVNPT.API.ViewModel.DsAccountDevice
{
    public class AlarmTypeList
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string LevelName { get; set; }

    }
}
