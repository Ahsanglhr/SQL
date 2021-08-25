This is a Time-Registration system written for Brüel & Kjær by Klaus Elk
The initial version was written in my spare-time, but as the system grew and users were added it became a "real job".

The design philosophy is - to quote Einstein - to make it as simple as possible - but not simpler.
I only use the C# that comes with Visual Studio.
There are a few layers, but I am sure that there are those who will advocate for more layers.

From the bottom there is:

ADO.net

A database layer where all SQL-commands are encapsulated in functions with various error checks etc.
This is hand-written. I have tried various auto-generated layers and they tend to be hard to maintain.

A model layer. This is a singleton-class that keeps information on the current user as well as various lookup-lists - initialized from the database.

The UI-functionality. Many would argue that this ought to be split in more layers. My experience is that this was fast to write and not so hard to maintain.
Various model-control... layers are typically favoured but I dont find them easyer to maintain.
It is true however, that converting this into a web-application would have been a lot easyer with such layers.

=================================
To install the Express database open SQL Server Management Studio.
Create an empty database by right clicking on 'Databases' in the Object Explorer and select "New Database".
You may call it 'TimeReg' but you dont need to.
Choose the owner of the base.
Right-click the new database and select "New Query".
In the query view - paste in the contents of TimeReg.sql.
In the first line change the name 'TimeReg' to the name of your database.
Press F5 to run the query.

Double-click on TimeReg.sln to start Visual Studio and compile the Application.
Edit DBUtil to make the Connection-string match Server, Catalog and Login Name.
Recompile and test.



 