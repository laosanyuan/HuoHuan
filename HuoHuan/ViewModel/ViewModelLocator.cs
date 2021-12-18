using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using HuoHuan.ViewModel.Pages;

namespace HuoHuan.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();

            //Page
            SimpleIoc.Default.Register<MainPageVM>();
            SimpleIoc.Default.Register<ViewPageVM>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        /// <summary>
        /// Ö÷Ò³Ãæ
        /// </summary>
        public MainPageVM MainPage => ServiceLocator.Current.GetInstance<MainPageVM>();

        /// <summary>
        /// ä¯ÀÀÒ³Ãæ
        /// </summary>
        public ViewPageVM ViewPage => ServiceLocator.Current.GetInstance<ViewPageVM>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}