using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Lib
{
	public class Localizations : IService
	{
		Dictionary<string, string> texts;

		public void Load(string filename)
		{
			texts = new Dictionary<string, string>();
			var textAsset = Resources.Load(filename) as TextAsset;
			using (var reader = new StringReader(textAsset.text)) {
				while(true) {
					var key = reader.ReadLine();
				
					if (key != null) {
						var value = reader.ReadLine();
						texts.Add(key, value);
					} else {
						break;
					}
				}
			}
		}

		public string Get(string key) 
		{
			string value = null;
			texts.TryGetValue(key, out value);

			if (value == null) {
				value = string.Format("Key {0} doesn't exist", key);
			}
			return value;
		}
	}
}

