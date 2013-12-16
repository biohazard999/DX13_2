using System.ComponentModel;
using DevExpress.ExpressApp.Model;

namespace Para.Modules.Win.TaskbarIntegration.Model
{
    public interface IModelCustomProtocolOption : IModelNode
    {
        bool EnableProtocols { get; set; }

        [DefaultValue(true)]
        bool AutoRegisterProtocols { get; set; }
        string ProtocolName { get; set; }

        string ProtocolDescription { get; set; }
    }
}