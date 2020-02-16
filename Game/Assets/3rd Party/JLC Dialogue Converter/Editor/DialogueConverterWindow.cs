using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PixelCrushers.DialogueSystem.DialogueEditor;
using PixelCrushers.DialogueSystem;

namespace JLC 
{

	public class DialogueConverterWindow : EditorWindow {


		[MenuItem("JLC/Dialogue/Dialogue Converter", false, 0)]
		public static void OpenJLCDialogueConverterWindow() {
			EditorWindow.GetWindow(typeof(DialogueConverterWindow), false, "Dialogue Converter");
		}

		private static class Styles
		{
			private static GUIStyle box;
			public static GUIStyle Box { get {
				
					if(box == null)
					{
						box = new GUIStyle(GUI.skin.box);
						box.normal.background = EditorGUIUtility.whiteTexture;
					}
					return box;
				}}

			private static GUIStyle container;
			public static GUIStyle Container{ get {

					if(container == null)
					{
						container = new GUIStyle(GUI.skin.box);
						container.normal.background = EditorGUIUtility.whiteTexture;
						container.padding = new RectOffset(0,0,0,0);
						container.margin = new RectOffset(10,10,10,10);
					}
					return container;
				}}

			public static Color Grey {get { return new Color(0.7f,0.7f,0.7f); } }
			public static Color DarkGrey {get { return new Color(0.5f,0.5f,0.5f); } }

			public static Color Teal {get { return new Color(0.25f, 0.80f, 0.60f); } } 
		}

		public DialogueDatabase database;
		private Vector2 scrollPos = new Vector2();

		//IMPORT
		private bool showImport = true;
		private List<ParsedConversation> parsedConversations = new List<ParsedConversation>();
		private string pastedText;
		private Template template;
		public ActorAliases aliases;
		private bool showAliases;

		//EXPORT
		private bool showExport;
		private List<ConversationSelector> conversationsToExport = new List<ConversationSelector>();


		public void OnEnable()
		{
			LoadTemplate();
		}

		void LoadTemplate()
		{
			template = TemplateTools.LoadFromEditorPrefs();
		}

		public void OnGUI()
		{
			EditorGUILayout.BeginVertical(Styles.Container);

			DrawToolBar();

			if(showImport) DrawImport();
			else if(showExport) DrawExport();

			EditorGUILayout.EndVertical();
		}

		private void DrawToolBar()
		{
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Toggle(showImport, "Import", "toolbarbutton"))
			{
				showImport = true;
				showExport = false;
			}

			if(GUILayout.Toggle(showExport, "Export", "toolbarbutton"))
			{
				showImport = false;
				showExport = true;
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawImport()
		{
			StartBox(Styles.Grey);

			GUILayout.Space(5);
			database = (DialogueDatabase) EditorGUILayout.ObjectField("Dialogue Database", database, typeof(DialogueDatabase), false);
			GUILayout.Space(5);

			DrawAliases();

			GUILayout.Space(10);


			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Paste from clipboard"))
			{
				pastedText = EditorGUIUtility.systemCopyBuffer;
				ParseConversations(pastedText);
			}


			if(GUILayout.Button("Open File"))
			{
				string dir = EditorUtility.OpenFilePanel("Load File","","txt");

				if(!string.IsNullOrEmpty(dir))
				{
					string text = File.ReadAllText(dir);
					ParseConversations(text);
				}
			}


			if(GUILayout.Button("Clear"))
			{
				parsedConversations.Clear();
				pastedText = "";
				GUI.FocusControl("");
				Repaint();
			}

			EditorGUILayout.EndHorizontal();

			if(parsedConversations.Count > 0)
			{
				scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
				foreach(ParsedConversation conversation in parsedConversations)
				{
					StartBox((conversation.IsSelected) ? Styles.Teal : Color.white);

					EditorGUILayout.BeginHorizontal();
					conversation.IsSelected = GUILayout.Toggle(conversation.IsSelected, conversation.title);

					EditorGUILayout.LabelField("Actor:", GUILayout.Width(40));
					if(GUILayout.Button(conversation.actor, EditorStyles.popup, GUILayout.MaxWidth(140)))
					{
						ActorSelector(conversation.actor, (x)=> { conversation.actor = x; });
					}

					EditorGUILayout.LabelField("Conversant:", GUILayout.Width(70));

					if(GUILayout.Button(conversation.conversant, EditorStyles.popup, GUILayout.MaxWidth(140)))
					{
						ActorSelector(conversation.conversant, (x)=> { conversation.conversant = x; });
					}
						
					EditorGUILayout.EndHorizontal();

					EndBox();

				}
				EditorGUILayout.EndScrollView();


				EditorGUILayout.BeginHorizontal();

				if(GUILayout.Button("Select All"))
				{
					foreach(ParsedConversation conversation in parsedConversations)
					{
						conversation.IsSelected = true;
					}
				}
				if(GUILayout.Button("Select None"))
				{
					foreach(ParsedConversation conversation in parsedConversations)
					{
						conversation.IsSelected = false;
					}
				}

				EditorGUILayout.EndHorizontal();


				if(database != null)
				{
					if(GUILayout.Button("Import", GUILayout.Height(30)))
					{
						if(ConversationsAreSelected())
						{
							if(EditorUtility.DisplayDialog("Import Conversations","This will import all selected conversations into the selected dialogue database. \n\nA backup of the current database will be created.\n\nAre you sure you want to continue?","Import", "Cancel"))
							{
								AddConversationsToDatabase();
							}
						}
						else
						{
							EditorUtility.DisplayDialog("No Conversations Selected", "Select some conversations from the list.", "Ok");
						}
					}
				}
			}

			EndBox();
		}

		private void DrawAliases()
		{
			StartBox(Styles.DarkGrey);

			EditorGUILayout.BeginHorizontal();
			showAliases = EditorGUILayout.Foldout(showAliases, "Actor Aliases", true, EditorStyles.foldout);
			if(showAliases & aliases != null)
			{
				
				if(GUILayout.Button("+", EditorStyles.miniButton, GUILayout.Width(20)))
				{
					aliases.aliases.Add(new ActorAliases.Alias());
				}
			}

			aliases = (ActorAliases) EditorGUILayout.ObjectField(aliases, typeof(ActorAliases), false);		

			if(aliases != null)
			{
				EditorGUILayout.LabelField("Case Sensitive", GUILayout.Width(80));
				aliases.caseSensitive = EditorGUILayout.Toggle(aliases.caseSensitive, GUILayout.Width(20));
			}

			EditorGUILayout.EndHorizontal();

			if(showAliases && aliases != null)
			{
				foreach(ActorAliases.Alias a in aliases.aliases)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Actor:", GUILayout.Width(40));
					if(GUILayout.Button(a.actorName, EditorStyles.popup, GUILayout.MaxWidth(140)))
					{
						ActorSelector(a.actorName, (x)=> { a.actorName = x; });
						EditorUtility.SetDirty(aliases);
					}

					EditorGUILayout.LabelField("Alias:", GUILayout.Width(40));
					a.alias = EditorGUILayout.TextField(a.alias);

					if(GUILayout.Button("-", EditorStyles.miniButton, GUILayout.Width(20)))
					{
						aliases.aliases.Remove(a);
						EditorUtility.SetDirty(aliases);
						GUIUtility.ExitGUI();
					}

					EditorGUILayout.EndHorizontal();
				}
				GUILayout.Space(5);
			}

			EndBox();
		}

		private void DrawExport()
		{
			StartBox(Styles.Grey);

			GUILayout.Space(10);
			EditorGUI.BeginChangeCheck();
			database = (DialogueDatabase) EditorGUILayout.ObjectField("Dialogue Database", database, typeof(DialogueDatabase), false);
			if(EditorGUI.EndChangeCheck())
			{
				LoadFromDatabase();
			}
			GUILayout.Space(10);

			if(GUILayout.Button("Refresh"))
			{
				LoadFromDatabase();
			}
				
			if(conversationsToExport.Count > 0)
			{
				scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
				foreach(ConversationSelector conversation in conversationsToExport)
				{
					StartBox((conversation.selected) ? Styles.Teal : Color.white);
					conversation.selected = GUILayout.Toggle(conversation.selected, conversation.conversation.Title);
					EndBox();

				}
				EditorGUILayout.EndScrollView();


				EditorGUILayout.BeginHorizontal();

				if(GUILayout.Button("Select None"))
				{
					foreach(ConversationSelector conversation in conversationsToExport)
					{
						conversation.selected = false;
					}
				}

				if(GUILayout.Button("Select All"))
				{
					foreach(ConversationSelector conversation in conversationsToExport)
					{
						conversation.selected = true;
					}
				}

				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();

				if(database != null)
				{
					if(GUILayout.Button("Export To Clipboard", GUILayout.Height(30)))
					{
						if(ConversationsAreSelected())
						{
							EditorGUIUtility.systemCopyBuffer = SerializeConversations();
						}
						else
						{
							EditorUtility.DisplayDialog("No Conversations Selected", "Select some conversations from the list.", "Ok");
						}
					}

					if(GUILayout.Button("Export To File", GUILayout.Height(30)))
					{
						if(ConversationsAreSelected())
						{
							ExportToFile(SerializeConversations());
						}
						else
						{
							EditorUtility.DisplayDialog("No Conversations Selected", "Select some conversations from the list.", "Ok");
						}
					}
				}

				EditorGUILayout.EndHorizontal();
			}

			EndBox();
		}

		void StartBox(Color color)
		{
			Color oldColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			EditorGUILayout.BeginVertical(Styles.Box);
			GUI.backgroundColor = oldColor;
		}

		void EndBox()
		{
			EditorGUILayout.EndVertical();
		}

		private void LoadFromDatabase()
		{
			conversationsToExport.Clear();

			if(database != null)
			{
				foreach(Conversation conversation in database.conversations)
				{
					conversationsToExport.Add(new ConversationSelector(conversation));
				}
			}
		}

		private void ActorSelector(string currentName, System.Action<string> callback)
		{
			if(database == null)
			{
				EditorUtility.DisplayDialog("No Database", "Assign a database in the window.", "Ok");
				return;
			}

			GenericMenu menu = new GenericMenu();

			menu.AddItem(new GUIContent("None"), currentName == "", ()=> {callback(null);});

			for(int i = 0; i < database.actors.Count; i++)
			{
				int index = i;
				menu.AddItem(new GUIContent(database.actors[index].Name), database.actors[index].Name == currentName, ()=> {
					callback(database.actors[index].Name);
				});
			}

			menu.ShowAsContext();
		}

		private string SerializeConversations()
		{
			List<Conversation> convos = new List<Conversation>();
			foreach(ConversationSelector cs in conversationsToExport)
			{
				if(cs.selected) convos.Add(cs.conversation);
			}
			ConversationSerializer serializer = new ConversationSerializer();
			return serializer.SerializeConversations(database, convos);
		}

		private void ParseConversations(string importedText)
		{
			ConversationParser parser = new ConversationParser();
			parsedConversations = parser.ParseConversations(importedText, aliases);
		}

		private void AddConversationsToDatabase()
		{
			BackupDatabase();

			foreach(ParsedConversation parsedConversation in parsedConversations)
			{
				if(!parsedConversation.IsSelected) continue;

				Conversation conversationToAdd = null;
				bool replace = false;

				foreach(Conversation databaseConversation in database.conversations)
				{
					if(databaseConversation.id == parsedConversation.id)
					{
						conversationToAdd = databaseConversation;
						replace = true;
						break;
					}
				}

				if(replace)
				{
					conversationToAdd.ActorID = database.GetActor(parsedConversation.actor).id;
					conversationToAdd.ConversantID = database.GetActor(parsedConversation.conversant).id;

					conversationToAdd.dialogueEntries.Clear();
					CreateStartNode(conversationToAdd);

					conversationToAdd.Title = parsedConversation.title;

					conversationToAdd.Description = parsedConversation.description;

				}
				else
				{
					conversationToAdd = CreateConversation(parsedConversation.id, parsedConversation.title, parsedConversation.actor, parsedConversation.conversant, parsedConversation.description);
				}

				string actor = database.GetActor(conversationToAdd.ActorID).Name;
				string conversant = database.GetActor(conversationToAdd.ConversantID).Name;

				string lastSpeaker = actor;

				foreach(ParsedDialogueEntry node in parsedConversation.nodes)
				{
					if(lastSpeaker != node.actorName)
					{
						conversant = lastSpeaker;
					}

					DialogueEntry newEntry = CreateDialogueEntry(node, parsedConversation.id, database.GetActor(conversant).id);

					lastSpeaker = node.actorName;


					conversationToAdd.dialogueEntries.Add(newEntry);

					foreach(Link link in parsedConversation.links)
					{
						if(link.originDialogueID == node.id)
						{
							newEntry.outgoingLinks.Insert(0, link);//Add(link);
						}
					}
				}

				//Add a link from the Start entry
				conversationToAdd.dialogueEntries[0].outgoingLinks.Add(parsedConversation.links[0]);

				if(!replace) database.AddConversation(conversationToAdd);
			}

		}

		private Conversation CreateConversation(int id, string title, string actor, string conversant, string description)
		{
			if(template == null) LoadTemplate();

			Conversation c = template.CreateConversation(id, title);
			c.ActorID = database.GetActor(actor).id;
			c.ConversantID = database.GetActor(conversant).id;
			c.Description = description;
			CreateStartNode(c);

			return c;
		}

		private void CreateStartNode(Conversation conversation)
		{
			if(template == null) LoadTemplate();

			DialogueEntry startNode = template.CreateDialogueEntry(0, conversation.id, "START");
			startNode.ActorID = conversation.ActorID;
			startNode.ConversantID = conversation.ConversantID;
			startNode.Sequence = "None()";
			conversation.dialogueEntries.Add(startNode);
		}

		private DialogueEntry CreateDialogueEntry(ParsedDialogueEntry node, int conversationID, int conversantID)
		{
			if(template == null) LoadTemplate();

			DialogueEntry newEntry = template.CreateDialogueEntry(node.id, conversationID, "");
			newEntry.ActorID = database.GetActor(node.actorName).id;
			newEntry.DialogueText = node.dialogueText;
			newEntry.ConversantID = conversantID;
			newEntry.Sequence = node.sequence;
			newEntry.MenuText = node.menuText;
			newEntry.conditionsString = node.conditions;

			Field.SetValue(newEntry.fields, "Description", node.description);

			if(!string.IsNullOrEmpty(node.responseMenuSequence))
			{
				//newEntry.ResponseMenuSequence = node.responseMenuSequence;
				newEntry.fields.Add(new Field("Response Menu Sequence", node.responseMenuSequence, FieldType.Text));
			}

			return newEntry;
		}


		private void ExportToFile(string text)
		{
			string dir = EditorUtility.SaveFilePanel("Save File","",database.name + " _ExportedConversations.txt" ,"txt");
			if(!string.IsNullOrEmpty(dir))
			{
				File.WriteAllText(dir, text);
			}
		}

		private string ImportFromFile()
		{
			string dir = EditorUtility.OpenFilePanel("Open File","","txt");

			if(!string.IsNullOrEmpty(dir))
			{
				string text = File.ReadAllText(dir);
				return text;
			}
			return null;
		}

		private void BackupDatabase()
		{
			string path = AssetDatabase.GetAssetPath(database);
			string dir = Path.GetDirectoryName(path);// + "/_Backups";
			string fileName = Path.GetFileNameWithoutExtension(path);
			string fileExt = Path.GetExtension(path);
			string dateTime = System.DateTime.Now.ToString("yy_MM_dd");

			string newPath = Path.Combine(dir, fileName + "_" + dateTime + fileExt);

			if(File.Exists(newPath))
			{
				for (int i = 1; i < 10000; i++) 
				{
					newPath = Path.Combine(dir, fileName + "_" + dateTime + "(" + i + ")" + fileExt);
					if(!File.Exists(newPath))
						break;
				}
			}

			AssetDatabase.CopyAsset(path, newPath);
			AssetDatabase.Refresh();
			Debug.Log("Made a backup of the database at: " + newPath);
		}

		private bool ConversationsAreSelected()
		{
			if(showImport)
			{
				foreach(ParsedConversation c in parsedConversations)
				{
					if(c.IsSelected) return true;
				}

				return false;
			}
			else if(showExport)
			{
				foreach(ConversationSelector c in conversationsToExport)
				{
					if(c.selected) return true;
				}

				return false;
			}

			return false;
		}


		public class ConversationSelector
		{
			public Conversation conversation;
			public bool selected;

			public ConversationSelector(Conversation conversation)
			{
				this.conversation = conversation;
			}
		}
	}	
}
