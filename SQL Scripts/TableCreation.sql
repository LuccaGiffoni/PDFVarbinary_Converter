-- Insert the name of your database here
USE <yourdatabase>

-- Here it's a script to create a table called 'archives'
CREATE TABLE archives
( id INT IDENTITY(1,1),
  pdfvarbinary VARBINARY(MAX) NOT NULL,
  pdfname VARCHAR(50) NOT NULL,
  CONSTRAINT id_pk PRIMARY KEY (id)
);
