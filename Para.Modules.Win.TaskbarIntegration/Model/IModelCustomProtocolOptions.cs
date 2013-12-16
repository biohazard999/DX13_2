using DevExpress.ExpressApp.Model;

namespace Para.Modules.Win.TaskbarIntegration.Model
{
    public interface IModelCustomProtocolOptions : IModelNode
    {
        IModelCustomProtocolOption CustomProtocolOptions { get; }
    }
}