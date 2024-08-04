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

INSERT INTO Users (Name, LastName, Email, Password, BirthDate, Nationality, Province)
VALUES 
('John', 'Doe', 'john.doe@example.com', 'password123', '1990-01-01', 'USA', 'California'),
('Jane', 'Smith', 'jane.smith@example.com', 'password456', '1985-05-15', 'UK', 'London'),
('Carlos', 'Garcia', 'carlos.garcia@example.com', 'password789', '1992-09-21', 'Spain', 'Madrid'),
('Maria', 'Lopez', 'maria.lopez@example.com', 'password321', '1995-07-10', 'Argentina', 'Buenos Aires'),
('Akira', 'Yamamoto', 'akira.yamamoto@example.com', 'password654', '1988-03-22', 'Japan', 'Tokyo');

INSERT INTO Files (Name, Type, Path)
VALUES 
('profile1.jpg', 'jpg', '/images/profile1.jpg'),
('profile2.png', 'png', '/images/profile2.png'),
('video1.mp4', 'mp4', '/videos/video1.mp4'),
('doc1.pdf', 'pdf', '/docs/doc1.pdf'),
('post_image1.png', 'png', '/images/post_image1.png');

INSERT INTO Posts (Description, PostStatus, IdUser, IdFile)
VALUES 
('This is my first post!', 'Active', 1, NULL),
('Check out this awesome picture!', 'Active', 2, 5),
('Here is a video I wanted to share.', 'Active', 3, 3),
('Sharing some useful documents.', 'Active', 4, 4),
('Lovely day in the park!', 'Active', 5, NULL);

INSERT INTO Comments (Text, IdUser, IdPost)
VALUES 
('Great post!', 2, 1),
('Nice picture!', 3, 2),
('Thanks for sharing!', 4, 3),
('Very useful, thanks!', 5, 4),
('What a beautiful day!', 1, 5);

INSERT INTO Reactions (ReactionStatus, IdUser, IdPost)
VALUES 
('Recomendar', 1, 1),
('MeInteresa', 2, 2),
('Celebrar', 3, 3),
('Recomendar', 4, 4),
('MeInteresa', 5, 5);







