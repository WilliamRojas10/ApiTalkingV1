CREATE DATABASE talking;

USE talking;

CREATE TABLE Users (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(60) NOT NULL,
    LastName VARCHAR(60) NOT NULL,
    Email VARCHAR(60) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL,
    Nationality VARCHAR(60),
    Province VARCHAR(60),
    UserStatus ENUM ('Active', 'Blocked', 'Deleted') DEFAULT 'Active' NOT NULL
);

-- Inserta datos de prueba
INSERT INTO Users (Name, LastName, Email, Password, BirthDate, Nationality, Province, UserStatus)
VALUES
('John', 'Doe', 'john.doe@example.com', 'password123', '1985-05-15', 'American', 'California', 'Active'),
('Jane', 'Smith', 'jane.smith@example.com', 'securepassword', '1990-07-22', 'Canadian', 'Ontario', 'Active');

SELECT * FROM Users;
