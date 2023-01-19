CREATE TABLE IF NOT EXISTS accounts(
  id VARCHAR(255) NOT NULL primary key COMMENT 'primary key',
  createdAt DATETIME DEFAULT CURRENT_TIMESTAMP COMMENT 'Time Created',
  updatedAt DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP COMMENT 'Last Update',
  name varchar(255) COMMENT 'User Name',
  email varchar(255) COMMENT 'User Email',
  picture varchar(255) COMMENT 'User Picture'
) default charset utf8 COMMENT '';

CREATE TABLE IF NOT EXISTS albums(
id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
title VARCHAR(50) NOT NULL,
coverImg VARCHAR(255) NOT NULL DEFAULT 'https://images.unsplash.com/photo-1575485670541-824ff288aaf8?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1074&q=80',
category VARCHAR(25) NOT NULL DEFAULT 'misc',
archived BOOLEAN NOT NULL DEFAULT false,
creatorId VARCHAR(255) NOT NULL,

FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE
)default charset utf8 COMMENT '';


CREATE TABLE IF NOT EXISTS pictures(
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  imgUrl VARCHAR(255) NOT NULL,
  creatorId VARCHAR(255) NOT NULL,
  albumId INT NOT NULL,

  FOREIGN KEY (creatorId) REFERENCES accounts (id) ON DELETE CASCADE,
  FOREIGN KEY (albumId) REFERENCES albums (id) ON DELETE CASCADE
)default charset utf8 COMMENT '';

CREATE TABLE albumMembers(
  id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
  albumId INT NOT NULL,
  accountId VARCHAR(255) NOT NULL,

  Foreign Key (albumId) REFERENCES albums (id) ON DELETE CASCADE,
  Foreign Key (accountId) REFERENCES accounts(id) ON DELETE CASCADE
)default charset utf8;

INSERT INTO `albumMembers`
(`albumId`, `accountId`)
VALUES
(9, '6216b36ebc31a249987812b1');


INSERT INTO albums
(title,category,`coverImg`,`creatorId`)
VALUES
('Rats', 'Pugs', 'https://images.unsplash.com/photo-1575485670541-824ff288aaf8?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1074&q=80', '6216b36ebc31a249987812b0');
INSERT INTO albums
(title,category,`coverImg`,`creatorId`, archived)
VALUES
('Secret Rats', 'Pugs', 'https://images.unsplash.com/photo-1587404688696-048e51ee79e2?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1074&q=80', '6216b36ebc31a249987812b1', true);


SELECT
al.*,
ac.*
FROM albums al
JOIN accounts ac ON ac.id = al.creatorId
WHERE al.archived = true;


SELECT
ac.*,
am.id
FROM albumMembers am
JOIN accounts ac ON am.`accountId` = ac.id