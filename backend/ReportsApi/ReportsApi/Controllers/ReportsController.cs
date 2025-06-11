using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportWebApi.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ReportWebApi.Controllers;

/// <summary>
/// Контроллер для работы с отчетами
/// </summary>
[ApiController]
[Route("[controller]")]
public class ReportsController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "ProtheticOnly")]
    [Produces("application/json")]
    public Task<ReportModel[]> Get()
    {
        var reports = new[]
        {
            new ReportModel(DateTime.Today-TimeSpan.FromDays(2), "Отчет 1", "100%", "OK"),
            new ReportModel(DateTime.Today-TimeSpan.FromDays(1), "Отчет 2", "90%", "OK"),
            new ReportModel(DateTime.Today, "Отчет 3", "20%", "Low battery"),
        };
        return Task.FromResult(reports);
    }

}