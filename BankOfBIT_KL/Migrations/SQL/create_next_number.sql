-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
CREATE PROCEDURE [dbo].[next_number]
	-- Add the parameters for the stored procedure here
	@Discriminator nvarChar(30),
	@NewVal bigint OUTPUT
AS
    SELECT @NewVal = NextAvailableNumber  
    FROM NextUniqueNumbers AS sp 
    WHERE Discriminator = @Discriminator;  

	BEGIN TRANSACTION
	UPDATE NextUniqueNumbers SET NextAvailableNumber = NextAvailableNumber + 1
	WHERE Discriminator = @Discriminator;
	COMMIT

RETURN
