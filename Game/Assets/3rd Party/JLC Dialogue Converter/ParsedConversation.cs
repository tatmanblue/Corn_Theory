using System.Collections.Generic;
using PixelCrushers.DialogueSystem;

namespace JLC
{
	public class ParsedConversation
	{
		public int id;
		public string title = "";
		public string actor = "";
		public string conversant = "";
		public string description = string.Empty;
		public List<ParsedDialogueEntry> nodes = new List<ParsedDialogueEntry>();
		public List<Link> links = new List<Link>();

		public int NodeCount {get { return nodes.Count; } }
		public bool IsSelected {get; set;} //Used by converter window

		public ParsedConversation(int id, string name)
		{
			this.id = id;
			this.title = name;
		}

		public void AddLink(int parentNode, int childNode)
		{
			bool alreadyAdded = false;
			foreach(Link l in links)
			{
				if(l.originDialogueID == parentNode && l.destinationDialogueID == childNode)
				{
					alreadyAdded = true;
					break;
				}
			}

			if(!alreadyAdded)
			{
				links.Add(new Link(id, parentNode, id, childNode));
			}

		}

		/// <summary>
		/// Used when importing. Make a guess at the Actor and Conversant names. We can override this later in the converter window.
		/// </summary>
		public void AutoFillActorNames()
		{
			if(nodes.Count <= 0) return;

			actor = nodes[0].actorName;
			conversant = actor;

			foreach(ParsedDialogueEntry de in nodes)
			{
				if(de.actorName != actor)
				{
					conversant = de.actorName;
					break;
				}
			}
		}
	}
}
