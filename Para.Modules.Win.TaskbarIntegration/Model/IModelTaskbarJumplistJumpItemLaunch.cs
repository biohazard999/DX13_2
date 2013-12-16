namespace Para.Modules.Win.TaskbarIntegration.Model
{
    public interface IModelTaskbarJumplistJumpItemLaunch : IModelTaskbarJumplistJumpItemBase
    {
        string PathToLaunch { get; set; }
        string Arguments { get; set; }

        string WorkingDirectory { get; set; }
    }
}