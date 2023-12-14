using ErgoLAB.Expert.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kingfar.BioFeedback.WPFUI.Views.Pages.Scheme
{
    //接收ExpertClient返回
    public class TestRpcInteractionMaster : IRpcInteractionMaster
    {
        private readonly IRpcInteractionSlave _slave;

        public string? EmbedSchemeRunPath { get; set; }

        public TestRpcInteractionMaster(IRpcInteractionSlave slave)
        {
            //用于给clien发消息
            _slave = slave;
        }

        /// <summary>
        /// 接受ExpertClient返回状态，确认是否已经正常加载
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task NotifyAsync(NotifyOptions options)
        {
            switch (options.Type)
            {
                case NotifyType.SlaveReady://加载完成
                    //此处可以设置服务端执行医疗接口用于的
                    break;

                case NotifyType.SlaveExit://退出
                    break;

                default:
                    break;
            }
            return Task.CompletedTask;
        }

        public Task ReportEmbedSchemeRunAsync(EmbedSchemeRunReportOptions options)
        {
            //可以了解运行状态
            throw new NotImplementedException();
        }
    }
}