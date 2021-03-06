using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using Caliburn.Micro;
using NavigationListControl;

namespace Inferis.TwunchApp {
    public class AppBootstrapper : PhoneBootstrapper {
        PhoneContainer container;

        protected override void Configure()
        {
            container = new PhoneContainer(this);

            container.RegisterPhoneServices();
            container.RegisterAllViewModelsForPages();

            //container.InstallChooser<PhoneNumberChooserTask, PhoneNumberResult>();
            //container.InstallLauncher<EmailComposeTask>();

            AddCustomConventions();
        }

        protected override void OnLaunch(object sender, Microsoft.Phone.Shell.LaunchingEventArgs e)
        {
            base.OnLaunch(sender, e);

        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        static void AddCustomConventions()
        {
            var ec = ConventionManager.AddElementConvention<NavigationList>(NavigationList.ItemsSourceProperty, "DataContext", "Loaded");
            ec.ApplyBinding = (viewModelType, path, property, element, convention) => {
                if (!ConventionManager.SetBinding(viewModelType, path, property, element, convention))
                    return false;

                var nl = (NavigationList)element;
                nl.ItemTemplate = ConventionManager.DefaultItemTemplate;
                var method = viewModelType.GetMethod("Navigate" + element.Name);
                if (method != null) {
                    nl.Navigation += (s, e) => { method.Invoke(element.DataContext, new[] { e.Item }); };
                }


                return true;
            };

            ConventionManager.AddElementConvention<Pivot>(Pivot.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) => {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention)) {
                        ConventionManager
                            .ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, viewModelType);
                        return true;
                    }

                    return false;
                };

            ConventionManager.AddElementConvention<Panorama>(Panorama.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) => {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention)) {
                        ConventionManager
                            .ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, viewModelType);
                        return true;
                    }

                    return false;
                };
        }
    }
}
