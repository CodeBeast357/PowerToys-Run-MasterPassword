using Microsoft.PowerToys.Settings.UI.Library;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PowerToysRunMasterPassword;

internal sealed class MasterPasswordSettings
{
    private readonly bool _initialized;

    public string Username { get; private set; }

    public AlgorithmVersion AlgorithmVersion { get; private set; }

    public byte PasswordListLength { get; private set; }

    public MasterPasswordSettings()
    {
        UpdateSettings(null);
        _initialized = true;
    }

    internal void UpdateSettings(PowerLauncherPluginSettings settings)
    {
        if ((settings is null || settings.AdditionalOptions is null) & _initialized)
        {
            return;
        }

        Username = GetSettingOrDefault(settings, nameof(Username))?.TextValue;
        AlgorithmVersion = (AlgorithmVersion)GetSettingOrDefault(settings, nameof(AlgorithmVersion))?.ComboBoxValue;
        PasswordListLength = (byte)GetSettingOrDefault(settings, nameof(PasswordListLength))?.NumberValue;
    }

    private static PluginAdditionalOption GetSettingOrDefault(PowerLauncherPluginSettings settings, string name)
    {
        return settings?.AdditionalOptions?.FirstOrDefault(x => x.Key == name)
            ?? GetAdditionalOptions().First(x => x.Key == name);
    }

    internal static IEnumerable<PluginAdditionalOption> GetAdditionalOptions()
    {
        yield return new PluginAdditionalOption
        {
            Key = nameof(Username),
            PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Textbox,
            TextValue = string.Empty,
        };
        yield return new PluginAdditionalOption
        {
            Key = nameof(AlgorithmVersion),
            PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Combobox,
            ComboBoxItems = Enum.GetValues<AlgorithmVersion>()
                .Select(x => KeyValuePair.Create(Enum.GetName(x), x.ToString()))
                .ToList(),
            ComboBoxValue = (int)AlgorithmVersion.V3,
        };
        yield return new PluginAdditionalOption
        {
            Key = nameof(PasswordListLength),
            PluginOptionType = PluginAdditionalOption.AdditionalOptionType.Numberbox,
            NumberBoxMin = 1d,
            NumberBoxMax = byte.MaxValue,
            NumberBoxSmallChange = 1d,
            NumberBoxLargeChange = 3d,
            NumberValue = 5d,
        };
    }
}