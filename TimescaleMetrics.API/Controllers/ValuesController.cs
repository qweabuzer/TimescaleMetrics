using Microsoft.AspNetCore.Mvc;
using TimescaleMetrics.Core.Interfaces;
using TimescaleMetrics.Core.Models;

namespace TimescaleMetrics.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IValueService _valueService;
        private readonly IResultService _resultService;
        private ILogger<ValuesController> _logger;

        public ValuesController(IValueService valueService, IResultService resultService, ILogger<ValuesController> logger)
        {
            _valueService = valueService;
            _resultService = resultService;
            _logger = logger;

        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("файл не может быть пустым");

            if (!Path.GetExtension(file.FileName).Equals(".csv", StringComparison.OrdinalIgnoreCase))
                return BadRequest("можно загрузить только файлы типа .csv");

            if (file.Length > 10 * 1024 * 1024)
                return BadRequest("файл не может весить больше 10МБ");

            try
            {
                using var stream = file.OpenReadStream();
                var result = await _valueService.ParseFileAsync(file.FileName, stream);

                if (result.IsFailure)
                    return BadRequest(result.Error);

                return Ok(result.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ошибка при чтении файла {FileName}", file?.FileName);
                return StatusCode(500, "возникла ошибка при чтении файла");
            }
        }

        [HttpGet("results/filter")]
        public async Task<IActionResult> GetFilteredResults([FromQuery] ResultsFilter filter)
        {
            var results = await _resultService.GetFilterResultsAsync(filter);

            if(results.IsFailure)
                return BadRequest(results.Error);

            return Ok(results.Value);
        }

        [HttpGet("values/last/{fileName}")]
        public async Task<IActionResult> GetLastValues(string fileName, [FromQuery] int count = 10)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("имя файла обязательно");

            var values = await _valueService.GetLastValuesAsync(fileName, count);

            if (values.IsFailure)
                return BadRequest(values.Error);

            return Ok(values.Value);
        }
    }
}
