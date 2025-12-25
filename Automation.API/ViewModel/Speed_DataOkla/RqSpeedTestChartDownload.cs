using System.Collections.Generic;

namespace Network.API.ViewModel.Speed_DataOkla
{
    public class RqAutomationChartDownload
    {
        public List<string> listNhaMang { get; set; }
        public List<string> listKhuVuc { get; set; }
        public string type { get; set; }
        public int month { get; set; }
        public int quarter { get; set; }
        public int year { get; set; }
    }
}
