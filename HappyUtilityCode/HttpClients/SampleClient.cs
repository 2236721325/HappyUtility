using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text;

namespace WM_Utility.HttpClients
{
    /// <summary>
    /// 适应不同编码
    /// </summary>
    public class SampleClient : ISampleClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

      

        public SampleClient(ILogger<SampleClient> logger,
            HttpClient httpClient)
        {
            // 先添加这一个，才能使用 UTF 以外的其他编码 
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            httpClient.DefaultRequestHeaders.Add("Accept",
                "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

            httpClient.DefaultRequestHeaders.Add("User-Agent",
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:73.0) Gecko/20100101 Firefox/73.0");

            _httpClient = httpClient;
            _logger = logger;
        }
        // charset 默认为 UTF-8, 根据需要可以修改为 GBK,GB2312等 
        public async Task<string> GetDataAsync(string uri, string charset = "UTF-8")
        {
            _logger.LogInformation("SampleClient {0} at {1}", "Started", DateTime.UtcNow);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                var response = await _httpClient.GetAsync(uri).ConfigureAwait(false);
                // 读取字符流
                var result = await response.Content.ReadAsStreamAsync();
                // 使用指定的字符编码读取字符流， 默认编码：UTF-8，其他如：GBK
                var stream = new StreamReader(result, Encoding.GetEncoding(charset));
                // 字符流转为字符串并返回
                return stream.ReadToEnd();
            }
            catch (HttpRequestException hre)
            {
                hre.ToString();
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                sw.Stop();
                _logger.LogInformation("response.Content.ReadAsStreamAsync time cost: {0} ", sw.ElapsedMilliseconds.ToString());
            }
        }
    }
}
