namespace Agora.Core.Settings;

/// <summary>
/// Represents the configurable settings related to users, typically defined in the application's <c>appsettings.json</c>.
/// </summary>
public class UserSettings
{
    /// <summary>
    /// Gets or sets the amount of initial credit (in Kairos points) assigned to a user upon registration.
    /// </summary>
    public int InitialCredit { get; set; }
}