using System;
using System.Collections.Generic;

namespace Network.API.ViewModel
{
    /// <summary>
    /// Request model for fixing a single bad configuration
    /// </summary>
    public class R001FixConfigurationRequest
    {
        public string NeName { get; set; }
        public int CellId { get; set; }
        public DateTime? DetectedDate { get; set; }
        
        // All parameter values from bad configuration
        public string UtranPsHoSwitch { get; set; }
        public string UtranSrvccSwitch { get; set; }
        public string UtranCsfbSwitch { get; set; }
        public string UtranFlashCsfbSwitch { get; set; }
        public string GeranFlashCsfbSwitch { get; set; }
        public string CsfbAdaptiveBlindHoSwitch { get; set; }
        public string UtranCsfbSteeringSwitch { get; set; }
        public string IdleCsfbRedirectOptSwitch { get; set; }
        public string DlVoipBundlingSwitch { get; set; }
        public string UlVoipPreAllocationSwitch { get; set; }
        public string UlVoipDelaySchSwitch { get; set; }
        public string UlVoipLoadBasedSchSwitch { get; set; }
        public string UlVoipServStateEnhancedSw { get; set; }
        public string UlVoipSchOptSwitch { get; set; }
        public string UlVoLteDataSizeEstSwitch { get; set; }
        public DateTime? ReportDate { get; set; }
    }

    /// <summary>
    /// Response model for fix configuration result
    /// </summary>
    public class R001FixConfigurationResponse
    {
        public int Id { get; set; }
        public string NeName { get; set; }
        public int CellId { get; set; }
        public string BaselineType { get; set; }
        public string Command { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}

