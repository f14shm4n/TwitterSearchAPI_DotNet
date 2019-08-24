# TwitterSearchAPI .Net Standard

This is a port of the Java project to use Twitter search with out TwitterAPI. 

See original repo: https://github.com/tomkdickinson/TwitterSearchAPI.

This project has some differences from the original Java-project, but in general it provides the same functionality.

**Note: The original Java code is completely reworked, but main algorithm here.**

## Requirements

- **.Net Standard 2.0**

## Install

[Nuget](https://www.nuget.org/packages/TwitterSearchAPI): `Install-Package TwitterSearchAPI`

## Supported methods of data extraction

- Search tweets by [*text or what you like*], this method do the same as you can achive using regular [twitter search](https://twitter.com/search-home);
- Extracts tweets from user timeline, for example, from this url: https://twitter.com/variety
- Extracts tweets from list timeline, for example, from this url: https://twitter.com/NYTMetro/lists/nyt-nyc-local-news
- Extracts a list of members from twitter list, for example, from this url: https://twitter.com/NYTMetro/lists/nyt-nyc-local-news/members
- Extracts a list of subscribers from twitter list, from example, from this url: https://twitter.com/NYTMetro/lists/nyt-nyc-local-news/subscribers 

## Usage

### Search tweets

```C#
// Define a list where we will store the results
List<Tweet> tweets = new List<Tweet>();
// Creates extractor
var extractor = new TweetExtractor(new HttpClient());            
// Run extractor
await extractor.SearchTweetsAsync(
    // Creates a search info with search query.
    new SearchExecutionInfo
    {
        // Don't worry about encoding special chars
        // it will work and with this input: "Tanya Tucker’s ‘While I’m Livin”"
        Query = "Telemundo Has &#8216;Game of Thrones&#8217; Hopes for &#8216;La Reina del Sur&#8217; Season 2 Premiere"
    },
    // Stop the extractor after we get 20 items at least.
    canExecute: () => tweets.Count <= 20,
    // Here we put found tweets into our list
    onTweetsExtracted: results =>
    {
        tweets.AddRange(results);
});

... Here you can work with tweets from list ...
```

### Extract tweets from user timeline

```C#
List<Tweet> tweets = new List<Tweet>();

var extractor = new TweetExtractor(new HttpClient());
await extractor.ExtractTweetsFromUserTimelineAsync(
    new ProfileTimelineExecutionInfo
    {
        // This we use any public user_screen_name like: 'Variety', 'Recode' and etc
        UserScreenName = "Variety"
    },
    canExecute: () => tweets.Count <= 20,
    onTweetsExtracted: results => tweets.AddRange(results)
);
```

### Extract tweets from list timeline

```C#
List<Tweet> tweets = new List<Tweet>();

var extractor = new TweetExtractor(new HttpClient());
await extractor.ExtractTweetsFromTimelineAsync(
    new TimelineExecutionInfo
    {
        // This we use any list timeline url like: 
        // "https://twitter.com/cspan/lists/members-of-congress"
        // "https://twitter.com/NYTMetro/lists/nyt-nyc-local-news"
        TimelineUrl = "https://twitter.com/NYTMetro/lists/nyt-nyc-local-news"
    },
    canExecute: () => tweets.Count <= 20,
    onTweetsExtracted: results => tweets.AddRange(results)
);
```

### Extract twitter list members

```C#
List<UserProfile> profiles = new List<UserProfile>();

var extractor = new UserExtractor(new HttpClient());
await extractor.ExtractTwitterListMembersAsync(
    new TwitterListExecutionInfo
    {
        // Input any twitter list url
        TwitterListUrl = "https://twitter.com/cspan/lists/members-of-congress"
    },
    canExecute: () => profiles.Count <= 20,
    onUsersExtracted: results => profiles.AddRange(results)
);
```

### Extract twitter list subscribers

```C#
List<UserProfile> profiles = new List<UserProfile>();

var extractor = new UserExtractor(new HttpClient());
await extractor.ExtractTwitterListSubscribersAsync(
    new TwitterListExecutionInfo
    {
        // Input any twitter list url
        TwitterListUrl = "https://twitter.com/cspan/lists/members-of-congress"
    },
    canExecute: () => profiles.Count <= 20,
    onUsersExtracted: results => profiles.AddRange(results)
);
```

# License

[MIT](https://github.com/f14shm4n/TwitterSearchAPI_DotNet/blob/master/LICENSE.md)
