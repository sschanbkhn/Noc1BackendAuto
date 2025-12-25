using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Network.Core.Models;

namespace Network.API.Model
{
    [Table("r001_scheduler_fix_parametter")]
    public class R001_SchedulerFixParameter : AuditEntity
    {
        // Id, CreatedDateTime, CreatedBy, UpdatedDateTime, UpdatedBy are inherited from AuditEntity
        // Note: Need to configure column names in DbContext for AuditEntity properties
        
        [Column("ne_name")]
        public string NeName { get; set; }
        
        [Column("cell_id")]
        public int CellId { get; set; }
        
        [Column("utran_ps_ho_switch")]
        public string UtranPsHoSwitch { get; set; }
        
        [Column("report_date")]
        public DateTime? ReportDate { get; set; }
        
        // CreatedDateTime, CreatedBy, UpdatedDateTime, UpdatedBy are inherited from AuditEntity
        
        [Column("utran_srvcc_switch")]
        public string UtranSrvccSwitch { get; set; }
        
        [Column("utran_csfb_switch")]
        public string UtranCsfbSwitch { get; set; }
        
        [Column("utran_flash_csfb_switch")]
        public string UtranFlashCsfbSwitch { get; set; }
        
        [Column("geran_flash_csfb_switch")]
        public string GeranFlashCsfbSwitch { get; set; }
        
        [Column("csfb_adaptive_blind_ho_switch")]
        public string CsfbAdaptiveBlindHoSwitch { get; set; }
        
        [Column("utran_csfb_steering_switch")]
        public string UtranCsfbSteeringSwitch { get; set; }
        
        [Column("idle_csfb_redirect_opt_switch")]
        public string IdleCsfbRedirectOptSwitch { get; set; }
        
        [Column("dl_voip_bundling_switch")]
        public string DlVoipBundlingSwitch { get; set; }
        
        [Column("ul_voip_pre_allocation_switch")]
        public string UlVoipPreAllocationSwitch { get; set; }
        
        [Column("ul_voip_delay_sch_switch")]
        public string UlVoipDelaySchSwitch { get; set; }
        
        [Column("ul_voip_load_based_sch_switch")]
        public string UlVoipLoadBasedSchSwitch { get; set; }
        
        [Column("ul_voip_serv_state_enhanced_sw")]
        public string UlVoipServStateEnhancedSw { get; set; }
        
        [Column("ul_voip_sch_opt_switch")]
        public string UlVoipSchOptSwitch { get; set; }
        
        [Column("ul_vo_lte_data_size_est_switch")]
        public string UlVoLteDataSizeEstSwitch { get; set; }
        
        [Column("updated_edit")]
        public DateTime? UpdatedEdit { get; set; }
        
        [Column("cmd_at")]
        public string CmdAt { get; set; }
        
        [Column("cmd_reg_at")]
        public string CmdRegAt { get; set; }
        
        [Column("baseline_type")]
        public string BaselineType { get; set; }
        
        [Column("status")]
        public int? Status { get; set; }
    }
}

