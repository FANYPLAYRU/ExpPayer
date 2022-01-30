using System;
using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using Rocket.Unturned.Player;
using System.Collections.Generic;

namespace ExpPay
{
	public class ExpCommand : IRocketCommand
	{
		public void Execute(IRocketPlayer caller, string[] command ) 
		{
			var up = (UnturnedPlayer)caller;
			if (command.Length != 1)
               {
				UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_exp_lengtherror"), UnityEngine.Color.red);
				return;
			}
			if (!int.TryParse(command[0], out var arg1))
			{

				UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_exp_arg1error"), UnityEngine.Color.red);
				return;
			}
			if (arg1 == 0)
			{
				UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_exp_arg1zero"), UnityEngine.Color.red);
				up.Player.skills.ServerSetExperience((uint)arg1);
				return;
			}
			up.Player.skills.ServerSetExperience(up.Player.skills.experience + (uint)arg1);
			UnturnedChat.Say(caller, ExpPayer.Instance.Translate("exppayer_exp_success", arg1, ExpPayer.Instance.Configuration.Instance.currencySymbol), UnityEngine.Color.red, false);
		}

		public AllowedCaller AllowedCaller => AllowedCaller.Player;
		public string Name => "exp";
		public string Help => "Add or Remove experience";
		public string Syntax => "/exp <count>";
		public List<string> Aliases { get { return new List<string>(); } }
		public List<string> Permissions => new List<string> { "exppayer.exp" };

		public DateTime LastSalary;
	}
}
