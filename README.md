# TwitterSearchAPI_DotNet

This is a port of the Java project to use Twitter search with out TwitterAPI. 

See original repo: https://github.com/tomkdickinson/TwitterSearchAPI.

This project has some differences from the original Java-project, but in general it provides the same functionality.

## Requirements

This project targeting on the **.Net Standard 2.0**.

## Install

[Nuget](https://www.nuget.org/packages/TwitterSearchAPI): `Install-Package TwitterSearchAPI`

## How to it works?

The search engine does not persists all found tweets in internal collections, 
instead of this the search engine fire the special event that provides a tweets collection 
for current search iteration. The event listener decide what to do with this tweets.

## How to use

Firstly, we need to create the search engine and collection where we will store our tweets:

```C#
List<Tweet> tweets = new List<Tweet>();
TwitterSearch searchEngine = new TwitterSearch(new System.Net.Http.HttpClient(), () => tweets.Count <= 20);
```

The main part in a line above is `() => tweets.Count <= 20`, this code define the search engine execute condition.
While our list of tweets contains less than or equal to 20 tweets, the search engine will work as soon as the number of tweets is more than 20, then the search will be stopped.

Secondly, we subscribe to an event that provides us tweets that have been found and started searching:

```C#
searchEngine.TweetListReady += (s, e) =>
{
    tweets.AddRange(e.Tweets);
};
await searchEngine.SearchAsync("Awesome tweet", 100);
```

In a last line we pass the search query `Awesome tweet` and the interval between search iteration in millis.

It's all.