﻿using Abp.Application.Navigation;
using Abp.Configuration;
using Abp.Localization;
using Abp.Threading;

namespace Cms.Api
{
    public class CmsNavigationProvider : NavigationProvider
    {

        private readonly ISettingManager _settingManager;

        public CmsNavigationProvider(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    CreateNavigationWithSettings(NavigationNames.Cms)
                    .AddItem(CreateNavigationWithSettings(NavigationNames.CmsManage))
                );
        }

        private MenuItemDefinition CreateNavigationWithSettings(string name)
        {
            return new MenuItemDefinition(
                name,
                L(name),
                url: Setting(NavigationNames.GetName(name, NavigationNames.Propertys.Url)),
                icon: Setting(NavigationNames.GetName(name, NavigationNames.Propertys.Icon)),
                isVisible: Setting<bool>(NavigationNames.GetName(name, NavigationNames.Propertys.IsVisible)),
                isEnabled: Setting<bool>(NavigationNames.GetName(name, NavigationNames.Propertys.IsEnabled)),
                requiredPermissionName: Setting(NavigationNames.GetName(name, NavigationNames.Propertys.RequiredPermissionName)),
                order: Setting<int>(NavigationNames.GetName(name, NavigationNames.Propertys.Order))
                );
        }

        private T Setting<T>(string name) where T : struct
        {
            return AsyncHelper.RunSync<T>(() => _settingManager.GetSettingValueAsync<T>(name));
        }

        private string Setting(string name)
        {
            return AsyncHelper.RunSync(() => _settingManager.GetSettingValueAsync(name));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, CmsConsts.LocalizationSourceName);
        }
    }
}
