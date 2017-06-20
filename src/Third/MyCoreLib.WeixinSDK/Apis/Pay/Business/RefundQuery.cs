﻿using MyCoreLib.WeixinSDK.Entiyies;

namespace MyCoreLib.WeixinSDK.Apis.Pay.Business
{
    public class RefundQuery
    {
        /// <summary>
        /// 退款查询完整业务流程逻辑
        /// </summary>
        /// <param name="refund_id">微信退款单号（优先使用）</param>
        /// <param name="out_refund_no">商户退款单号</param>
        /// <param name="transaction_id">微信订单号</param>
        /// <param name="out_trade_no">商户订单号</param>
        /// <returns>退款查询结果（xml格式）</returns>
        public static string Run(string refund_id, string out_refund_no, string transaction_id, string out_trade_no)
        {
            WxPayData data = new WxPayData();
            if(!string.IsNullOrEmpty(refund_id))
                data.SetValue("refund_id", refund_id);//微信退款单号，优先级最高
            else if(!string.IsNullOrEmpty(out_refund_no))
                data.SetValue("out_refund_no", out_refund_no);//商户退款单号，优先级第二
            else if(!string.IsNullOrEmpty(transaction_id))
                data.SetValue("transaction_id", transaction_id);//微信订单号，优先级第三
            else
                data.SetValue("out_trade_no", out_trade_no);//商户订单号，优先级最低            

            WxPayData result = WxPayAPI2.RefundQuery(data);//提交退款查询给API，接收返回数据
            return result.ToPrintStr();
        }
    }
}