﻿using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyCoreLib.WeixinSDK.Entiyies.JsonResult;
using MyCoreLib.WeixinSDK.Enums;
using MyCoreLib.WeixinSDK.Helper.Conventers;

namespace MyCoreLib.WeixinSDK.Helper.Http
{
    public static class CommonJsonSend
    {
        #region 同步请求

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="sendType"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static WxJsonResult Send(string accessToken, string urlFormat, object data, RequestMethod sendType = RequestMethod.POST, int timeOut = GlobalContext.TIME_OUT, bool checkValidationResult = false)
        {
            return Send<WxJsonResult>(accessToken, urlFormat, data, sendType, timeOut);
        }

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat">用accessToken参数填充{0}</param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="sendType"></param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="checkValidationResult"></param>
        /// <returns></returns>
        public static T Send<T>(string accessToken, string urlFormat, object data, RequestMethod sendType = RequestMethod.POST, int timeOut = GlobalContext.TIME_OUT, bool checkValidationResult = false, JsonSetting jsonSetting = null)
        {
            try
            {
                var url = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken.AsUrlData());
                switch (sendType)
                {
                    case RequestMethod.GET:
                        return WebClientHelper.GetJson<T>(url);
                    case RequestMethod.POST:
                        SerializerHelper serializerHelper = new SerializerHelper();
                        var jsonString = serializerHelper.GetJsonString(data, jsonSetting);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            var bytes = Encoding.UTF8.GetBytes(jsonString);
                            ms.Write(bytes, 0, bytes.Length);
                            ms.Seek(0, SeekOrigin.Begin);

                            return WebClientHelper.PostGetJson<T>(url, null, ms, timeOut: timeOut, checkValidationResult: checkValidationResult);
                        }

                    //TODO:对于特定的错误类型自动进行一次重试，如40001（目前的问题是同样40001会出现在不同的情况下面）
                    default:
                        throw new ArgumentOutOfRangeException("sendType");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region 异步请求

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <returns></returns>
        public static async Task<WxJsonResult> SendAsync(string accessToken, string urlFormat, object data, RequestMethod sendType = RequestMethod.POST, int timeOut = GlobalContext.TIME_OUT, JsonSetting jsonSetting = null)
        {
            return await SendAsync<WxJsonResult>(accessToken, urlFormat, data, sendType, timeOut);
        }

        /// <summary>
        /// 向需要AccessToken的API发送消息的公共方法
        /// </summary>
        /// <param name="accessToken">这里的AccessToken是通用接口的AccessToken，非OAuth的。如果不需要，可以为null，此时urlFormat不要提供{0}参数</param>
        /// <param name="urlFormat"></param>
        /// <param name="data">如果是Get方式，可以为null。在POST方式中将被转为JSON字符串提交</param>
        /// <param name="sendType">发送类型，POST或GET，默认为POST</param>
        /// <param name="timeOut">代理请求超时时间（毫秒）</param>
        /// <param name="checkValidationResult">验证服务器证书回调自动验证</param>
        /// <param name="jsonSetting">JSON字符串生成设置</param>
        /// <returns></returns>
        public static async Task<T> SendAsync<T>(string accessToken, string urlFormat, object data, RequestMethod sendType = RequestMethod.POST, int timeOut = GlobalContext.TIME_OUT, bool checkValidationResult = false,
            JsonSetting jsonSetting = null
            )
        {
            try
            {
                var url = string.IsNullOrEmpty(accessToken) ? urlFormat : string.Format(urlFormat, accessToken.AsUrlData());

                switch (sendType)
                {
                    case RequestMethod.GET:
                        return await WebClientHelper.GetJsonAsync<T>(url);
                    case RequestMethod.POST:
                        SerializerHelper serializerHelper = new SerializerHelper();
                        var jsonString = serializerHelper.GetJsonString(data, jsonSetting);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            var bytes = Encoding.UTF8.GetBytes(jsonString);
                            await ms.WriteAsync(bytes, 0, bytes.Length);
                            ms.Seek(0, SeekOrigin.Begin);

                            return
                                await
                                    WebClientHelper.PostGetJsonAsync<T>(url, null, ms, timeOut: timeOut,
                                        checkValidationResult: checkValidationResult);
                        }
                    default:
                        throw new ArgumentOutOfRangeException("sendType");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion
    }
}
