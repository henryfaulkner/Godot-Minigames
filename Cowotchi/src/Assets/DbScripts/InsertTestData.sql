INSERT INTO Eggs (Name, AnimalTypeId, IsHatched, CreatedDate, ModifiedDate, IsDeleted) 
VALUES
('Monterey', 1, 0, datetime('now'), datetime('now'), 0),
('Jack', 1, 1, datetime('now'), datetime('now'), 0),
('Colby', 1, 1, datetime('now'), datetime('now'), 0);

INSERT INTO HatchRequirements (EggId, HatchRequirementTypeId, ProgressIndex, Goal, CreatedDate, ModifiedDate, IsDeleted)
VALUES
(1, 1, 0, 3),
(2, 1, 3, 3),
(3, 1, 3, 3),
(4, 1, 0, 3);

INSERT INTO Animals (Name, StomachMax, LoveMax, AnimalTypeId, EggId, Level, XpOffset, CreatedDate, ModifiedDate, IsDeleted) 
VALUES
('Jack', 3, 3, 1, 2, 1, 0, datetime('now'), datetime('now'), 0),
('Colby', 3, 3, 2, 3, 1, 0, datetime('now'), datetime('now'), 0);

INSERT INTO Eggs (Name, AnimalTypeId, IsHatched, CreatedDate, ModifiedDate, IsDeleted) 
VALUES
('Cheddar', 1, 0, datetime('now'), datetime('now'), 0);