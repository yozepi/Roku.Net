using Rssdp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using yozepi.Roku.Infrastructure;

namespace yozepi.Roku
{
    public class RokuRemote : IRokuRemote
    {
        public Uri Url { get; internal set; }
        public RokuInfo Info { get; internal set; }
        public IList<RokuApp> Apps { get; internal set; }

        private IRokuRequestFactory _requestFactory;


        internal RokuRemote(IRokuRequestFactory requestFactory, Uri uri, RokuInfo info, IList<RokuApp> apps)
        {
            _requestFactory = requestFactory;
            Url = uri;
            Info = info;
            Apps = apps;
        }

        #region methods:


        public async Task<ICommandResponse> LaunchAppAsync(int appId)
        {
            var request = _requestFactory.Create();
            using (var response = await request.GetResponseAsync(UrlUtils.LaunchAppUrlFor(this.Url, appId), "POST"))
            {
                return new CommandResponse(response);
            }
        }

        public async Task<ActiveAppInfo> GetActiveAppAsync()
        {
            var request = _requestFactory.Create();

            using (var response = await request.GetResponseAsync(UrlUtils.ActiveAppUrlFor(this.Url), "GET"))
            {
                ActiveAppInfo result = null;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result = new ActiveAppInfo
                    {
                        IsSuccess = false,
                        StatusCode = response.StatusCode,
                        StatusDescription = response.StatusDescription
                    };
                }
                else
                {
                    result = ConversionUtils.XmlToInstance<ActiveAppInfo>(response.GetResponseStream());
                    result.IsSuccess = true;
                    result.StatusCode = response.StatusCode;
                    result.StatusDescription = response.StatusDescription;
                }
                return result;
            }
        }

        public async Task<ICommandResponse> SearchAsync(string keyword, SearchType? type = null, int? season = null, int? appId = null, bool launch = false)
        {

            if (!this.Info.IsSearchEnabled)
                throw new InvalidOperationException("Search is not supported for this device.");

            SearchUrlBuilder builder = new SearchUrlBuilder
            {
                Keyword = keyword,
                SearchType = type,
                Season = season,
                AppId = appId,
                Launch = launch
            };
            var url = builder.BuildSearchUrlFor(this.Url);

            var request = _requestFactory.Create();
            using (var response = await request.GetResponseAsync(url, "POST"))
            {
                return new CommandResponse(response);
            }
        }


        public Task<ICommandResponse> KeypressAsync(CommandKeys key)
        {
            return SendKeyPressAsync(key.ToRouteValue());
        }

        public Task<ICommandResponse> KeypressAsync(char key)
        {
            return SendKeyPressAsync(key.ToRouteValue());
        }

        private async Task<ICommandResponse> SendKeyPressAsync(string key)
        {
            var request = _requestFactory.Create();
            using (var response = await request.GetResponseAsync(UrlUtils.KeyPressUrlFor(this.Url, key), "POST"))
            {
                return new CommandResponse(response);
            }
        }



        public Task<ICommandResponse> KeyDownAsync(CommandKeys key)
        {
            return SendKeyDownAsync(key.ToRouteValue());
        }

        public Task<ICommandResponse> KeyDownAsync(char key)
        {
            return SendKeyDownAsync(key.ToRouteValue());
        }

        internal string _lastKeyPressed;
        private async Task<ICommandResponse> SendKeyDownAsync(string key)
        {
            var request = _requestFactory.Create();
            using (var response = await request.GetResponseAsync(UrlUtils.KeyDownUrlFor(this.Url, key), "POST"))
            {
                var result = new CommandResponse(response);
                if (result.IsSuccess)
                {
                    _lastKeyPressed = key;
                }
                return result;
            }
        }

        public async Task<ICommandResponse> KeyUpAsync()
        {
            if (_lastKeyPressed == null)
                throw new InvalidOperationException("There was no initiating keydown operation.");

            var request = _requestFactory.Create();
            using (var response = await request.GetResponseAsync(UrlUtils.KeyUpUrlFor(this.Url, _lastKeyPressed), "POST"))
            {
                var result = new CommandResponse(response);
                if (result.IsSuccess)
                {
                    _lastKeyPressed = null;
                }
                return result;
            }
        }

        public async Task<AppIcon> GetAppIconAsync(int appId)
        {

            var uri = $"{this.Url}query/icon/{appId}";

            var request = _requestFactory.Create();
            using (var response = await request.GetResponseAsync(UrlUtils.AppIconUrlFor(this.Url, appId), "GET"))
            {
                AppIcon result = null;
                if (response.StatusCode != HttpStatusCode.OK)
                {
                    result = new AppIcon(response);
                }
                else
                {
                    MemoryStream localStream = new MemoryStream();
                    await response.GetResponseStream().CopyToAsync(localStream);
                    result = new AppIcon(appId, response.ContentType, localStream.ToArray(), response);
                }

                return result;
            }
        }

        #endregion //Methods:
    }
}
