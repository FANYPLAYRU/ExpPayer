using System;
using System.Collections.Generic;
using Rocket.API;

namespace ExpPay {
	// Token: 0x02000003 RID: 3
	public class Config : IRocketPluginConfiguration, IDefaultable {
		// Token: 0x06000005 RID: 5 RVA: 0x000021D8 File Offset: 0x000003D8
		public void LoadDefaults() {
			TimeBetweenSalaries = 900f;
			SalaryList = new List<Config.SalaryData>();
			currencySymbol = "F";
			PluginLoad = true;
			SalaryList.Add(new Config.SalaryData {
				salary = 100U,
				Permission = "exppay.Police",
				Text = "Вы получили зарплату Полицейского в размере: "
			});
			SalaryList.Add(new Config.SalaryData {
				salary = 200U,
				Permission = "exppay.Medic",
				Text = "Вы получили зарплату Медика в размере: "
			});
		}

		// Token: 0x04000004 RID: 4
		public List<Config.SalaryData> SalaryList;

		// Token: 0x04000005 RID: 5
		public float TimeBetweenSalaries;

		public string currencySymbol;
		public bool PluginLoad;

		// Token: 0x02000004 RID: 4
		public class SalaryData
		{
			// Token: 0x04000006 RID: 6
			public string Permission;
			public string Text;
			// Token: 0x04000007 RID: 7
			public uint salary;
		}
	}
}
