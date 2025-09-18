using DNTCaptcha.Core;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly string[] _fontNames = new string[]
            {
             "Comic Sans MS",
             "Arial",
             "Times New Roman",
             "Georgia",
             "Verdana",
             "Geneva"
            };

        private readonly ICaptchaImageProvider _captchaImageProvider;

        public WeatherForecastController(ICaptchaImageProvider captchaImageProvider)
        {
            _captchaImageProvider = captchaImageProvider;
        }

        //GET: api/public/cms/refresh-captcha
        [HttpGet("refresh-captcha")]
        [ProducesDefaultResponseType]
        public ActionResult<RefreshCaptchaDto> GetRefreshCaptcha()
        {
            string code = GetRandomCode();
            string fontName = GetRandomFontName();

            var captcha = _captchaImageProvider.DrawCaptcha(
                code,
                "#808080",
                "#F5DEB3",
                24,
                fontName);

            return Ok(new RefreshCaptchaDto()
            {
                Ci = Convert.ToBase64String(captcha),
                RefreshTimeSeconds = 30
            });
        }

        private static string GetRandomCode()
        {
            return Random.Shared.Next(100000, 999999).ToString();
        }

        private string GetRandomFontName()
        {
            return _fontNames[Random.Shared.Next(0, _fontNames.Length - 1)];
        }

        public class RefreshCaptchaDto
        {

            [JsonPropertyName("ci")]
            public string Ci { get; set; }

            [JsonPropertyName("refreshTimeSeconds")]
            public int RefreshTimeSeconds { get; set; }
        }
    }
}
