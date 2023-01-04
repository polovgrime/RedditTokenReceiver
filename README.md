# RedditTokenReceiver
I used this one for my personal private project.

It's loosely based on [Reddit.NET's](https://github.com/sirkris/Reddit.NET) [solution](https://github.com/sirkris/Reddit.NET/tree/master/src/AuthTokenRetrieverLib) for getting reddit's tokens

## Description

Gets token through reddit's API using browser and saves it in a JSON file and prints it on a web page. For receiving tokens, app starts up an httpserver for receiving a single request with reddit's answer which will contain all the needed tokens 

## Setting up the application. Settings.json

### Preparing app on the reddit side
Take your reddit app and set redirect uri [here](https://www.reddit.com/prefs/apps)

![image](https://user-images.githubusercontent.com/28809629/210569708-43cfea1c-1569-4e48-aa95-295ef5d33518.png)

### Settings.json contents

```JSON
{
  "AppId": "your reddit app id",
  "BrowserPath": "path to browser",
  "Port": 8080, //port for http server
  "WaitForInput": true,
  "TokenSavePath": "save directory",
  "Scope": "creddits%20modcontributors%20modmail%20modconfig%20subscribe%20structuredstyles%20vote%20wikiedit%20mysubreddits%20submit%20modlog%20modposts%20modflair%20save%20modothers%20read%20privatemessages%20report%20identity%20livemanage%20account%20modtraffic%20wikiread%20edit%20modwiki%20modself%20history%20flair"
}
```

### Deufalt rights scope 
```
creddits%20modcontributors%20modmail%20modconfig%20subscribe%20structuredstyles%20vote%20wikiedit%20mysubreddits%20submit%20modlog%20modposts%20modflair%20save%20modothers%20read%20privatemessages%20report%20identity%20livemanage%20account%20modtraffic%20wikiread%20edit%20modwiki%20modself%20history%20flair
```
You can edit needed rights here as you see fit
