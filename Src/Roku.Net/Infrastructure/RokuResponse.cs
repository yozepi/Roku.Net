using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace yozepi.Roku.Infrastructure
{
    internal class RokuResponse : IRokuResponse
    {
        HttpWebResponse _httpResponse;

        internal RokuResponse(HttpWebResponse httpResponse)
        {
            _httpResponse = httpResponse;
        }
        public HttpStatusCode StatusCode => _httpResponse.StatusCode;
        public string StatusDescription => _httpResponse.StatusDescription;
        public string ContentType => _httpResponse.ContentType;
        public Stream GetResponseStream()
        {
            return _httpResponse.GetResponseStream();
        }

            #region IDisposable Support
        private bool _isDesposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDesposed)
            {
                if (disposing)
                {
                    _httpResponse.Dispose();
                }

                _isDesposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
         }
        #endregion
    }
}
