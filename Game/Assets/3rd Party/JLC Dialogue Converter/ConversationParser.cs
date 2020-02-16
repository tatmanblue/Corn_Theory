using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;
using PixelCrushers.DialogueSystem;

namespace JLC
{
	public class ConversationParser
	{
		private const int MaxSafeguard = 9999; // Limits loops to prevent infinite loop bugs.
		private int documentRow; //keep track of what line we're on for error reporting
		private int documentColumn;

		private List<ParsedConversation> parsedConversations = new List<ParsedConversation>();
		private ParsedConversation currentConversation;
		private int lastNodeID;

		private int depthLevel;
		private Branch mainBranch = null;

		private string menuText = string.Empty;

		private bool isCommenting = false;

		private ActorAliases aliases = null;
	
		public List<ParsedConversation> ParseConversations(string conversationString, ActorAliases abbreviationList)
		{
			parsedConversations.Clear();
			aliases = abbreviationList;

			currentConversation = null;

			try
			{
				StringReader documentReader = new StringReader(conversationString);

				string line;
				documentRow = 1;
				lastNodeID = 0;
				depthLevel = 0;
				mainBranch = new Branch("Main", 0, 0);

				bool tieUpLooseEnds = false;
				string comment = string.Empty;

				while((line = documentReader.ReadLine()) != null)
				{
					documentRow++;
					documentColumn = 1;

					if(IsNullOrWhiteSpace(line)) 
					{
						continue;
					}

					line.Trim();

					StringReader lineReader = new StringReader(line);
					ParseWhitespace(lineReader);

					if(IsStartOfNewConversation(lineReader))
					{
						int conversationID = ParseConversationID(lineReader);
						string conversationTitle = ParseConversationTitle(lineReader);

						currentConversation = new ParsedConversation(conversationID, conversationTitle);
						parsedConversations.Add(currentConversation);

						lastNodeID = 0;

						if(!string.IsNullOrEmpty(comment))
						{
							currentConversation.description = comment;
							comment = string.Empty;
						}

						continue;
					}

					if(isCommenting || IsComment(lineReader, line))
					{
						comment += ParseComment(lineReader);
						if(isCommenting) comment += "\n"; //retain new line for multi-line comments

						//if(lastNodeID == 0) currentConversation.description += comment;
						//else currentConversation.nodes[currentConversation.NodeCount-1].description += comment;

						continue;
					}

					if(IsSequence(lineReader)) // this is a sequence for the last node that was moved to a new line in the document
					{
						string sequence = ParseSequence(lineReader);
						currentConversation.nodes[currentConversation.NodeCount-1].sequence = sequence;
						continue;
					}

					if(IsBranchName(lineReader))
					{
						string branchName = ParseBranchName(lineReader);

						Branch currentBranch = mainBranch.GetActiveBranch();

						if(branchName == currentBranch.name) //End of active branch
						{
							currentBranch.complete = true;

							lastNodeID = currentBranch.startNode;
							tieUpLooseEnds = true;
						
							depthLevel--;

						}
						else
						{
							depthLevel++;

							Branch b = new Branch(branchName, lastNodeID, depthLevel);
							currentBranch.children.Add(b);

							menuText = branchName;

							if(currentBranch.children.Count == 1)
							{
								//Check for response menu sequence
								string responseMenuSequence = ParseSequence(lineReader);
								currentConversation.nodes[currentConversation.NodeCount-1].responseMenuSequence = responseMenuSequence;
							}

							tieUpLooseEnds = false;
						
						}
					}
					else
					{
						//PROCESS DIALOGUE ENTRY
						if(currentConversation == null)
						{
							throw new ParserException("No conversation ID/Title found.");
						}
						int nodeID = currentConversation.NodeCount + 1;
						ParsedDialogueEntry node = ParseEntry(lineReader, nodeID);
						if(node != null)
						{
							currentConversation.nodes.Add(node);

							if(tieUpLooseEnds)
							{
								ReconnectBranch(mainBranch, nodeID);
								if(depthLevel <= 0) mainBranch.children.Clear();
								tieUpLooseEnds = false;
							}
							else
							{
								currentConversation.AddLink(lastNodeID, nodeID);

								if(!string.IsNullOrEmpty(menuText))
								{
									node.menuText = menuText;
									menuText = string.Empty;
								}
							}

							Branch currentBranch = mainBranch.GetActiveBranch();
							currentBranch.endNodeID = nodeID;

							lastNodeID = nodeID;

							if(!string.IsNullOrEmpty(comment))
							{
								node.description = comment;
								comment = string.Empty;
							}

						}
					}
						
				}

				foreach(Branch b in mainBranch.children)
				{
					foreach(Branch branch in b.children)
					{
						if(!branch.complete)
						{
							throw new ParserException("No end tag found for Branch: \"" + branch.name + "\"");
						}
					}
				}

				foreach(ParsedConversation c in parsedConversations)
				{
					c.AutoFillActorNames();
				}

			}
			catch(ParserException e)
			{
				Debug.LogWarning("Syntax error " + e.Message + "at line " + documentRow + ", " + documentColumn);
				parsedConversations.Clear();
			}

			return parsedConversations;
		}

		private void ReconnectBranch(Branch branch, int nodeID)
		{
			bool childrenAreConnected = true;

			for(int i = branch.children.Count -1; i >= 0; i--)
			{
				if(!branch.children[i].hasReconnected)
				{
					childrenAreConnected = false;
					ReconnectBranch(branch.children[i], nodeID);
				}
			}

			if(childrenAreConnected && branch.depth > depthLevel)
			{
				currentConversation.AddLink(branch.endNodeID, nodeID);
				branch.hasReconnected = true;
			}
		}

		private int ParseConversationID(StringReader reader)
		{
			ParseWhitespace(reader);
			StringBuilder sb = new StringBuilder();

			int safeguard = 0;
			while (HasNextChar(reader) && safeguard < MaxSafeguard)
			{
				safeguard++;
				var c = (char)reader.Peek();
				if (char.IsDigit(c))
				{
					sb.Append(ReadNextChar(reader));
				}
				else
				{
					break;
				}
			}

			int id = -1;
			int.TryParse(sb.ToString(), out id);
			if(id != -1)
			{
				return id;
			}
			else
			{
				throw new ParserException("Can't read the conversation ID number");
			}

		}

		private string ParseConversationTitle(StringReader reader)
		{
			ParseWhitespace(reader);
			StringBuilder sb = new StringBuilder();

			int safeguard = 0;
			while (HasNextChar(reader) && safeguard < MaxSafeguard)
			{
				sb.Append(ReadNextChar(reader));
			}

			return sb.ToString().Trim();

		}

		private string ParseBranchName(StringReader reader)
		{
			ParseWhitespace(reader);
			StringBuilder sb = new StringBuilder();

			int safeguard = 0;
			while (HasNextChar(reader) && safeguard < MaxSafeguard)
			{
				safeguard++;
				var c = (char)reader.Peek();
				if (c == ']')
				{
					ReadNextChar(reader);
					break;
				}
				else
				{
					sb.Append(ReadNextChar(reader));
				}
			}

			return sb.ToString().Trim();

		}

		ParsedDialogueEntry ParseEntry(StringReader reader, int nodeID)
		{
			ParsedDialogueEntry node = null;

			string actor = ParseActorName(reader);
			string dialogueText = ParseDialogueText(reader);
			string sequence = ParseSequence(reader);
			string conditions = ParseConditions(reader);

			if(!string.IsNullOrEmpty(actor))
			{
				node = new ParsedDialogueEntry(nodeID, actor, dialogueText, sequence, conditions);
			}

			return node;
		}

		private string ParseActorName(StringReader reader)
		{
			ParseWhitespace(reader);

			StringBuilder sb = new StringBuilder();

			bool foundColon = false;
			int safeguard = 0;
			while(HasNextChar(reader) && safeguard < MaxSafeguard)
			{
				safeguard++;
				char c = (char)reader.Peek();
				if(c == ':')
				{
					foundColon = true;
					ReadNextChar(reader);
					break;
				}
				else
				{
					sb.Append(ReadNextChar(reader));
				}
			}
				
			if(foundColon)
			{
				return CheckForAlias(sb.ToString());
			}
			else
			{
				documentColumn = 1;
				throw new ParserException("Was expecting an Actor name, but couldn't parse it.");
			}
		}

		private string CheckForAlias(string name)
		{
			if(aliases == null) return name;

			string actor = aliases.FindActorFromAlias(name);
			return actor;

		}

		private string ParseDialogueText(StringReader reader)
		{
			ParseWhitespace(reader);

			StringBuilder sb = new StringBuilder();

			int safeguard = 0;
			while(HasNextChar(reader) && safeguard < MaxSafeguard)
			{
				safeguard++;
				char c = (char)reader.Peek();
				if(c == '{' || c == '|') 
				{
					break;
				}
				else
				{
					sb.Append(ReadNextChar(reader));
				}
			}

			return sb.ToString();
		}

		private string ParseSequence(StringReader reader)
		{
			ParseWhitespace(reader);

			if(!IsNextChar(reader, '{'))
			{
				return string.Empty;

			}

			ReadNextChar(reader);

			StringBuilder sb = new StringBuilder();

			int safeguard = 0;
			while(HasNextChar(reader) && safeguard < MaxSafeguard)
			{
				safeguard++;
				char c = (char)reader.Peek();
				if(c == '}') 
				{
					ReadNextChar(reader);
					break;
				}
				else
				{
					sb.Append(ReadNextChar(reader));
				}
			}

			return sb.ToString();
		}

		private string ParseConditions(StringReader reader)
		{
			ParseWhitespace(reader);

			if(!IsNextChar(reader, '|'))
			{
				return "";
			}

			ReadNextChar(reader);

			StringBuilder sb = new StringBuilder();

			int safeguard = 0;
			while(HasNextChar(reader) && safeguard < MaxSafeguard)
			{
				safeguard++;
				char c = (char)reader.Peek();
				if(c == '|') 
				{
					ReadNextChar(reader);
					break;
				}
				else
				{
					sb.Append(ReadNextChar(reader));
				}
			}
			return sb.ToString();
		}

		private string ParseComment(StringReader reader)
		{
			ParseWhitespace(reader);

			StringBuilder sb = new StringBuilder();

			char lastChar = ' ';

			int safeguard = 0;
			while(HasNextChar(reader) && safeguard < MaxSafeguard)
			{
				safeguard++;
				char c = (char)reader.Peek();

				if(c == '/' && lastChar == '/')
				{
					sb.Remove(sb.Length-1, 1); //remove the forward slash that we just added, since the comment is closing.
					isCommenting = false;
					break;
				}
				else
				{
					lastChar = c;
					sb.Append(ReadNextChar(reader));
				}
			}

			return sb.ToString();
		}



		private void ParseWhitespace(StringReader reader)
		{
			int safeguard = 0;
			while (IsNextCharWhiteSpace(reader) && safeguard < MaxSafeguard)
			{
				safeguard++;
				ReadNextChar(reader);
			}
		}

		private bool IsNextCharWhiteSpace(StringReader reader)
		{
			return HasNextChar(reader) && char.IsWhiteSpace((char)reader.Peek());
		}

		private bool IsNextChar(StringReader reader, char requiredChar)
		{
			return HasNextChar(reader) && (char)reader.Peek() == requiredChar;
		}

		private bool IsNextCharNot(StringReader reader, char requiredChar)
		{
			return HasNextChar(reader) && (char)reader.Peek() != requiredChar;
		}

		private bool HasNextChar(StringReader reader)
		{
			return reader != null && reader.Peek() != -1;
		}

		private char ReadNextChar(StringReader reader)
		{
			var c = (char)reader.Read();
			documentColumn++;
			return c;
		}

		private void ParseChar(StringReader reader, char requiredChar)
		{
			if (IsNextChar(reader, requiredChar))
			{
				ReadNextChar(reader);
			}
			else
			{
				throw new ParserException("Expected '" + requiredChar + "'");
			}
		}
			
		private bool IsStartOfNewConversation(StringReader reader)
		{
			if(IsNextChar(reader, '#'))
			{
				ReadNextChar(reader);
				return true;
			}

			return false;
		}


		private bool IsComment(StringReader reader, string line)
		{
			if(line.StartsWith("//"))
			{
				ReadNextChar(reader);
				ReadNextChar(reader);
				isCommenting = true;
				return true;
			}
			else if(line.StartsWith("/"))
			{
				documentColumn = 0;
				throw new ParserException("Comments must start with \"//\"");
			}

			return false;
		}

		private bool IsSequence(StringReader reader)
		{
			if(IsNextChar(reader, '{'))
			{
				ReadNextChar(reader);
				return true;
			}

			return false;
		}

		private bool IsBranchName(StringReader reader)
		{
			if(IsNextChar(reader, '['))
			{
				ReadNextChar(reader);
				return true;
			}

			return false;
		}

		private bool IsNullOrWhiteSpace(string value)
		{
			if (value != null)
			{
				for (int i = 0; i < value.Length; i++)
				{
					if (!char.IsWhiteSpace(value[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		public class Branch
		{
			public string name;
			public int depth;
			public int startNode;
			public int endNodeID;

			public List<Branch> children = new List<Branch>();
			public bool complete = false;
			public bool hasReconnected = false;

			public Branch(string name, int startNode, int depthLevel)
			{
				this.name = name;
				this.startNode = startNode;
				this.depth = depthLevel;
			}

			public Branch GetActiveBranch()
			{
				Branch b = this;
				if(children.Count > 0)
				{
					if(!children[children.Count-1].complete)
					{
						b = children[children.Count-1];
						b = b.GetActiveBranch();
					}
				}

				return b;
			}

			public void RemoveCompletedBranches()
			{
				for(int i = children.Count-1; i > 0; i--)
				{
					children[i].RemoveCompletedBranches();
					if(children[i].complete) children.RemoveAt(i);
				}
			}
		}

	}
}