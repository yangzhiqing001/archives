using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace archives.common
{

    public class OssHelper
    {
        public static AliyunCommonResponse SendSms(string templateCode, string phone, string jsonParams)
        {
            ApplicationLog.Info($"start SendSms:{templateCode},json:{jsonParams}");
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", "LTAIs7TC8gUyoffn", "A6OKeTXS9Dwk64Zm6xEgMrWzFBO20S");
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest
            {
                Method = MethodType.POST,
                Domain = "dysmsapi.aliyuncs.com",
                Version = "2017-05-25",
                Action = "SendSms"
            };
            request.AddQueryParameters("PhoneNumbers", phone);
            request.AddQueryParameters("SignName", "建设公司综合管理部档案室");
            request.AddQueryParameters("TemplateCode", templateCode);
            request.AddQueryParameters("TemplateParam", jsonParams);
            try
            {
                CommonResponse response = client.GetCommonResponse(request);
                ApplicationLog.Info($"end SendSms:{templateCode},json:{jsonParams}; response:{response.Data}");
                return response.Data.Deserialize<AliyunCommonResponse>();
            }
            //catch (ServerException e)
            //{
            //    Console.WriteLine(e);
            //}
            //catch (ClientException e)
            //{
            //    Console.WriteLine(e);
            //}
            catch(Exception ex)
            {
                ApplicationLog.Error("end SendSms Exception", ex);
            }
            return null;
        }
    }

    public class AliyunCommonResponse
    {
        public string Message { get; set; }

        public string RequestId { get; set; }

        public string Code { get; set; }
    }
}
