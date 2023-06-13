CREATE TABLE `user` (
  `id` integer PRIMARY KEY,
  `username` varchar(255) UNIQUE NOT NULL,
  `wins` integer,
  `losses` integer,
  `created_at` timestamp
);

CREATE TABLE `run` (
  `id` integer PRIMARY KEY,
  `player_id` integer NOT NULL,
  `nickname` varchar(255),
  `combatlevel` integer,
  `playerlevel` integer,
  `goldsold` integer,
  `helped` integer,
  `rec_help` integer,
  `profession` varchar(255) NOT NULL,
  `race` varchar(255) NOT NULL
);

CREATE TABLE `combat` (
  `id` integer PRIMARY KEY,
  `combatlvl_player` integer,
  `combatlvl_monster` integer,
  `win` boolean,
  `run_id` integer NOT NULL,
  `helper` integer
);

ALTER TABLE `run` ADD FOREIGN KEY (`player_id`) REFERENCES `user` (`id`);

ALTER TABLE `combat` ADD FOREIGN KEY (`helper`) REFERENCES `user` (`id`);

ALTER TABLE `combat` ADD FOREIGN KEY (`run_id`) REFERENCES `run` (`id`);
