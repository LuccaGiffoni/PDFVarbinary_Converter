# PDF to Varbinary Converter
A console application developed to convert a PDF to Varbinary and automatically insert into a database.
All the code you need is here.

## How it works
You just need to clone this repo, then open the Visual Studio application. After that, change the fields from class 'ConnectionParameters' to use your own parameters to connect with database.
</br> After that you need to set the connection string to your database, setting the right fields to insert data. On my case, I use a VARBINARY(MAX) field and a VARCHAR(50) to allocate the filename.s

## SQL Database
Just to help, I inserted a SQL Server script to create a table to allocate the PDF conversions. You can find it inside the 'SQL Scripts' folder.

## Creating a executable (.exe) app
After you've done all changes you want it's time to compile the application. To do that, all you gotta do is:
</br></br>1. Open the Visual Studio application;
</br>2. Go to 'Compilation' on top nav bar;
</br>3. Select 'Publish PDFConverter';
</br>4. Select where you wanna publish it -- to test, you can choose 'Folder' option;
</br>5. Choose 'Folder' option again and select the specified folder you want to compile the app;
</br>6. Compile the app and that's it;

## How to use the application
The application have two functions: convert a PDF to Varbinary and insert it into a database;
</br>To use the app you need to set the Connection Parameters, inside the app or before compiling it.
