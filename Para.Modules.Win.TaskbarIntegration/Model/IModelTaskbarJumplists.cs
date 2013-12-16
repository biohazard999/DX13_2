using DevExpress.ExpressApp.Model;

namespace Para.Modules.Win.TaskbarIntegration.Model
{
    public interface IModelTaskbarJumplists : IModelNode
    {
        IModelTaskbarJumplistTaskCategory TasksCategory { get; }

        IModelTaskbarJumplistCustomCategories CustomCategories { get; }
    }
}