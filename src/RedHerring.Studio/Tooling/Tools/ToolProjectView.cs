using IconFonts;
using ImGuiNET;
using RedHerring.ImGui;
using RedHerring.Studio.Models;
using RedHerring.Studio.Models.Project.FileSystem;
using Gui = ImGuiNET.ImGui;

namespace RedHerring.Studio.Tools;

[Tool(ToolName)]
public sealed class ToolProjectView : ATool
{
	public const string ToolName = "Project view";
	
	private const ImGuiTreeNodeFlags TreeCommonFlags       = ImGuiTreeNodeFlags.SpanAvailWidth;
	private const ImGuiTreeNodeFlags TreeInternalNodeFlags = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick | TreeCommonFlags;
	private const ImGuiTreeNodeFlags TreeLeafNodeFlags     = ImGuiTreeNodeFlags.Leaf        | ImGuiTreeNodeFlags.NoTreePushOnOpen  | TreeCommonFlags;
	
	protected override string Name => ToolName;
    
	public ToolProjectView(StudioModel studioModel) : base(studioModel)
	{
	}
    
	public ToolProjectView(StudioModel studioModel, int uniqueId) : base(studioModel, uniqueId)
	{
	}

	public override void Update(out bool finished)
	{
		finished = UpdateUI();
	}

	private bool UpdateUI()
	{
		bool isOpen = true;
		if (Gui.Begin(NameWithSalt, ref isOpen))
		{
			//Tree();
			UpdateFolder(StudioModel.Project.AssetsFolder);
			
			Gui.End();
		}

		return !isOpen;
	}

	private void UpdateFolder(ProjectFolderNode? node)
	{
		if (node is null)
		{
			return;
		}

		Icon.Folder(node.Children.Count == 0);
		bool nodeExpanded = UpdateNode(node, TreeInternalNodeFlags);

		if (nodeExpanded)
		{
			foreach (AProjectNode child in node.Children)
			{
				if (child is ProjectFolderNode folder)
				{
					UpdateFolder(folder);
				}
				else if (child is ProjectFileNode file)
				{
					UpdateFile(file);
				}
			}
            
			Gui.TreePop();
		}
	}
    
	private void UpdateFile(ProjectFileNode node)
	{
		Icon.File(node.Path);
		UpdateNode(node, TreeLeafNodeFlags);
	}
	
	private bool UpdateNode(AProjectNode node, ImGuiTreeNodeFlags flags)
	{
		string id = node.Meta.Guid!;

		if (StudioModel.Selection.IsSelected(id))
		{
			flags |= ImGuiTreeNodeFlags.Selected;
		}

		bool nodeExpanded = Gui.TreeNodeEx(id, flags, node.Name);

		if (Gui.IsItemClicked() && !Gui.IsItemToggledOpen())
		{
			HandleSelection(id);
		}

		return nodeExpanded;
	}

	private void HandleSelection(string id)
	{
		if(Gui.GetIO().KeyCtrl)
		{
			StudioModel.Selection.Flip(id);
			return;
		}

		if (Gui.GetIO().KeyShift)
		{
			// TODO
			return;
		}

		StudioModel.Selection.DeselectAll();
		StudioModel.Selection.Select(id);
	}

















	private ImGuiTreeNodeFlags base_flags                          = ImGuiTreeNodeFlags.OpenOnArrow | ImGuiTreeNodeFlags.OpenOnDoubleClick | ImGuiTreeNodeFlags.SpanAvailWidth;
	private bool               align_label_with_current_x_position = false;
	private bool               test_drag_and_drop                  = false;
    
	// 'selection_mask' is dumb representation of what may be user-side selection state.
	//  You may retain selection state inside or outside your objects in whatever format you see fit.
	// 'node_clicked' is temporary storage of what node we have clicked to process selection at the end
	/// of the loop. May be a pointer to your own node type, etc.
	private int selection_mask = (1 << 2);
    
	private void Tree()
	{
		{
			if (Gui.TreeNode("Basic trees"))
			{
				for (int i = 0; i < 5; i++)
				{
					// Use SetNextItemOpen() so set the default state of a node to be open. We could
					// also use TreeNodeEx() with the ImGuiTreeNodeFlags.DefaultOpen flag to achieve the same thing!
					if (i == 0)
						Gui.SetNextItemOpen(true, ImGuiCond.Once);

					if (Gui.TreeNode(i, $"Child {i}"))
					{
						Gui.Text("blah blah");
						Gui.SameLine();
						if (Gui.SmallButton("button")) {}
						Gui.TreePop();
					}
				}
				Gui.TreePop();
			}

			if (Gui.TreeNode("Advanced, with Selectable nodes"))
			{

				int flags = (int) base_flags;
				Gui.CheckboxFlags("ImGuiTreeNodeFlags.OpenOnArrow",       ref flags, (int)ImGuiTreeNodeFlags.OpenOnArrow);
				Gui.CheckboxFlags("ImGuiTreeNodeFlags.OpenOnDoubleClick", ref flags, (int)ImGuiTreeNodeFlags.OpenOnDoubleClick);
				Gui.CheckboxFlags("ImGuiTreeNodeFlags.SpanAvailWidth",    ref flags, (int)ImGuiTreeNodeFlags.SpanAvailWidth);
				Gui.CheckboxFlags("ImGuiTreeNodeFlags.SpanFullWidth",     ref flags, (int)ImGuiTreeNodeFlags.SpanFullWidth);
				base_flags = (ImGuiTreeNodeFlags)flags;
                
				Gui.Checkbox("Align label with current X position", ref align_label_with_current_x_position);
				Gui.Checkbox("Test tree node as drag source",       ref test_drag_and_drop);
				Gui.Text("Hello!");
				if (align_label_with_current_x_position)
					Gui.Unindent(Gui.GetTreeNodeToLabelSpacing());

				int node_clicked = -1;
				for (int i = 0; i < 6; i++)
				{
					// Disable the default "open on single-click behavior" + set Selected flag according to our selection.
					// To alter selection we use IsItemClicked() && !IsItemToggledOpen(), so clicking on an arrow doesn't alter selection.
					ImGuiTreeNodeFlags node_flags  = base_flags;
					bool               is_selected = (selection_mask & (1 << i)) != 0;
					if (is_selected)
						node_flags |= ImGuiTreeNodeFlags.Selected;
					if (i < 3)
					{
						// Items 0..2 are Tree Node
						bool node_open = Gui.TreeNodeEx(i, node_flags, $"Selectable Node {i}");
						if (Gui.IsItemClicked() && !Gui.IsItemToggledOpen())
							node_clicked = i;
						if (test_drag_and_drop && Gui.BeginDragDropSource())
						{
							Gui.SetDragDropPayload("_TREENODE", 0, 0);
							Gui.Text("This is a drag and drop source");
							Gui.EndDragDropSource();
						}
						if (node_open)
						{
							Gui.BulletText("Blah blah\nBlah Blah");
							Gui.TreePop();
						}
					}
					else
					{
						// Items 3..5 are Tree Leaves
						// The only reason we use TreeNode at all is to allow selection of the leaf. Otherwise we can
						// use BulletText() or advance the cursor by GetTreeNodeToLabelSpacing() and call Text().
						node_flags |= ImGuiTreeNodeFlags.Leaf | ImGuiTreeNodeFlags.NoTreePushOnOpen; // ImGuiTreeNodeFlags.Bullet
						Gui.TreeNodeEx(i, node_flags, $"Selectable Leaf {i}");
						if (Gui.IsItemClicked() && !Gui.IsItemToggledOpen())
							node_clicked = i;
						if (test_drag_and_drop && Gui.BeginDragDropSource())
						{
							Gui.SetDragDropPayload("_TREENODE", 0, 0);
							Gui.Text("This is a drag and drop source");
							Gui.EndDragDropSource();
						}
					}
				}
				if (node_clicked != -1)
				{
					// Update selection state
					// (process outside of tree loop to avoid visual inconsistencies during the clicking frame)
					if (Gui.GetIO().KeyCtrl)
						selection_mask ^= (1 << node_clicked);          // CTRL+click to toggle
					else //if (!(selection_mask & (1 << node_clicked))) // Depending on selection behavior you want, may want to preserve selection when clicking on item that is part of the selection
						selection_mask = (1 << node_clicked);           // Click to single-select
				}
				if (align_label_with_current_x_position)
					Gui.Indent(Gui.GetTreeNodeToLabelSpacing());
				Gui.TreePop();
			}
		}
	}
}