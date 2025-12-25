using System;
using System.Collections.Generic;

namespace Network.API.ViewModel.Dashboard
{
    public class R001DashboardResponse
    {
        public DateTime Date { get; set; }
        public R001Statistics Statistics { get; set; }
        public List<R001ParameterSummary> ParameterSummaries { get; set; }
        public List<R001ConfiguredSite> ConfiguredSites { get; set; }
    }

    public class R001Statistics
    {
        public int TotalConfiguredSites { get; set; }
        public int CorrectConfigurations { get; set; }
        public int IncorrectConfigurations { get; set; }
        public decimal CorrectPercentage { get; set; }
    }

    public class R001ParameterSummary
    {
        public string ParameterName { get; set; }
        public int CorrectCount { get; set; }
        public int IncorrectCount { get; set; }
        public int TotalCount { get; set; }
        public decimal CorrectPercentage { get; set; }
    }

    public class R001ConfiguredSite
    {
        public string NeName { get; set; }
        public int CellId { get; set; }
        public DateTime? ReportDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsCorrect { get; set; }
        public List<R001ParameterDetail> Parameters { get; set; }
    }

    public class R001ParameterDetail
    {
        public string ParameterName { get; set; }
        public string ActualValue { get; set; }
        public string ExpectedValue { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class R001DetailRequest
    {
        public DateTime? Date { get; set; }
        public string ParameterName { get; set; }
        public bool? IsCorrect { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class R001DetailResponse
    {
        public List<R001DetailItem> Data { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
    }

    public class R001DetailItem
    {
        public string NeName { get; set; }
        public int CellId { get; set; }
        public List<R001ParameterDetail> Parameters { get; set; }
        public DateTime? ReportDate { get; set; }
        public bool IsCorrect { get; set; }
    }
}