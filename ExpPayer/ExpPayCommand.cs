using System;
using Rocket.API;
using Rocket.Core;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Extensions;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Linq;
using System.Collections.Generic;

namespace ExpPay
{
	public class ExpPayCommand : IRocketCommand {
		public void Execute(IRocketPlayer caller, string[] command) {
			ExpPayer.Instance.LastSalary = LastSalary;
			foreach (SteamPlayer player in Provider.clients)
			{
				UnturnedPlayer unturnedPlayer = player.ToUnturnedPlayer();
				foreach (Config.SalaryData salaryData in ExpPayer.Instance.Configuration.Instance.SalaryList)
				{
					if (R.Permissions.GetPermissions(unturnedPlayer).Exists(x => x.Name == salaryData.Permission))
					{
						var salaryPermission = R.Permissions.GetPermissions(unturnedPlayer).First(x =>
							ExpPayer.Instance.Configuration.Instance.SalaryList.Exists(s => s.Permission == x.Name));
						if (salaryPermission == null) return;

						var salary =
						ExpPayer.Instance.Configuration.Instance.SalaryList.FirstOrDefault(x => x.Permission == salaryPermission.Name);
						unturnedPlayer.Experience += salaryData.salary;
						UnturnedChat.Say(unturnedPlayer.CSteamID, salaryData.Text + salaryData.salary.ToString() + ExpPayer.Instance.Configuration.Instance.currencySymbol);
					}
				}
			}

		}

		public AllowedCaller AllowedCaller => AllowedCaller.Player;
		public string Name => "exppay";
		public string Help => "Pay exp all players and reset timer.";
		public string Syntax => "/exppay or /epay";
		public List<string> Aliases => new List<string> { "epay" };
		public List<string> Permissions => new List<string> { "exppayer.exppay" };

		public DateTime LastSalary;

	}
}
