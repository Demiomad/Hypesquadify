using Hypesquadify.Enums;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Hypesquadify.Discord
{
    /// <summary>
    /// Utilities for setting a Hypesquad badge.
    /// </summary>
    public static class HypesquadBadge
    {
        private const string Endpoint = "https://discord.com/api/v9/hypesquad/online";

        /// <summary>
        /// Sets the Hypesquad badge to the one specified by <paramref name="house"/>
        /// </summary>
        /// <param name="token">The user's Discord token.</param>
        /// <param name="house">The target Hypesquad house.</param>
        /// <returns>The HTTP response.</returns>
        public static async Task<HttpResponseMessage> SetAsync(string token, HypesquadHouse house)
        {
            var data = new
            {
                house_id = (int)house
            };

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(Endpoint),
                Method = HttpMethod.Post,
                Content = JsonContent.Create(data)
            };

            request.Headers.UserAgent.Clear();
            request.Headers.UserAgent.ParseAdd($"Hypesquadify/{Globals.AppVersion}");
            request.Headers.Add("Authorization", token);

            return await Globals.Http.SendAsync(request);
        }
    }
}
