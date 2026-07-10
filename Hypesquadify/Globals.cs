using System;
using System.Collections.Generic;
using System.Text;

namespace Hypesquadify
{
    /// <summary>
    /// Represents the global variables.
    /// </summary>
    public static class Globals
    {
        /// <summary>
        /// Gets or sets the app's version.
        /// </summary>
        public static Version AppVersion { get; }
            = new Version(2, 0, 0);

        /// <summary>
        /// Gets or sets the HTTP client.
        /// </summary>
        public static HttpClient Http { get; set; }
    }
}
