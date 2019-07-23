using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Codezi.Domain.Models;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;

namespace Codezi.Application.Commands
{
    public class CompanyInformationCommand : ModuleBase<SocketCommandContext>
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private Company _company;
        public CompanyInformationCommand()
        {
            _httpClient.BaseAddress = new Uri("https://localhost:5001");
            _httpClient.DefaultRequestHeaders.Clear();
        }

        [Command("company"), Summary("Get company information based on a company symbol.")]
        public async Task GetInformationBySymbol([Remainder] string symbol)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"api/stocks/company/{symbol}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await _httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var jsonSerializer = new JsonSerializer();
                        _company = jsonSerializer.Deserialize<Company>(jsonTextReader);
                    }
                }
            }

            var embed = new EmbedBuilder();
            embed.WithAuthor(_company.CompanyName);
            embed.WithColor(Color.DarkRed);
            embed.AddField("Industry", _company.Industry);
            embed.AddField("Exchange", _company.Exchange, true);
            embed.AddField("CEO", _company.Ceo, true);
            embed.WithDescription(_company.Description);
            embed.WithThumbnailUrl(_company.Website);


            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
