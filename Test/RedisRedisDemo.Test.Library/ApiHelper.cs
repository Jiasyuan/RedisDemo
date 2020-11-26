using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace RedisRedisDemo.Test.Library
{
    public static class ApiHelper
    {

        public static T Api<T>(string apiServer, EnumContentType contentType, EnumApiMethodType apiMethodType, string methodName, string getParam, object parameter, bool isJson = true)
        {
            // 如果有getParam，確保有加"?"
            if (!string.IsNullOrWhiteSpace(getParam) && !getParam.StartsWith("?"))
            {
                getParam = "?" + getParam.Trim();
            }
            else if (string.IsNullOrWhiteSpace(getParam))
            {
                getParam = default;
            }

            // 整理呼叫的url
            string apiURL = CombinePath(apiServer, methodName) + getParam;

            HttpWebRequest request = HttpWebRequest.Create(apiURL) as HttpWebRequest;
            string PostTypeStr = string.Empty;
            switch (apiMethodType)
            {
                case EnumApiMethodType.Post:
                    PostTypeStr = WebRequestMethods.Http.Post;
                    break;
                case EnumApiMethodType.Get:
                    PostTypeStr = WebRequestMethods.Http.Get;
                    break;
                case EnumApiMethodType.Put:
                    PostTypeStr = WebRequestMethods.Http.Put;
                    break;
                default:
                    break;
            }
            request.Method = PostTypeStr; // 方法
            request.KeepAlive = true; //是否保持連線
            request.ContentType = GetAPIContentType(contentType);
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };//for https
            // 讓這個request最多等10分鐘
            request.Timeout = 600000;
            request.MaximumResponseHeadersLength = int.MaxValue;
            request.MaximumAutomaticRedirections = int.MaxValue;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            try
            {
                // 整理成呼叫的body paramter
                if (apiMethodType != EnumApiMethodType.Get)
                {
                    string JSONParameterString = SerializeToJson<object>(parameter);
                    byte[] bs = System.Text.Encoding.UTF8.GetBytes(JSONParameterString);
                    using (Stream reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                    }
                }
                string jsonResult = default;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var temp = reader.ReadToEnd();
                            jsonResult = temp;
                            //TODO:反序列化
                            if (string.IsNullOrEmpty(temp))
                            {
                                return default(T);
                            }
                            else
                            {
                                T result = default(T);
                                if (isJson)
                                {
                                    result = DeserializeJson<T>(jsonResult);
                                }
                                else
                                {
                                    result = (T)Convert.ChangeType(temp, typeof(T));
                                }
                                return result;
                            }
                        }
                    }
                }
            }
            catch (WebException webException)
            {
                if (webException.Response == null)
                {
                    throw new Exception("服務無回應", webException);
                }
                using (StreamReader reader = new StreamReader(webException.Response.GetResponseStream()))
                {
                    HttpWebResponse res = (HttpWebResponse)webException.Response;
                    var pageContent = reader.ReadToEnd();
                    T result = JsonConvert.DeserializeObject<T>(pageContent);
                    return result;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string CombinePath(string serverBasePath, string callPath)
        {
            if (serverBasePath.Length > 1 && serverBasePath.EndsWith("/")) { serverBasePath = serverBasePath.Substring(0, serverBasePath.Length - 1); }
            if (callPath.Length > 1 && callPath.StartsWith("/")) { callPath = callPath.Substring(1); }
            return serverBasePath + "/" + callPath;
        }

        public static T DeserializeJson<T>(string fromJsonString)
        {
            return JsonConvert.DeserializeObject<T>(fromJsonString);

        }

        public static string SerializeToJson<T>(this T objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        private static string GetAPIContentType(EnumContentType ContentType)
        {
            string result;
            switch (ContentType)
            {
                case EnumContentType.json:
                    result = ConstSetting.ContentTypeJson;
                    break;
                case EnumContentType.formurlencoded:
                    result = ConstSetting.ContentTypeform;
                    break;
                default:
                    throw new Exception("Unknown ContentType.");
            }

            return result;
        }
    }

    internal static class ConstSetting
    {
        public static string ContentTypeJson => "application/json";
        public static string ContentTypeform => "application/x-www-form-urlencoded";
    }
}
