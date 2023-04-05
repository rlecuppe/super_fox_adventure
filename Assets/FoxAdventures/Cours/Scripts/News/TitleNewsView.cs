using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;

[System.Serializable]
public class TitleNewsViewEntry
{
    public string title;
    public string body;
    public DateTime displayedDate;

    public string DisplayedDateStr
    {
        get
        {
            return this.displayedDate.ToLongDateString();
        }
    }
}

public class TitleNewsView : MonoBehaviour
{
    // Content root
    public Transform contentRoot = null;

    // Content UI
    public Text contentText = null;

    // Show on
    public bool automaticShowLatestNews = false;

    void OnEnable()
    {
        // Hide by default
        this.HideView();

        // Show latest news
        if (this.automaticShowLatestNews == true)
            this.ShowLatestNews();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            this.HideView();
        }
    }

    public void ShowLatestNews()
    {
        // Trigger news show if logged in
        if (PlayfabAuth.IsLoggedIn == true)
        {
            PlayFab.PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest(), result => {
                Debug.Log("Got latest news!");
                // Create a TitleNewsViewEntry for each news item
                List<TitleNewsViewEntry> news = new List<TitleNewsViewEntry>();
                foreach (var newsItem in result.News)
                {
                    Debug.Log("News: " + newsItem.Title + " - " + newsItem.Body);
                    TitleNewsViewEntry entry = new TitleNewsViewEntry
                    {
                        title = newsItem.Title,
                        body = newsItem.Body,
                        displayedDate = newsItem.Timestamp
                    };
                    news.Add(entry);
                }
                this.OnGetTitleNewsSuccess(news);
            }, error => this.OnGetTitleNewsError());
        }
    }

    private void OnGetTitleNewsSuccess(List<TitleNewsViewEntry> news)
    {
        // News found
        if (news != null && news.Count > 0)
        {
            // Update Content
            if (this.contentText != null)
            {
                string newsContent = string.Empty;
                for (int i = 0; i < news.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(newsContent) == false)
                        newsContent += "\n\n";

                    // Fill content with our news
                    newsContent += "- " + news[i].DisplayedDateStr + " -";
                    newsContent += "\n<color=orange>" + news[i].title + "</color>";
                    newsContent += "\n" + news[i].body;
                }

                // Update view
                this.contentText.text = newsContent;
            }

            // Show
            this.ShowView();
        }
        // No news
        else
        {
            // Hide view immediately
            this.HideView();
        }
    }

    private void OnGetTitleNewsError()
    {
        // Log
        Debug.LogError("TitleNewsView.OnGetTitleNewsError() - Error: TODO");

        // Hide view immediately
        this.HideView();
    }


    // Show / Hide content root
    public void ShowView()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(true);
    }

    public void HideView()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(false);
    }
}
