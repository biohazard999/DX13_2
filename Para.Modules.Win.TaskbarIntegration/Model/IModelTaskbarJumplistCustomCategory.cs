using DevExpress.ExpressApp.Model;

namespace Para.Modules.Win.TaskbarIntegration.Model
{
    public interface IModelTaskbarJumplistCustomCategory : IModelNode, IModelList<IModelTaskbarJumplistItem>
    {
        string Caption { get; set; }
    }
}