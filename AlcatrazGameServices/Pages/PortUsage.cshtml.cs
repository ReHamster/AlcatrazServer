using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alcatraz.GameServices.Pages
{
    public class PortUsageModel : PageModel
    {
        public List<Tuple<int, int, int>> Ports { get; set; }

        public void OnGet()
        {
            Ports = new List<Tuple<int, int, int>>();

            int id = 1;
            foreach (var pl in QNetZ.NetworkPlayers.Players)
            {
                if (pl.Client == null)
                    continue;

                var foundTuple = Ports.FindIndex(x => x.Item2 == pl.Client.Endpoint.Port);

                if(foundTuple != -1)
                {
                    var tup = new Tuple<int, int, int>(Ports[foundTuple].Item1, Ports[foundTuple].Item2, Ports[foundTuple].Item3+1);
                    Ports[foundTuple] = tup;
                }
                else
                    Ports.Add(new Tuple<int, int, int>(id++, pl.Client.Endpoint.Port, 1));
			}
        }
    }
}
