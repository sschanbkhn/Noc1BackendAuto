namespace Network.API.ViewModel.Net_NetworkLinks
{
    public class ConnectionsResult
    {
        public string source {  get; set; }
        public string target { get; set; }
        public string sourcePort { get; set; }
        public string targetPort { get; set; }
        public int distance { get; set; }
        public string type { get; set; }
    }
}
