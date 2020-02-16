using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JLC
{
	[CreateAssetMenu(fileName = "Actor Aliases", menuName = "JLC/Actor Aliases", order = 1)]
	public class ActorAliases : ScriptableObject 
	{
		[System.Serializable]
		public class Alias
		{
			public string actorName;
			public string alias;
		}

		public List<Alias> aliases = new List<Alias>();
		public bool caseSensitive = false;

		public string FindActorFromAlias(string alias)
		{
			foreach(Alias a in aliases)
			{
				if(string.Equals(alias, a.alias, (caseSensitive) ? System.StringComparison.Ordinal : System.StringComparison.OrdinalIgnoreCase))
				{
					return a.actorName;
				}
			}

			return alias;
		}


	}
}
