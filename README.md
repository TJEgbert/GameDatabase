Have you ever wanted to keep track of your game collection in an easy-to-use program that also allows you to search it easily? This is the program for you!  As you keep entering more games the search features keep increasing as well.  You also have a record where you can update the order you want to play the games.  This program uses IGDB API to make it easier for you to enter games into your database.  Also, your personal database is all stored locally on your machine.

Set up
1.	Follow the steps here https://api-docs.igdb.com/#getting-started for “Account Creation” since we are going to need the Client ID and Client Secret, so the program connects to the IGDB.  (Coming very soon as of writing this I’m going to add an option where you don’t have to have it, but it just means the search feature won’t work but the database is set up in a way where that doesn’t matter.)
2.	Then make sure you start the program in x86 configuration so you can access your personal database.
3.	Then if you did step one you can enter in the Client ID and Secret in the corresponding field.  This is all saved locally on your machine, so you won’t have to enter it every time. 
    Currently working on (Or if you want to continue without, click continue without Client ID and Secret).

4.	Then enjoy!  Have running in once it will create a .exe file you can make a shortcut to, so you don’t have to run in code.
   
Technical info
To run the connections to the API and Twitch I used RestEase to connect to and then NewtonSoft.Json to convert the retrieved responses into something the program and use.  Then to save the information about the games I used System.Data.OLeDB query the Microsoft Access database.

Exception
If you receive a message “Please go to the GitHub repository and follow the instructions from the 'Exception' section thank you”.  Please send an email exceptionshappens@gmail.com with the text from the error_log and a short description of what was going on.  This will be an ongoing project I will be working on so I would be grateful if you would let me know of any errors. 
To find the error_log of the .exe follow this path GameDatabase\bin\x86\Debug\net6.0-windows\