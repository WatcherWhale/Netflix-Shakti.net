# Netflix-Shakti.net
An unofficial .net wrapper for the not so public Netflix Shakti API.
It uses asynchronous methods to obtain information from the Shakti api.

## Usage
**For more info go to the wiki pages**

**Download from NuGet**
```nuget
PM> Install-Package NetflixShakti.net
```

```c#
using NetflixShakti;

Netflix netflix;

private async void DoNetflixStuff()
{
    //A cookie conatiner obtained after the user Logged in
    CookieContainer container;
    //This method builds a cookiecontainer from acookie string that can be obtained from a webbrowser in Windows Forms
    container = Netflix.BuildCoockieContainer(cookiestring);
    
    //Initialize
    //Needs a cookiecontainer or a string with the cookies and an user id that can be obtained with the Netflix.GetIDFromSource Function
    netflix = new Netflix(conatiner,Netflix.GetIdFromSource(browserSource));
    
    //To Load a ViewHistory in pages
    List<ViewHistory> historyPages = await netflix.GetViewHistory();
    
    //To build a single ViewHistory object from the whole list
    ViewHistory viewHistory = await netflix.GetViewHistoryFromPages(historyPages);
    
    //Loading Netflix profiles will happen automaticaly when the class intializes
    //However you can force to reload the profiles
    //This function is awaitable
    netflix.LoadNetflixProfiles();
    
    //To get all Profiles
    var profiles = netflix.Profiles.profiles;
    //To get the active profile
    var active = netflix.Profiles.active;
    
    //To change to an other profile
    //This function is awaitable
    netflix.SwitchProfile(profiles[2]);
}
```
## Want to help
The whole Shakti api is not really discovered you can help out by figuring out the Shakti api and document it and sending it to me. You can also make an example how the api can be used in c# (easier for me) or you can just straight up fork the project and send me your changes.

## License
The Shakti.net api falls under the MIT license.
