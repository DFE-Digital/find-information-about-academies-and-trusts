using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data.AcademiesDb.Models.Cdm;

[ExcludeFromCodeCoverage] // Database model POCO
public class CdmSystemuser
{
    public int? Accessmode { get; set; }

    public Guid? Activedirectoryguid { get; set; }

    public Guid? Address1Addressid { get; set; }

    public int? Address1Addresstypecode { get; set; }

    public string? Address1City { get; set; }

    public string? Address1Composite { get; set; }

    public string? Address1Country { get; set; }

    public string? Address1County { get; set; }

    public string? Address1Fax { get; set; }

    public double? Address1Latitude { get; set; }

    public string? Address1Line1 { get; set; }

    public string? Address1Line2 { get; set; }

    public string? Address1Line3 { get; set; }

    public double? Address1Longitude { get; set; }

    public string? Address1Name { get; set; }

    public string? Address1Postalcode { get; set; }

    public string? Address1Postofficebox { get; set; }

    public int? Address1Shippingmethodcode { get; set; }

    public string? Address1Stateorprovince { get; set; }

    public string? Address1Telephone1 { get; set; }

    public string? Address1Telephone2 { get; set; }

    public string? Address1Telephone3 { get; set; }

    public string? Address1Upszone { get; set; }

    public int? Address1Utcoffset { get; set; }

    public Guid? Address2Addressid { get; set; }

    public int? Address2Addresstypecode { get; set; }

    public string? Address2City { get; set; }

    public string? Address2Composite { get; set; }

    public string? Address2Country { get; set; }

    public string? Address2County { get; set; }

    public string? Address2Fax { get; set; }

    public double? Address2Latitude { get; set; }

    public string? Address2Line1 { get; set; }

    public string? Address2Line2 { get; set; }

    public string? Address2Line3 { get; set; }

    public double? Address2Longitude { get; set; }

    public string? Address2Name { get; set; }

    public string? Address2Postalcode { get; set; }

    public string? Address2Postofficebox { get; set; }

    public int? Address2Shippingmethodcode { get; set; }

    public string? Address2Stateorprovince { get; set; }

    public string? Address2Telephone1 { get; set; }

    public string? Address2Telephone2 { get; set; }

    public string? Address2Telephone3 { get; set; }

    public string? Address2Upszone { get; set; }

    public int? Address2Utcoffset { get; set; }

    public Guid? Applicationid { get; set; }

    public string? Applicationiduri { get; set; }

    public Guid? Azureactivedirectoryobjectid { get; set; }

    public DateTime? Azuredeletedon { get; set; }

    public int? Azurestate { get; set; }

    public Guid? Businessunitid { get; set; }

    public string? Businessunitidname { get; set; }

    public Guid? Calendarid { get; set; }

    public int? Caltype { get; set; }

    public Guid? Createdby { get; set; }

    public string? Createdbyname { get; set; }

    public string? Createdbyyominame { get; set; }

    public DateTime? Createdon { get; set; }

    public Guid? Createdonbehalfby { get; set; }

    public string? Createdonbehalfbyname { get; set; }

    public string? Createdonbehalfbyyominame { get; set; }

    public bool? Defaultfilterspopulated { get; set; }

    public Guid? Defaultmailbox { get; set; }

    public string? Defaultmailboxname { get; set; }

    public string? Defaultodbfoldername { get; set; }

    public int? Deletedstate { get; set; }

    public string? Disabledreason { get; set; }

    public bool? Displayinserviceviews { get; set; }

    public string? Domainname { get; set; }

    public int? Emailrouteraccessapproval { get; set; }

    public string? Employeeid { get; set; }

    public long? EntityimageTimestamp { get; set; }

    public string? EntityimageUrl { get; set; }

    public Guid? Entityimageid { get; set; }

    public decimal? Exchangerate { get; set; }

    public string? Firstname { get; set; }

    public string? Fullname { get; set; }

    public string? Governmentid { get; set; }

    public string? Homephone { get; set; }

    public int? Identityid { get; set; }

    public int? Importsequencenumber { get; set; }

    public int? Incomingemaildeliverymethod { get; set; }

    public string? Internalemailaddress { get; set; }

    public int? Invitestatuscode { get; set; }

    public bool? Isactivedirectoryuser { get; set; }

    public bool? Isdisabled { get; set; }

    public bool? Isemailaddressapprovedbyo365admin { get; set; }

    public bool? Isintegrationuser { get; set; }

    public bool? Islicensed { get; set; }

    public bool? Issyncwithdirectory { get; set; }

    public string? Jobtitle { get; set; }

    public string? Lastname { get; set; }

    public DateTime? Latestupdatetime { get; set; }

    public string? Middlename { get; set; }

    public string? Mobilealertemail { get; set; }

    public Guid? Mobileofflineprofileid { get; set; }

    public string? Mobileofflineprofileidname { get; set; }

    public string? Mobilephone { get; set; }

    public Guid? Modifiedby { get; set; }

    public string? Modifiedbyname { get; set; }

    public string? Modifiedbyyominame { get; set; }

    public DateTime? Modifiedon { get; set; }

    public Guid? Modifiedonbehalfby { get; set; }

    public string? Modifiedonbehalfbyname { get; set; }

    public string? Modifiedonbehalfbyyominame { get; set; }

    public int? MsdynAgentType { get; set; }

    public string? MsdynBotapplicationid { get; set; }

    public string? MsdynBotdescription { get; set; }

    public string? MsdynBotendpoint { get; set; }

    public string? MsdynBothandle { get; set; }

    public int? MsdynBotprovider { get; set; }

    public string? MsdynBotsecretkeys { get; set; }

    public int? MsdynCapacity { get; set; }

    public Guid? MsdynDefaultpresenceiduser { get; set; }

    public string? MsdynDefaultpresenceidusername { get; set; }

    public bool? MsdynGdproptout { get; set; }

    public string? MsdynGridwrappercontrolfield { get; set; }

    public bool? MsdynIsexpertenabledforswarm { get; set; }

    public string? MsdynOwningenvironmentid { get; set; }

    public int? MsdynUsertype { get; set; }

    public string? Nickname { get; set; }

    public Guid? Organizationid { get; set; }

    public string? Organizationidname { get; set; }

    public int? Outgoingemaildeliverymethod { get; set; }

    public DateTime? Overriddencreatedon { get; set; }

    public Guid? Parentsystemuserid { get; set; }

    public string? Parentsystemuseridname { get; set; }

    public string? Parentsystemuseridyominame { get; set; }

    public int? Passporthi { get; set; }

    public int? Passportlo { get; set; }

    public string? Personalemailaddress { get; set; }

    public string? Photourl { get; set; }

    public Guid? Positionid { get; set; }

    public string? Positionidname { get; set; }

    public int? Preferredaddresscode { get; set; }

    public int? Preferredemailcode { get; set; }

    public int? Preferredphonecode { get; set; }

    public Guid? Processid { get; set; }

    public bool? PtmPeruserlicensingdocumentscorepack { get; set; }

    public bool? PtmPeruserlicensingdocumentscorepackserver { get; set; }

    public Guid? Queueid { get; set; }

    public string? Queueidname { get; set; }

    public string? Salutation { get; set; }

    public bool? Setupuser { get; set; }

    public string? Sharepointemailaddress { get; set; }

    public Guid? Siteid { get; set; }

    public string? Siteidname { get; set; }

    public string? Skills { get; set; }

    public Guid? Stageid { get; set; }

    public Guid? Systemuserid { get; set; }

    public Guid? Territoryid { get; set; }

    public string? Territoryidname { get; set; }

    public int? Timezoneruleversionnumber { get; set; }

    public string? Title { get; set; }

    public Guid? Transactioncurrencyid { get; set; }

    public string? Transactioncurrencyidname { get; set; }

    public string? Traversedpath { get; set; }

    public int? Userlicensetype { get; set; }

    public string? Userpuid { get; set; }

    public int? Utcconversiontimezonecode { get; set; }

    public long? Versionnumber { get; set; }

    public string? Windowsliveid { get; set; }

    public string? Yammeremailaddress { get; set; }

    public string? Yammeruserid { get; set; }

    public string? Yomifirstname { get; set; }

    public string? Yomifullname { get; set; }

    public string? Yomilastname { get; set; }

    public string? Yomimiddlename { get; set; }

    public Guid? Id { get; set; }
}
