using DevExpress.ExpressApp.DC;
using Para.Modules.Win.TaskbarIntegration.Model;

namespace Para.Modules.Win.TaskbarIntegration
{
    [DomainLogic(typeof(IModelCustomProtocolOption))]
    public static class ModelCustomProtocolOption_Logic
    {
        public static string Get_ProtocolDescription(IModelCustomProtocolOption option)
        {
            if (option == null || string.IsNullOrEmpty(option.ProtocolName))
                return "Protocol Handler";

            return option.ProtocolName + " Protocol";
        }
    }
}