using DryIoc;
using HuoHuan.ViewModels.Pages;

namespace HuoHuan.ViewModels
{
    public class ViewModelLocator
    {
        Container _container;
        public ViewModelLocator()
        {
            this._container = new Container();

            this._container.Register<MainViewModel>();
            this._container.Register<MainPageVM>();
            this._container.Register<ViewPageVM>();
        }

        public MainViewModel Main => _container.Resolve<MainViewModel>();

        public MainPageVM MainPage => _container.Resolve<MainPageVM>();

        public ViewPageVM ViewPage => _container.Resolve<ViewPageVM>();
    }
}
