namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator.Queries;

public static class GroupQueries
{
    public static readonly string Insert = @"
                                                    INSERT INTO [gias].[Group]
                                                   ([Group UID]
                                                   ,[Group ID]
                                                   ,[Group Name]
                                                   ,[Companies House Number]
                                                   ,[Group Type (code)]
                                                   ,[Group Type]
                                                   ,[Closed Date]
                                                   ,[Group Status (code)]
                                                   ,[Group Status]
                                                   ,[Group Contact Street]
                                                   ,[Group Contact Locality]
                                                   ,[Group Contact Town]
                                                   ,[Group Contact Postcode]
                                                   ,[Head of Group Title]
                                                   ,[Head of Group First Name]
                                                   ,[Head of Group Last Name]
                                                   ,[UKPRN]
                                                   ,[Incorporated on (open date)]
                                                   ,[Open date])
                                             VALUES
                                                   (@GroupUID
                                                   ,@GroupId
                                                   ,@GroupName
                                                   ,@CompaniesHouseNumber
                                                   ,@GroupTypeCode
                                                   ,@GroupType
                                                   ,@ClosedDate
                                                   ,@GroupStatusCode
                                                   ,@GroupStatus
                                                   ,@GroupContactStreet
                                                   ,@GroupContactLocality
                                                   ,@GroupContactTown
                                                   ,@GroupContactPostcode
                                                   ,@HeadOfGroupTitle
                                                   ,@HeadOfGroupFirstName
                                                   ,@HeadOfGroupLastName
                                                   ,@Ukprn
                                                   ,@IncorporatedOnOpenDate
                                                   ,@OpenDate)";
}
