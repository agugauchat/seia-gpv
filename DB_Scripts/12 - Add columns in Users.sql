EXEC sp_RENAME 'Users.SuspendedDate', 'ActivationDate', 'COLUMN';

ALTER TABLE Users ADD FullName varchar(150);
ALTER TABLE Users ADD Phone varchar(25);
ALTER TABLE Users ADD Description varchar(500);
ALTER TABLE Users ADD Email varchar(150);
ALTER TABLE Users ADD ShowData bit NOT NULL  DEFAULT(0);
ALTER TABLE Users ADD BlockedPublications int NOT NULL DEFAULT(0);
ALTER TABLE Users ADD BlockedComments int NOT NULL DEFAULT(0);

ALTER TABLE Posts ADD DeletedByComplaints bit NOT NULL  DEFAULT(0);
ALTER TABLE Comments ADD DeletedByComplaints bit NOT NULL  DEFAULT(0);