using System.ComponentModel;
using DevExpress.ExpressApp.Model;

namespace Para.Modules.Win.TaskbarIntegration.Model
{
    public interface IModelTaskbarJumplistOption : IModelNode
    {
        bool EnableJumpList { get; set; }

        IModelTaskbarJumplists Jumplists { get; }

        [DefaultValue("JumplistImages.dll")]
        string AutomaticImageAssemblyName { get; set; }
    }
}