using DevExpress.ExpressApp.Model;

namespace Para.Modules.Win.TaskbarIntegration.Model
{
    public interface IModelTaskbarOptions : IModelNode
    {
        IModelTaskbarJumplistOption TaskbarJumplistOptions { get; }
    }
}