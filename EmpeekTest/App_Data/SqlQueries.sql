INSERT INTO PETS(Name) Values 
("Spike"),("Snowball"),("Santa")
,("Billy"),("Luccie")
,("Maggie"),("Vassy")

INSERT INTO Users(Name) Values 
("Alex"),("Kate"),("Trump")
,("Barack"),("Victor")
,("King")

INSERT INTO UserPets(UserId, PetId) SELECT 5, ID FROM PETS WHERE NAME = "Luccie"

SELECT UserId, count(PetId) as PetsCount  From UserPets Group BY (UserId) 