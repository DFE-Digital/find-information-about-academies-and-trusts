namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator.Queries;

public static class GroupLinkQueries
{
    public static readonly string Insert = @"
                                                    DELETE FROM [gias].[GroupLink]

                                                   INSERT INTO [gias].[GroupLink]
                                                   ([URN]
                                                   ,[Group UID]
                                                   ,[Group ID]
                                                   ,[Group Name]
                                                   ,[Companies House Number]
                                                   ,[Group Type (code)]
                                                   ,[Group Type]
                                                   ,[Closed Date]
                                                   ,[Group Status (code)]
                                                   ,[Group Status]
                                                   ,[Joined date]
                                                   ,[EstablishmentName]
                                                   ,[TypeOfEstablishment (code)]
                                                   ,[TypeOfEstablishment (name)]
                                                   ,[PhaseOfEducation (code)]
                                                   ,[PhaseOfEducation (name)]
                                                   ,[LA (code)]
                                                   ,[LA (name)]
                                                   ,[Incorporated on (open date)]
                                                   ,[Open date]
                                                   ,[URN_GroupUID])
                                             VALUES
                                                   (@URN
                                                   ,@GroupUid
                                                   ,@GroupId
                                                   ,@GroupName
                                                   ,@CompaniesHouseNumber
                                                   ,@GroupTypeCode
                                                   ,@GroupType
                                                   ,@ClosedDate
                                                   ,@GroupStatusCode
                                                   ,@GroupStatus
                                                   ,@JoinedDate
                                                   ,@EstablishmentName
                                                   ,@TypeOfEstablishmentCode
                                                   ,@TypeOfEstablishmentName
                                                   ,@PhaseOfEducationCode
                                                   ,@PhaseOfEducationName
                                                   ,@LaCode
                                                   ,@LaName
                                                   ,@IncorporatedOnOpenDate
                                                   ,@OpenDate
                                                   ,@UrnGroupUid)";
}
