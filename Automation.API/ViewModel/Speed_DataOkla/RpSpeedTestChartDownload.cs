using System.Collections.Generic;

namespace Network.API.ViewModel.Speed_DataOkla
{
    public class RpAutomationChartDownload
    {
        public List<string> labels { get; set; }
        public List<string> datas_labels { get; set; }
        public List<List<double>> datas { get; set; }
    }
}
