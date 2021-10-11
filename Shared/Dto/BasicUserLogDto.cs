using JetBrains.Annotations;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dto
{
	public class BasicUserLogDto
	{
		public Platform LastPlatform { get; set; }

		 public string DeviceModel { get; set; }

		 public string OSVersion { get; set; }

		public string IPAddress { get; set; }
	}
}
