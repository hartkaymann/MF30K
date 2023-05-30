
CREATE TABLE users (
  id integer PRIMARY KEY,
  username varchar(255) UNIQUE NOT NULL,
  wins integer,
  losses integer,
  registrationDate date
);

CREATE TABLE runs (
  `id` integer PRIMARY KEY,
  `user_owner` integer NOT NULL,
  `combatLevel` integer,
  `playerLevel` integer,
  `goldsold` integer,
  `profession` varchar(255) NOT NULL,
  `race` varchar(255) NOT NULL
);

CREATE TABLE `combat` (
  `id` integer PRIMARY KEY,
  `combatLvlPlayer` integer,
  `combatLvlMonster` integer,
  `win` boolean,
  `owner_run` integer NOT NULL,
  `consequence` integer
);
