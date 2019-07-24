using System.Net.Http;
using System.Threading.Tasks;
using Discord.Commands;

namespace Codezi.Application.Commands
{
    public class BuyInfoCommand : ModuleBase<SocketCommandContext>
    {
        private readonly HttpClient _httpClient = new HttpClient();

        [Command("intraday"), Summary("Gets current intraday")]
        public async Task GetBuyOrLeaveInfo([Remainder] string symbol)
        {

        }

        // Trading algorithm: 
        //# This is a simple trading algorithm that buy low sell high

        //# Algorithm:
        //# 1.    Use 27 stock context
        //# 2.    For each stock in these stocks
        //# 3.    Buy in long when current price lower than 45 day price average, and when I have over 2000 capital left, and when I have not hold any position for that one particular stock

        //# 4.    buy (2000 / current stock price = # of shares of that stock)
        //# 5.    Hold until price has come up
        //# 6.    Sell Once the price is higher and total earning is larger than $100
    }
}
