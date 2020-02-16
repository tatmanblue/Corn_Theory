using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using PixelCrushers.DialogueSystem;

namespace JLC
{
	public class ConversationSerializer
	{
		private List<BranchPoint> branchPoints = new List<BranchPoint>();
		private Conversation currentConversation;
		private StringBuilder builder;
		private DialogueDatabase database;

		private List<int> visitedIDs = new List<int>();

		public string SerializeConversations(DialogueDatabase database, List<Conversation> conversations)
		{
			this.database = database;
			currentConversation = null;

			builder = new StringBuilder();

			foreach(Conversation conversation in conversations)
			{
				branchPoints.Clear();
				currentConversation = conversation;
				TokenizeConversationName(conversation);
				CreateEntries(conversation);
				builder.AppendLine();
			}

			return builder.ToString();
		}

		private void CreateEntries(Conversation conversation)
		{
			visitedIDs.Clear();
			DialogueEntry currentEntry = conversation.dialogueEntries[0];

			while(currentEntry.outgoingLinks.Count > 0 || branchPoints.Count > 0)
			{
				currentEntry = GetNextEntry(currentEntry);
					
				if(branchPoints.Count > 0 && ( IsDeadEnd(currentEntry) || IsPickUpPoint(currentEntry)))
				{
					if(currentEntry != null && IsDeadEnd(currentEntry)) TokenizeEntry(currentEntry);

					BranchPoint bp = branchPoints[branchPoints.Count-1];

					TokenizeBranchName(bp.branchNodes[0]);
					bp.branchNodes.RemoveAt(0);
					builder.AppendLine();

					if(bp.branchNodes.Count > 0)
					{
						currentEntry = bp.branchNodes[0];
						TokenizeBranchName(bp.branchNodes[0]);
					}
					else
					{
						branchPoints.Remove(bp);

						while(branchPoints.Count > 0)
						{
							BranchPoint parentBranch = branchPoints[branchPoints.Count-1];
							TokenizeBranchName(parentBranch.branchNodes[0]);
							parentBranch.branchNodes.RemoveAt(0);

							if(parentBranch.branchNodes.Count > 0)
							{
								currentEntry = parentBranch.branchNodes[0];
								builder.AppendLine();
								TokenizeBranchName(parentBranch.branchNodes[0]);
								break;
							}
							else
							{
								branchPoints.Remove(parentBranch);
							}
						}
					}
				}

				if(currentEntry != null)
				{
					if(!HasAlreadyBeenVisited(currentEntry))
					{
						TokenizeEntry(currentEntry);
					}
					else if(currentEntry.outgoingLinks.Count > 0)
					{
						AddRecursiveErrorWarning();
						break;
					}
				}
				else
				{
					break;
				}
			}

		}

		private DialogueEntry GetNextEntry(DialogueEntry currentEntry)
		{
			int branchCount = currentEntry.outgoingLinks.Count;
			if(branchCount <= 0) return null;

			if(branchCount > 1)
			{
				List<DialogueEntry> branchNodes = new List<DialogueEntry>();
				for(int i = 0; i < currentEntry.outgoingLinks.Count; i++)
				{
					DialogueEntry node = currentConversation.GetDialogueEntry(currentEntry.outgoingLinks[i].destinationDialogueID);
					branchNodes.Add(node);
				}
				branchPoints.Add(new BranchPoint(currentEntry.id, branchNodes));

				builder.AppendLine();
				TokenizeBranchName(branchNodes[0], currentEntry);

				return branchNodes[0];
			}
			else
			{
				return currentConversation.GetDialogueEntry(currentEntry.outgoingLinks[0].destinationDialogueID);	
			}
		}

		private bool IsDeadEnd(DialogueEntry entry)
		{
			if(entry == null || entry.outgoingLinks.Count == 0) return true;

			return false;
		}

		private bool IsPickUpPoint(DialogueEntry entry)
		{
			if(entry == null) return false;

			int counter = 0;

			foreach(DialogueEntry de in currentConversation.dialogueEntries)
			{
				foreach(Link l in de.outgoingLinks)
				{
					if(l.destinationDialogueID == entry.id) counter++;
				}
			}
				
			return counter > 1;
		}

		private bool HasAlreadyBeenVisited(DialogueEntry entry)
		{
			for(int i = 0; i < visitedIDs.Count; i++)
			{
				if(visitedIDs[i] == entry.id) return true;
			}

			return false;
		}
			
		private void TokenizeConversationName(Conversation conversation)
		{
			if(!string.IsNullOrEmpty(conversation.Description))
			{
				builder.Append("// ")
					.Append(conversation.Description)
					.Append(" //")
					.AppendLine()
					.AppendLine();
			}

			builder.Append('#')
				.Append(conversation.id.ToString())
				.Append(' ')
				.AppendLine(conversation.Title.ToString())
				.AppendLine();
		}

		private void Indent()
		{
			if(branchPoints.Count > 0)
			{
				for(int i = 0; i < branchPoints.Count; i++)
				{
					builder.Append('\t');
				}
			}
		}

		private void TokenizeBranchName(DialogueEntry entry, DialogueEntry parent = null)
		{
			Indent();

			builder.Append('[')
				.Append(entry.MenuText)
				.Append(']');

			if(parent != null)
				TokenizeResponseMenuSequence(parent);

			builder.AppendLine();
		}

		private void TokenizeEntry(DialogueEntry entry)
		{
			Indent();

			TokenizeComment(entry);

			TokenizeSpeaker(entry);

			builder.Append(entry.DialogueText)
				.Append(' ');

			TokenizeSequence(entry);
			TokenizeConditions(entry);

			builder.AppendLine();

			visitedIDs.Add(entry.id);
		}

		private void TokenizeComment(DialogueEntry entry)
		{
			Field f = Field.Lookup(entry.fields, "Description");
			if(f != null && !string.IsNullOrEmpty(f.value))
			{
				builder.AppendLine()
					.AppendLine()
					.Append("// ")
					.Append(f.value)
					.Append(" //")
					.AppendLine();
			}
		}

		private void TokenizeSpeaker(DialogueEntry entry)
		{
			string speaker = database.GetActor(entry.ActorID).Name;
			builder.Append(speaker)
				.Append(": ");
		}

		private void TokenizeSequence(DialogueEntry entry)
		{
			string sequence = RemoveLineBreaks(entry.Sequence);


			if(!string.IsNullOrEmpty(sequence))
			{
				builder.Append('{')
					.Append(sequence)
					.Append('}');
			}
		}

		private void TokenizeResponseMenuSequence(DialogueEntry entry)
		{
			if(entry.HasResponseMenuSequence())
			{
				string responseMenu = RemoveLineBreaks(entry.ResponseMenuSequence);

				builder.Append(' ')
					.Append('{')
					.Append(responseMenu)
					.Append('}');
			}
		}

		private void TokenizeConditions(DialogueEntry entry)
		{
			string conditions = RemoveLineBreaks(entry.conditionsString);

			if(!string.IsNullOrEmpty(conditions))
			{
				builder.Append(' ')
					.Append('|')
					.Append(conditions)
					.Append('|');
			}
		}

		private string RemoveLineBreaks(string text)
		{
			if(string.IsNullOrEmpty(text)) return text;

			text = text.Replace("\n", string.Empty);
			text = text.Replace("\r", string.Empty);
			text = text.Replace("\t", string.Empty);

			return text;
		}

		private void AddRecursiveErrorWarning()
		{
			builder.Append("\nERROR. SERIALIZATION WAS ABORTED. THIS CONVERSATION HAS RECURSIVE/LOOPING LINKS, WHICH THE CONVERTER DOES NOT CURRENTLY SUPPORT.");
		}


		public class BranchPoint
		{
			public int startID;

			public List<DialogueEntry> branchNodes = new List<DialogueEntry>();

			public BranchPoint(int startID, List<DialogueEntry> branchNodes)
			{
				this.startID = startID;
				this.branchNodes = branchNodes;
			}
		}
	}

}
