using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WM_Utility.HttpClients
{
    /// <summary>
    /// 适应不同编码
    /// </summary>
    public interface ISampleClient
    {
        Task<string> GetDataAsync(string uri, string charset = "UTF-8");
    }
}
