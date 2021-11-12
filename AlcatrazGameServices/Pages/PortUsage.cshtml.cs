using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Alcatraz.GameServices.Pages
{
    public class PortUsage
    {
        public PortUsage()
        {
            Users = new List<string>();
        }

        public int Id { get; set; }
        public int Port { get; set; }
        public List<string> Users { get; set; }
    }

    public class PortUsageModel : PageModel
    {
        public List<PortUsage> UsageList { get; set; }

        public void OnGet()
        {
            UsageList = new List<PortUsage>();

            int id = 1;
            foreach (var pl in QNetZ.NetworkPlayers.Players)
            {
                if (pl.Client == null)
                    continue;

                var foundTuple = UsageList.FindIndex(x => x.Port == pl.Client.Endpoint.Port);

                if(foundTuple != -1)
                {
                    UsageList[foundTuple].Users.Add(pl.Name);
                }
                else
                {
                    UsageList.Add(new PortUsage()
                    {
                        Id = id,
                        Port = pl.Client.Endpoint.Port,
                        Users = new List<string>() { pl.Name }
                    });
                }
			}
        }
    }
}
