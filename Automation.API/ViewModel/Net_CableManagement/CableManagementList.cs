using System.ComponentModel.DataAnnotations;
using System;

namespace Network.API.ViewModel.Net_CableManagement
{
    public class CableManagementList
    {
        public Guid Id { get; set; }

        public string CableCode { get; set; }

        public string CableType { get; set; }

        public string Line { get; set; }

        public string HeadDevice { get; set; }

        public string LastDevice { get; set; }

        public string SetPoint { get; set; }

        public string ManageOrgan { get; set; }

        public string ManagerName { get; set; }

        public string ManagerTel { get; set; }

        public string ManagerEmail { get; set; }
    }
}
