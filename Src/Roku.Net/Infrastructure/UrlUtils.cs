using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace yozepi.Roku.Infrastructure
{
    internal class UrlUtils
    {

        public static string DeviceInfoUrlFor(Uri rokuUri)
        {
            return $"{rokuUri}query/device-info";
        }

        public static string AppsUrlFor(Uri rokuUri)
        {
            return $"{rokuUri}query/apps";
        }

        public static string ActiveAppUrlFor(Uri rokuUri)
        {
            return $"{rokuUri}query/active-app";
        }

        public static string AppIconUrlFor(Uri rokuUri, int appId)
        {
            return $"{rokuUri}query/icon/{appId}";
        }

        public static string LaunchAppUrlFor(Uri rokuUri, int appId)
        {
            return $"{rokuUri}launch/{appId}";
        }

        public static string KeyDownUrlFor(Uri rokuUri, string key)
        {
            return $"{rokuUri}keydown/{key}";
        }

        public static string KeyUpUrlFor(Uri rokuUri, string lastKeyPressed)
        {
            return $"{rokuUri}keyup/{lastKeyPressed}";
        }

        public static string KeyPressUrlFor(Uri rokuUri, string key)
        {
            return $"{rokuUri}keypress/{key}";
        }

    }
}
