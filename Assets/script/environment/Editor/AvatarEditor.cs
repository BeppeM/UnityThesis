using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


[CustomEditor(typeof(AbstractAvatar), true)]
public class AvatarEditor : Editor
{

    public VisualTreeAsset visualTree;
    private VisualElement root;

    private List<string> agentNames = new List<string>();
    // Script associated to the dynamic inspector
    private AbstractAvatar avatarScript;

    private List<string> filteredItems;

    private ToolbarSearchField searchbar;
    private ListView agentListView;
    private Button addFriendButton;
    private VisualElement actualFriendsContainer;
    private ListView actualFriendsListView;

    private void OnEnable()
    {
        avatarScript = (AbstractAvatar)target;
        // Mark the object as dirty to ensure changes are saved
        EditorUtility.SetDirty(avatarScript);

        // Find all avatars in the game and retrieve the names
        agentNames = GameObject.FindGameObjectsWithTag("JacamoAgent")
                                .Select(enemy => enemy.name.ToLower())
                                .ToList();
    }


    public override VisualElement CreateInspectorGUI()
    {
        root = new VisualElement();
        // Add UI builder into the root to update the UI of the inspector
        visualTree.CloneTree(root);

        //// Retrieve UI elements from the root
        //searchbar = root.Q<ToolbarSearchField>("searchFriend");
        //addFriendButton = root.Q<Button>("addFriend");
        //agentListView = root.Q<ListView>("agentNames");
        //agentListView.selectionType = SelectionType.Single;
        //actualFriendsContainer = root.Q<VisualElement>("actualFriendsContainer");
        //actualFriendsListView = root.Q<ListView>("actualFriends");

        //showFriends();

        //// Make list view dynamic
        //searchbar.RegisterValueChangedCallback(evt =>
        //{
        //    string searchText = evt.newValue.ToLower();
        //    if (searchText != "")
        //    {
        //        filteredItems = agentNames.Where(item => item.ToLower().Contains(searchText)).ToList();
        //    }
        //    else
        //    {
        //        filteredItems.Clear();
        //    }
        //    UpdateListView();
        //});

        //string agentName = "";

        //// On double click on the list view item
        //agentListView.itemsChosen += objects =>
        //{
        //    searchbar.value = objects.First().ToString();
        //    agentName = objects.First().ToString();
        //};

        //// OnClick to add new friend
        //addFriendButton.clicked += () =>
        //{            
        //    if (agentName != "")
        //    {
        //        if (!avatarScript.InitialAgentBeliefs.Friends.Contains(agentName))
        //        {
        //            avatarScript.InitialAgentBeliefs.Friends.Add(agentName);
        //        }
        //        Debug.Log("Actual friends " + string.Join(", ", avatarScript.InitialAgentBeliefs.Friends));
        //        showFriends();
        //        // Reset agentName
        //        agentName = "";
        //    }
        //    else
        //    {
        //        Debug.Log("Search the friend you want to add");
        //    }
        //};

        return root;
    }

    private void showFriends()
    {
        if (avatarScript.InitialAgentBeliefs.Friends.Count == 0 || avatarScript.InitialAgentBeliefs.Friends == null)
        {
            actualFriendsContainer.style.display = DisplayStyle.None;
        }
        else
        {
            actualFriendsContainer.style.display = DisplayStyle.Flex;
            actualFriendsListView.itemsSource = avatarScript.InitialAgentBeliefs.Friends;
            actualFriendsListView.makeItem = () => new Label();
            actualFriendsListView.bindItem = (element, i) => (element as Label).text = avatarScript.InitialAgentBeliefs.Friends[i];
            actualFriendsListView.Rebuild();
        }
    }

    private void UpdateListView()
    {
        agentListView.itemsSource = filteredItems;
        agentListView.makeItem = () => new Label();
        agentListView.bindItem = (element, i) => (element as Label).text = filteredItems[i];
        agentListView.Rebuild();
    }

}
