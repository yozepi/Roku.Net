using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace yozepi.Roku
{
    /// <summary>
    /// Use an instance of this class to control a particular remote. 
    /// </summary>
    /// <remarks>
    /// Use the RokuDiscovery class to find instances of rokus on your local network.
    /// The RokuDiscovery.DiscoverAsync method will return a list of IRokuRemote instances for each 
    /// Roku found on your local network.
    /// </remarks>
    public interface IRokuRemote
    {
        /// <summary>
        /// A list of all the applications currently installed on this Roku.
        /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-query/appsExample
        /// </summary>
        IList<RokuApp> Apps { get;}

        /// <summary>
        /// Contains detailed information about the Roku device and it's capabilities.
        /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-query/device-infoExample
        /// </summary>
        RokuInfo Info { get;}

        /// <summary>
        /// The Url of the Roku device.
        /// </summary>
        Uri Url { get; }

        /// <summary>
        /// Returns information about the currently active application for this Roku.
        /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-query/active-appExamples
        /// </summary>
        /// <returns>Returns an ActiveAppInfo instance. IsSuccess will be true if the request was successful.</returns>
        Task<ActiveAppInfo> GetActiveAppAsync();

        /// <summary>
        /// Returns the application Icon for the provided Roku application Id.
        /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-query/iconExample
        /// </summary>
        /// <param name="appId"></param>
        /// <returns>Returns an AppIcon instance. IsSuccess will be true if the request was successful.</returns>
        Task<AppIcon> GetAppIconAsync(int appId);

        /// <summary>
        /// Sends a key-down message to the Roku device.
        /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-query/iconExample
        /// </summary>
        /// <param name="key" cref="CommandKeys">A command key to send to the Roku.</param>
        /// <returns>Returns an ICommandResponse instance. IsSuccess will be true if the request was successful.</returns>
        /// <remarks>
        /// Similar to pressing and holding the equivalent key on a Roku remote.
        /// The Roku will continue to behave as though the key has been pressed until the KeyUp command is received.
        /// </remarks>
        Task<ICommandResponse> KeyDownAsync(CommandKeys key);

        /// <summary>
        /// Sends a key-down message to the Roku device.
        /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-query/iconExample
        /// </summary>
        /// <param name="key" cref="Char">The character to send to the Roku</param>
        /// <returns>Returns an ICommandResponse instance. IsSuccess will be true if the request was successful.</returns>
        /// <remarks>
        /// Similar to pressing and holding the equivalent key on a Roku remote.
        /// The Roku will continue to behave as though the key has been pressed until the KeyUp command is received.
        /// </remarks>
        Task<ICommandResponse> KeyDownAsync(char key);

        /// <summary>
        /// Sends a key-up message to the Roku device.
        /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-keyup/key
        /// </summary>
        /// <returns>Returns an ICommandResponse instance. IsSuccess will be true if the request was successful.</returns>
        /// <exception cref="InvalidOperationException">No previous key-down command was sent.</exception>
        Task<ICommandResponse> KeyUpAsync();

        /// <summary>
        /// Sends a key-press message to the Roku device.
        /// </summary>
        /// <param name="key" cref="CommandKeys">A command key to send to the Roku.</param>
        /// <returns>Returns an ICommandResponse instance. IsSuccess will be true if the request was successful.</returns>
        /// <remarks>
        /// Similar to pressing and releasing the equivalent key on a Roku remote.
        /// </remarks>
        Task<ICommandResponse> KeypressAsync(CommandKeys key);

        /// <summary>
        /// Sends a key-press message to the Roku device.
        /// </summary>
        /// <param name="key" cref="Char">The character to send to the Roku</param>
        /// <returns>Returns an ICommandResponse instance. IsSuccess will be true if the request was successful.</returns>
        /// <remarks>
        /// Similar to pressing and releasing the equivalent key on a Roku remote.
        /// </remarks>
        Task<ICommandResponse> KeypressAsync(char key);

        /// <summary>
        /// Launches an application on the Roku device.
        /// See the Roku API documentation at 
        /// </summary>
        /// <param name="appId">The Roku application Id</param>
        /// <returns>Returns an ICommandResponse instance. IsSuccess will be true if the request was successful.</returns>
        Task<ICommandResponse> LaunchAppAsync(int appId);

        /// <summary>
        /// Launches a search on the Roku device.
        /// See the Roku API documentation at https://sdkdocs.roku.com/display/sdkdoc/External+Control+API#ExternalControlAPI-searchExamples
        /// </summary>
        /// <param name="keyword">(required) The keword or phrase to search for.</param>
        /// <param name="type" cref="SearchType?">(optional) the type of program/content to search for. If omitted the all content is searched.</param>
        /// <param name="season">(optional) Search for a particular season (ignored if type is anything other than TVShow).</param>
        /// <param name="appId">(optional) If provided, then this application will have priority in the search.</param>
        /// <param name="launch">(optional) If provided, launches the application upon a successful search.</param>
        /// <returns>Returns an ICommandResponse instance. IsSuccess will be true if the request was successful.</returns>
        Task<ICommandResponse> SearchAsync(string keyword, SearchType? type = null, int? season = null, int? appId = null, bool launch = false);
    }
}