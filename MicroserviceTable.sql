--One account could have more than one business phone number 
CREATE TABLE MessageDetails (
	BusinessID INT Primary Key, 
	PhoneNumber NVARCHAR(20) NOT NULL, 
	MessageBody VARCHAR(100) NULL, 
	SentTime DATETIME NOT NULL,
	AccountID INT NOT NULL
)


