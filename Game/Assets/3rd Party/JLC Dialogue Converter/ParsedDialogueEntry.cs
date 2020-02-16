
namespace JLC
{
	public class ParsedDialogueEntry
	{
		public int id = 0;
		public string actorName = "";
		public string dialogueText = "";
		public string sequence = "";
		public string conditions = "";

		public string description = string.Empty;
		public string menuText = string.Empty;
		public string responseMenuSequence = string.Empty;

		public ParsedDialogueEntry(int id, string actorName, string dialogueText, string sequence, string conditions)
		{
			this.id = id;
			this.actorName = actorName;
			this.dialogueText = dialogueText;
			this.sequence = sequence;
			this.conditions = conditions;
		}
	}
}

