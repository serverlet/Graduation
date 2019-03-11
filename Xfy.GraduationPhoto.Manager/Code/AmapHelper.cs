using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Xfy.GraduationPhoto.Manager.Code
{
    public class AmapHelper
    {
        /// <summary>
        /// 高德地图请求类
        /// </summary>
        private readonly HttpClient _amapHttpClient;

        private readonly string ApiVersion;

        public AmapHelper()
        {
            _amapHttpClient = new HttpClient(new MyHttpMessageHandler())
            {
                BaseAddress = new Uri("https://restapi.amap.com"),
            };
            
            //_amapHttpClient.BaseAddress
            ApiVersion = "v3";
        }

        /// <summary>
        /// 逆地址编码
        /// </summary>
        public async Task<AmapReturn> Geocode_Regeo(double lon, double lang)
        {
            string result = await _amapHttpClient.GetStringAsync($"{ApiVersion}/geocode/regeo/?location={lon},{lang}");
            AmapReturn amap = Newtonsoft.Json.JsonConvert.DeserializeObject<AmapReturn>(result);
            return amap;
        }

    }

    public class MyHttpMessageHandler : HttpClientHandler
    {
        private const string KEY = "a2295e7e46a09998a6bc42aa667e5d3f";

        public MyHttpMessageHandler()
        {
            this.UseProxy = false;
            this.UseDefaultCredentials = false;
        }


        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //request.RequestUri.Query += "&key=";
            string pathQuery = string.Format("{0}&key={1}", request.RequestUri.PathAndQuery, KEY);
            request.RequestUri = new Uri(string.Format("{0}{1}", request.RequestUri.GetLeftPart(UriPartial.Authority), pathQuery));
            return base.SendAsync(request, cancellationToken);
        }
        
    }

    public class AmapReturn
    {
        public int Status { get; set; }

        public string Info { get; set; }

        public Regeocode Regeocode { get; set; }
    }

    public class Regeocode
    {
        [Newtonsoft.Json.JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }

        public AddressComponent AddressComponent { get; set; }
    }

    public class AddressComponent
    {
        public string Country { get; set; }

        public string Province { get; set; }

        public string District { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TownShip { get; set; }
    }

}
