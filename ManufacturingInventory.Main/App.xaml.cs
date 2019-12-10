﻿using Prism.Ioc;
using DryIoc;
using Prism.DryIoc;
using Prism.Regions;
using System.Windows;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Prism;
using ManufacturingInventory.Common.Buisness.Interfaces;
using ManufacturingInventory.Common.Buisness.Concrete;
using ManufacturingInventory.Common.Model;
using ManufacturingInventory.ManufacturingApplication.ViewModels;
using ManufacturingInventory.ManufacturingApplication.Views;


namespace ManufacturingInventory.ManufacturingApplication {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App {

        private IUserService userService = new UserService();

        protected override Window CreateShell() {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {
            if (this.userService.IsValid()) {
                var container = containerRegistry.GetContainer();
                container.Register<ManufacturingContext>(setup: Setup.With(allowDisposableTransient: true));

                containerRegistry.Register<ILogInService, LogInService>();
                containerRegistry.Register<IDomainManager, DomainManager>();
                containerRegistry.RegisterInstance<IUserService>(this.userService);
            } else {
                this.Shutdown();
            }

        }

        protected override void OnStartup(StartupEventArgs e) {
            DXSplashScreen.Show<ManufacturingInventory.ManufacturingApplication.SETSplashScreen>();
            ApplicationThemeHelper.UpdateApplicationThemeName();
            ThemeManager.ApplicationThemeChanged += this.ThemeManager_ApplicationThemeChanged;
            GridControl.AllowInfiniteGridSize = true;


            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            if (this.ShowLogin()) {
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            } else {
                Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
                this.Shutdown();
            }
            base.OnStartup(e);
        }

        private bool ShowLogin() {
            //Startup Login
            LoginWindow loginWindow = new LoginWindow();
            DomainManager domainManager = new DomainManager();
            UserServiceProvider userServiceProvider = new UserServiceProvider(new ManufacturingContext(), domainManager);
            LogInService logInService = new LogInService(domainManager, userServiceProvider);
            var loginVM = new LoginViewModel(logInService);
            loginVM.LoginCompleted += (sender, args) => {
                if (loginVM.LoginResponce.Success) {
                    this.userService = loginVM.LoginResponce.Service;
                    DXSplashScreen.Show<SETSplashScreen>();
                }
                loginWindow.Close();

            };
            loginWindow.DataContext = loginVM;
            if (DXSplashScreen.IsActive)
                DXSplashScreen.Close();

            loginWindow.ShowDialog();
            return this.userService.IsValid();
        }

        private bool CheckVersion() {
            CheckVersionWindow versionWindow = new CheckVersionWindow();
            var versionVM = new CheckVersionViewModel();
            versionVM.CheckCompleted += (sender, args) => {
                versionWindow.Close();
            };
            versionWindow.DataContext = versionVM;
            versionWindow.ShowDialog();
            return true;
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings) {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            var factory = base.Container.Resolve<IRegionBehaviorFactory>();
            regionAdapterMappings.RegisterMapping(typeof(DocumentGroup), AdapterFactory.Make<RegionAdapterBase<DocumentGroup>>(factory));
            regionAdapterMappings.RegisterMapping(typeof(LayoutPanel), AdapterFactory.Make<RegionAdapterBase<LayoutPanel>>(factory));
            regionAdapterMappings.RegisterMapping(typeof(LayoutGroup), AdapterFactory.Make<RegionAdapterBase<LayoutGroup>>(factory));
            regionAdapterMappings.RegisterMapping(typeof(TabbedGroup), AdapterFactory.Make<RegionAdapterBase<TabbedGroup>>(factory));
        }

        private void ThemeManager_ApplicationThemeChanged(DependencyObject sender, ThemeChangedRoutedEventArgs e) {
            ApplicationThemeHelper.SaveApplicationThemeName();
        }
    }
}
