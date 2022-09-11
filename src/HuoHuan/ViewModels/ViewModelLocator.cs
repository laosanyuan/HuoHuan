using HuoHuan.ViewModels.Pages;
using Ninject;

namespace HuoHuan.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IKernel _ninjectKernel = new StandardKernel();
        public ViewModelLocator()
        {
            this._ninjectKernel.Bind<MainViewModel>().ToSelf().InSingletonScope();

            this._ninjectKernel.Bind<HomePageVM>().ToSelf().InSingletonScope();
            this._ninjectKernel.Bind<ViewPageVM>().ToSelf().InSingletonScope();
            this._ninjectKernel.Bind<ManagePluginVM>().ToSelf().InSingletonScope();
        }

        public MainViewModel Main => _ninjectKernel.Get<MainViewModel>();

        public HomePageVM HomePage => _ninjectKernel.Get<HomePageVM>();

        public ManagePluginVM ManagePlugin => _ninjectKernel.Get<ManagePluginVM>();

        public ViewPageVM ViewPage => _ninjectKernel.Get<ViewPageVM>();
    }
}
