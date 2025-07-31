using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CodeChecker : MonoBehaviour
{
    string[] blockTags = {"START", "DEFINE", "SETUP", "SETUP1",
        "LOOP", "LOOP1", "LOOP2","IFLOOP", "IFLOOPVAR", "IFLOOP1", "ELSELOOP", "ELSELOOP1", "DELAY" , "IFBLOCKHEADER"};
    public static bool listsVerified = false;
    public GameObject controllerMainBlock;
    public GameObject success, fail;

    public static List<KeyValuePair<string, string>> listOfAllChildren = new List<KeyValuePair<string, string>>();
    public List<KeyValuePair<string, string>> expectedListofChildren = new List<KeyValuePair<string, string>>();

    public void Start()
    {
        expectedListofChildren.Add(new KeyValuePair<string, string>("Player", "START"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("DEFINE", "START"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("SETUP", "START"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("SETUP1", "SETUP"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("LOOP", "START"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("LOOP1", "LOOP"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("LOOP2", "LOOP"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("IFLOOP", "LOOP"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("IFLOOPVAR", "IFBLOCKHEADER"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("IFBLOCKHEADER", "IFLOOP"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("IFLOOP1", "IFLOOP"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("ELSELOOP", "LOOP"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("ELSELOOP1", "ELSELOOP"));
        expectedListofChildren.Add(new KeyValuePair<string, string>("DELAY", "LOOP")); //////
    }

    private List<KeyValuePair<string, string>> AllChildren(GameObject root)
    {
        List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
        if (root.transform.childCount > 0)
        {
            foreach (Transform child in root.transform)
            {
                Searcher(result, child.gameObject);
            }
        }
        return result;
    }

    private void Searcher(List<KeyValuePair<string, string>> list, GameObject root)
    {
        list.Add(new KeyValuePair<string, string>(root.tag, root.transform.parent.tag));
        if (root.transform.childCount > 0)
        {
            foreach (Transform child in root.transform)
            {
                for (int i = 0; i < blockTags.Length; i++)
                {
                    if (child.CompareTag(blockTags[i]))
                    {
                        Searcher(list, child.gameObject);
                    }

                }
            }
        }
    }
    public void Update()
    {
        listOfAllChildren = AllChildren(controllerMainBlock);
    }

    public void OnPlayButtonClicked()
    {
        listsVerified = CompareX(listOfAllChildren, expectedListofChildren);

        if (listsVerified)
        {
            print("Lists are compared and verified");
            Debug.Log(listsVerified);
            success.SetActive(true);
        }
        else
        {
            Debug.Log("code check fail");
            fail.SetActive(true);
        }
    }

    bool CompareX(List<KeyValuePair<string, string>> list1, List<KeyValuePair<string, string>> list2)
    {
        bool returnValue = false; 
        int iterationCount = 0;
        int listCount = list1.Count;

        if (list1.Count != list2.Count)
        {
            return returnValue;
        }
        
        //comparing list items
        foreach (KeyValuePair<string, string> key in list1)
        {
            if (list2.Contains(key))
            {
                iterationCount++;
                if (iterationCount == listCount)
                {
                    returnValue = true;
                }
            }
            else
            {
                break;
            }
        }

        return returnValue;
    }
}
