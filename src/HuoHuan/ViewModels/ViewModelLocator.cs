using DryIoc;
using HuoHuan.ViewModels.Pages;

namespace HuoHuan.ViewModels
{
    public class ViewModelLocator
    {
        private readonly Container _container;
        public ViewModelLocator()
        {
            this._container = new Container();

            this._container.Register<MainViewModel>();
            this._container.Register<HomePageVM>();
            this._container.Register<ViewPageVM>();
        }

        public MainViewModel Main => _container.Resolve<MainViewModel>();

        public HomePageVM HomePage => _container.Resolve<HomePageVM>();

        public ViewPageVM ViewPage => _container.Resolve<ViewPageVM>();
    }
}
