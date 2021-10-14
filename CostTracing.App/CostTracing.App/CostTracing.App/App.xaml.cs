using CostTracing.App.Services;
using CostTracing.App.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CostTracing.App
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
