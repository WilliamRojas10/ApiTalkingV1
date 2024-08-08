drop database talking;
show databases;

CREATE DATABASE talking;

USE talking;

CREATE TABLE IF NOT EXISTS Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(60) NOT NULL,
    LastName VARCHAR(60) NOT NULL,
    Email VARCHAR(60) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL,
    Nationality VARCHAR(60),
    Province VARCHAR(60),
    UserStatus ENUM('Active', 'Blocked', 'Deleted') DEFAULT 'Active' NOT NULL
);

CREATE TABLE IF NOT EXISTS Files (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(60) NOT NULL,
    Type VARCHAR(10) NOT NULL, -- jpg, png, mp4, etc
    Path VARCHAR(255) NOT NULL    
);

CREATE TABLE IF NOT EXISTS Posts (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Description TEXT,
    PostStatus ENUM('Active', 'Blocked', 'Deleted') DEFAULT 'Active' NOT NULL,
    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IdUser INT NOT NULL,
    IdFile INT,
    FOREIGN KEY (IdUser) REFERENCES Users(Id),
    FOREIGN KEY (IdFile) REFERENCES Files(Id)
);

CREATE TABLE IF NOT EXISTS Comments (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Text TEXT NOT NULL,
    CommentStatus ENUM('Sent', 'Reported', 'Deleted') DEFAULT 'Sent' NOT NULL,
    RegistrationDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IdUser INT NOT NULL,
    IdPost INT NOT NULL,
    FOREIGN KEY (IdUser) REFERENCES Users(Id),
    FOREIGN KEY (IdPost) REFERENCES Posts(Id)
);

CREATE TABLE IF NOT EXISTS Reactions (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ReactionStatus ENUM('Recomendar', 'MeInteresa', 'Celebrar') NOT NULL,
    IdUser INT NOT NULL,
    IdPost INT NOT NULL,
    FOREIGN KEY (IdUser) REFERENCES Users(Id),
    FOREIGN KEY (IdPost) REFERENCES Posts(Id)
);

-- Datos mock o de prueba
INSERT INTO Users (Name, LastName, Email, Password, BirthDate, Nationality, Province, UserStatus)
VALUES
  ('John', 'Doe', 'johndoe@example.com', 'password123', '1990-01-01', 'American', 'California', 'Active'),
  ('Jane', 'Smith', 'janesmith@example.com', 'password456', '1992-05-15', 'Canadian', 'Ontario', 'Active'),
  ('Michael', 'Johnson', 'michaeljohnson@example.com', 'password789', '1988-11-20', 'Mexican', 'Mexico City', 'Blocked');

INSERT INTO Files (Name, Type, Path)
VALUES
  ('image1.jpg', 'jpg', '/path/to/image1.jpg'),
  ('video2.mp4', 'mp4', '/path/to/video2.mp4');
  
  INSERT INTO Posts (Description, PostStatus, IdUser, IdFile)
VALUES
  ('This is a test post', 'Active', 1, 1),
  ('Another post', 'Active', 2, NULL);

INSERT INTO Comments (Text, CommentStatus, IdUser, IdPost)
VALUES
  ('Great post!', 'Sent', 2, 1),
  ('Interesting', 'Sent', 1, 2);
  
  INSERT INTO Reactions (ReactionStatus, IdUser, IdPost)
VALUES
  ('Recomendar', 1, 1),
  ('MeInteresa', 2, 1);

select * from Posts;
select * from Reactions;
