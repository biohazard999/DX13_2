# DevExpress 13.2 Review - Part 3 #

In the last review, I showed how to use the new TaskbarAssistent. Let's integrate it into XAF!

## WinForms ##
What we know so far is using the TaskbarAssistent. That component is pretty forward, except 1 part. Native resources for the Jumplist.


What i'd like the application to behave:

![](http://i.imgur.com/qpWcCCn.png) 

![](http://i.imgur.com/Tzd2V85.png)

If we add a new `IModelTaskbarJumplistJumpItemLaunch` we can use any application we'd like to launch like a ApplicationShortcut. 

![](http://i.imgur.com/uaHUxiW.png)

Let's see how this looks like:

