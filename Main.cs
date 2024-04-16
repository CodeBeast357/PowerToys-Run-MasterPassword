using ManagedCommon;
using Microsoft.PowerToys.Settings.UI.Library;
using PowerToysRunMasterPassword.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Wox.Plugin;

namespace PowerToysRunMasterPassword;

public class Main : IPlugin, IPluginI18n, ISettingProvider
{
    public string Name => GetTranslatedPluginTitle();

    public string Description => GetTranslatedPluginDescription();

    public IEnumerable<PluginAdditionalOption> AdditionalOptions => MasterPasswordSettings.GetAdditionalOptions();

    private string _iconCopy;

    private readonly MasterPasswordSettings _settings = new();

    private PasswordListGenerator _passwordBuilder;

    public void Init(PluginInitContext context)
    {
        ResetPassword();
        context.API.ThemeChanged += OnThemeChanged;
        UpdateIconPath(context.API.GetCurrentTheme());
    }

    public List<Result> Query(Query query)
    {
        if (string.IsNullOrWhiteSpace(_settings.Username))
        {
            ResetPassword();

            return [];
        }

        if (_passwordBuilder == null)
        {
            var userSecret = query.Search.ToCharArray();
            _passwordBuilder = new(_settings.Username, _settings.AlgorithmVersion, ref userSecret);

            return [];
        }

        var siteName = query.Search;
        var resultPasswords = _passwordBuilder.Generate(siteName).Select((item, index) => new Result
        {
            Title = item,
            SubTitle = string.Format(Resources.Counter, index),
            IcoPath = _iconCopy,
            Action = (e) =>
            {
                Clipboard.SetText(item);
                return true;
            },
        });

        return resultPasswords.Take(_settings.PasswordListLength).ToList();
    }

    private void ResetPassword()
    {
        _passwordBuilder = null;
        GC.Collect();
    }

    private void UpdateIconPath(Theme theme)
    {
        _iconCopy = theme == Theme.Light || theme == Theme.HighContrastWhite
            ? "images/copy-light.png"
            : "images/copy-dark.png";
    }

    private void OnThemeChanged(Theme currentTheme, Theme newTheme) => UpdateIconPath(newTheme);

    public string GetTranslatedPluginTitle() => Resources.Name;

    public string GetTranslatedPluginDescription() => Resources.Description;

    public Control CreateSettingPanel() => throw new NotImplementedException();

    public void UpdateSettings(PowerLauncherPluginSettings settings) => _settings.UpdateSettings(settings);
}