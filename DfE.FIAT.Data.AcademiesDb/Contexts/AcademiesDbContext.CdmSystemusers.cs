using System.Diagnostics.CodeAnalysis;
using DfE.FIAT.Data.AcademiesDb.Models.Cdm;
using Microsoft.EntityFrameworkCore;

namespace DfE.FIAT.Data.AcademiesDb.Contexts;

public partial class AcademiesDbContext
{
    public DbSet<CdmSystemuser> CdmSystemusers { get; set; }

    [ExcludeFromCodeCoverage]
    protected static void OnModelCreatingCdmSystemusers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CdmSystemuser>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("systemuser", "cdm");

            entity.Property(e => e.Accessmode).HasColumnName("accessmode");
            entity.Property(e => e.Activedirectoryguid).HasColumnName("activedirectoryguid");
            entity.Property(e => e.Address1Addressid).HasColumnName("address1_addressid");
            entity.Property(e => e.Address1Addresstypecode).HasColumnName("address1_addresstypecode");
            entity.Property(e => e.Address1City).HasColumnName("address1_city");
            entity.Property(e => e.Address1Composite).HasColumnName("address1_composite");
            entity.Property(e => e.Address1Country).HasColumnName("address1_country");
            entity.Property(e => e.Address1County).HasColumnName("address1_county");
            entity.Property(e => e.Address1Fax).HasColumnName("address1_fax");
            entity.Property(e => e.Address1Latitude).HasColumnName("address1_latitude");
            entity.Property(e => e.Address1Line1).HasColumnName("address1_line1");
            entity.Property(e => e.Address1Line2).HasColumnName("address1_line2");
            entity.Property(e => e.Address1Line3).HasColumnName("address1_line3");
            entity.Property(e => e.Address1Longitude).HasColumnName("address1_longitude");
            entity.Property(e => e.Address1Name).HasColumnName("address1_name");
            entity.Property(e => e.Address1Postalcode).HasColumnName("address1_postalcode");
            entity.Property(e => e.Address1Postofficebox).HasColumnName("address1_postofficebox");
            entity.Property(e => e.Address1Shippingmethodcode).HasColumnName("address1_shippingmethodcode");
            entity.Property(e => e.Address1Stateorprovince).HasColumnName("address1_stateorprovince");
            entity.Property(e => e.Address1Telephone1).HasColumnName("address1_telephone1");
            entity.Property(e => e.Address1Telephone2).HasColumnName("address1_telephone2");
            entity.Property(e => e.Address1Telephone3).HasColumnName("address1_telephone3");
            entity.Property(e => e.Address1Upszone).HasColumnName("address1_upszone");
            entity.Property(e => e.Address1Utcoffset).HasColumnName("address1_utcoffset");
            entity.Property(e => e.Address2Addressid).HasColumnName("address2_addressid");
            entity.Property(e => e.Address2Addresstypecode).HasColumnName("address2_addresstypecode");
            entity.Property(e => e.Address2City).HasColumnName("address2_city");
            entity.Property(e => e.Address2Composite).HasColumnName("address2_composite");
            entity.Property(e => e.Address2Country).HasColumnName("address2_country");
            entity.Property(e => e.Address2County).HasColumnName("address2_county");
            entity.Property(e => e.Address2Fax).HasColumnName("address2_fax");
            entity.Property(e => e.Address2Latitude).HasColumnName("address2_latitude");
            entity.Property(e => e.Address2Line1).HasColumnName("address2_line1");
            entity.Property(e => e.Address2Line2).HasColumnName("address2_line2");
            entity.Property(e => e.Address2Line3).HasColumnName("address2_line3");
            entity.Property(e => e.Address2Longitude).HasColumnName("address2_longitude");
            entity.Property(e => e.Address2Name).HasColumnName("address2_name");
            entity.Property(e => e.Address2Postalcode).HasColumnName("address2_postalcode");
            entity.Property(e => e.Address2Postofficebox).HasColumnName("address2_postofficebox");
            entity.Property(e => e.Address2Shippingmethodcode).HasColumnName("address2_shippingmethodcode");
            entity.Property(e => e.Address2Stateorprovince).HasColumnName("address2_stateorprovince");
            entity.Property(e => e.Address2Telephone1).HasColumnName("address2_telephone1");
            entity.Property(e => e.Address2Telephone2).HasColumnName("address2_telephone2");
            entity.Property(e => e.Address2Telephone3).HasColumnName("address2_telephone3");
            entity.Property(e => e.Address2Upszone).HasColumnName("address2_upszone");
            entity.Property(e => e.Address2Utcoffset).HasColumnName("address2_utcoffset");
            entity.Property(e => e.Applicationid).HasColumnName("applicationid");
            entity.Property(e => e.Applicationiduri).HasColumnName("applicationiduri");
            entity.Property(e => e.Azureactivedirectoryobjectid).HasColumnName("azureactivedirectoryobjectid");
            entity.Property(e => e.Azuredeletedon).HasColumnName("azuredeletedon");
            entity.Property(e => e.Azurestate).HasColumnName("azurestate");
            entity.Property(e => e.Businessunitid).HasColumnName("businessunitid");
            entity.Property(e => e.Businessunitidname).HasColumnName("businessunitidname");
            entity.Property(e => e.Calendarid).HasColumnName("calendarid");
            entity.Property(e => e.Caltype).HasColumnName("caltype");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Createdbyname).HasColumnName("createdbyname");
            entity.Property(e => e.Createdbyyominame).HasColumnName("createdbyyominame");
            entity.Property(e => e.Createdon).HasColumnName("createdon");
            entity.Property(e => e.Createdonbehalfby).HasColumnName("createdonbehalfby");
            entity.Property(e => e.Createdonbehalfbyname).HasColumnName("createdonbehalfbyname");
            entity.Property(e => e.Createdonbehalfbyyominame).HasColumnName("createdonbehalfbyyominame");
            entity.Property(e => e.Defaultfilterspopulated).HasColumnName("defaultfilterspopulated");
            entity.Property(e => e.Defaultmailbox).HasColumnName("defaultmailbox");
            entity.Property(e => e.Defaultmailboxname).HasColumnName("defaultmailboxname");
            entity.Property(e => e.Defaultodbfoldername).HasColumnName("defaultodbfoldername");
            entity.Property(e => e.Deletedstate).HasColumnName("deletedstate");
            entity.Property(e => e.Disabledreason).HasColumnName("disabledreason");
            entity.Property(e => e.Displayinserviceviews).HasColumnName("displayinserviceviews");
            entity.Property(e => e.Domainname).HasColumnName("domainname");
            entity.Property(e => e.Emailrouteraccessapproval).HasColumnName("emailrouteraccessapproval");
            entity.Property(e => e.Employeeid).HasColumnName("employeeid");
            entity.Property(e => e.EntityimageTimestamp).HasColumnName("entityimage_timestamp");
            entity.Property(e => e.EntityimageUrl).HasColumnName("entityimage_url");
            entity.Property(e => e.Entityimageid).HasColumnName("entityimageid");
            entity.Property(e => e.Exchangerate)
                .HasColumnType("decimal(38, 18)")
                .HasColumnName("exchangerate");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Fullname).HasColumnName("fullname");
            entity.Property(e => e.Governmentid).HasColumnName("governmentid");
            entity.Property(e => e.Homephone).HasColumnName("homephone");
            entity.Property(e => e.Identityid).HasColumnName("identityid");
            entity.Property(e => e.Importsequencenumber).HasColumnName("importsequencenumber");
            entity.Property(e => e.Incomingemaildeliverymethod).HasColumnName("incomingemaildeliverymethod");
            entity.Property(e => e.Internalemailaddress).HasColumnName("internalemailaddress");
            entity.Property(e => e.Invitestatuscode).HasColumnName("invitestatuscode");
            entity.Property(e => e.Isactivedirectoryuser).HasColumnName("isactivedirectoryuser");
            entity.Property(e => e.Isdisabled).HasColumnName("isdisabled");
            entity.Property(e => e.Isemailaddressapprovedbyo365admin)
                .HasColumnName("isemailaddressapprovedbyo365admin");
            entity.Property(e => e.Isintegrationuser).HasColumnName("isintegrationuser");
            entity.Property(e => e.Islicensed).HasColumnName("islicensed");
            entity.Property(e => e.Issyncwithdirectory).HasColumnName("issyncwithdirectory");
            entity.Property(e => e.Jobtitle).HasColumnName("jobtitle");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Latestupdatetime).HasColumnName("latestupdatetime");
            entity.Property(e => e.Middlename).HasColumnName("middlename");
            entity.Property(e => e.Mobilealertemail).HasColumnName("mobilealertemail");
            entity.Property(e => e.Mobileofflineprofileid).HasColumnName("mobileofflineprofileid");
            entity.Property(e => e.Mobileofflineprofileidname).HasColumnName("mobileofflineprofileidname");
            entity.Property(e => e.Mobilephone).HasColumnName("mobilephone");
            entity.Property(e => e.Modifiedby).HasColumnName("modifiedby");
            entity.Property(e => e.Modifiedbyname).HasColumnName("modifiedbyname");
            entity.Property(e => e.Modifiedbyyominame).HasColumnName("modifiedbyyominame");
            entity.Property(e => e.Modifiedon).HasColumnName("modifiedon");
            entity.Property(e => e.Modifiedonbehalfby).HasColumnName("modifiedonbehalfby");
            entity.Property(e => e.Modifiedonbehalfbyname).HasColumnName("modifiedonbehalfbyname");
            entity.Property(e => e.Modifiedonbehalfbyyominame).HasColumnName("modifiedonbehalfbyyominame");
            entity.Property(e => e.MsdynAgentType).HasColumnName("msdyn_agentType");
            entity.Property(e => e.MsdynBotapplicationid).HasColumnName("msdyn_botapplicationid");
            entity.Property(e => e.MsdynBotdescription).HasColumnName("msdyn_botdescription");
            entity.Property(e => e.MsdynBotendpoint).HasColumnName("msdyn_botendpoint");
            entity.Property(e => e.MsdynBothandle).HasColumnName("msdyn_bothandle");
            entity.Property(e => e.MsdynBotprovider).HasColumnName("msdyn_botprovider");
            entity.Property(e => e.MsdynBotsecretkeys).HasColumnName("msdyn_botsecretkeys");
            entity.Property(e => e.MsdynCapacity).HasColumnName("msdyn_capacity");
            entity.Property(e => e.MsdynDefaultpresenceiduser).HasColumnName("msdyn_defaultpresenceiduser");
            entity.Property(e => e.MsdynDefaultpresenceidusername).HasColumnName("msdyn_defaultpresenceidusername");
            entity.Property(e => e.MsdynGdproptout).HasColumnName("msdyn_gdproptout");
            entity.Property(e => e.MsdynGridwrappercontrolfield).HasColumnName("msdyn_gridwrappercontrolfield");
            entity.Property(e => e.MsdynIsexpertenabledforswarm).HasColumnName("msdyn_isexpertenabledforswarm");
            entity.Property(e => e.MsdynOwningenvironmentid).HasColumnName("msdyn_owningenvironmentid");
            entity.Property(e => e.MsdynUsertype).HasColumnName("msdyn_usertype");
            entity.Property(e => e.Nickname).HasColumnName("nickname");
            entity.Property(e => e.Organizationid).HasColumnName("organizationid");
            entity.Property(e => e.Organizationidname).HasColumnName("organizationidname");
            entity.Property(e => e.Outgoingemaildeliverymethod).HasColumnName("outgoingemaildeliverymethod");
            entity.Property(e => e.Overriddencreatedon).HasColumnName("overriddencreatedon");
            entity.Property(e => e.Parentsystemuserid).HasColumnName("parentsystemuserid");
            entity.Property(e => e.Parentsystemuseridname).HasColumnName("parentsystemuseridname");
            entity.Property(e => e.Parentsystemuseridyominame).HasColumnName("parentsystemuseridyominame");
            entity.Property(e => e.Passporthi).HasColumnName("passporthi");
            entity.Property(e => e.Passportlo).HasColumnName("passportlo");
            entity.Property(e => e.Personalemailaddress).HasColumnName("personalemailaddress");
            entity.Property(e => e.Photourl).HasColumnName("photourl");
            entity.Property(e => e.Positionid).HasColumnName("positionid");
            entity.Property(e => e.Positionidname).HasColumnName("positionidname");
            entity.Property(e => e.Preferredaddresscode).HasColumnName("preferredaddresscode");
            entity.Property(e => e.Preferredemailcode).HasColumnName("preferredemailcode");
            entity.Property(e => e.Preferredphonecode).HasColumnName("preferredphonecode");
            entity.Property(e => e.Processid).HasColumnName("processid");
            entity.Property(e => e.PtmPeruserlicensingdocumentscorepack)
                .HasColumnName("ptm_peruserlicensingdocumentscorepack");
            entity.Property(e => e.PtmPeruserlicensingdocumentscorepackserver)
                .HasColumnName("ptm_peruserlicensingdocumentscorepackserver");
            entity.Property(e => e.Queueid).HasColumnName("queueid");
            entity.Property(e => e.Queueidname).HasColumnName("queueidname");
            entity.Property(e => e.Salutation).HasColumnName("salutation");
            entity.Property(e => e.Setupuser).HasColumnName("setupuser");
            entity.Property(e => e.Sharepointemailaddress).HasColumnName("sharepointemailaddress");
            entity.Property(e => e.Siteid).HasColumnName("siteid");
            entity.Property(e => e.Siteidname).HasColumnName("siteidname");
            entity.Property(e => e.Skills).HasColumnName("skills");
            entity.Property(e => e.Stageid).HasColumnName("stageid");
            entity.Property(e => e.Systemuserid).HasColumnName("systemuserid");
            entity.Property(e => e.Territoryid).HasColumnName("territoryid");
            entity.Property(e => e.Territoryidname).HasColumnName("territoryidname");
            entity.Property(e => e.Timezoneruleversionnumber).HasColumnName("timezoneruleversionnumber");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.Transactioncurrencyid).HasColumnName("transactioncurrencyid");
            entity.Property(e => e.Transactioncurrencyidname).HasColumnName("transactioncurrencyidname");
            entity.Property(e => e.Traversedpath).HasColumnName("traversedpath");
            entity.Property(e => e.Userlicensetype).HasColumnName("userlicensetype");
            entity.Property(e => e.Userpuid).HasColumnName("userpuid");
            entity.Property(e => e.Utcconversiontimezonecode).HasColumnName("utcconversiontimezonecode");
            entity.Property(e => e.Versionnumber).HasColumnName("versionnumber");
            entity.Property(e => e.Windowsliveid).HasColumnName("windowsliveid");
            entity.Property(e => e.Yammeremailaddress).HasColumnName("yammeremailaddress");
            entity.Property(e => e.Yammeruserid).HasColumnName("yammeruserid");
            entity.Property(e => e.Yomifirstname).HasColumnName("yomifirstname");
            entity.Property(e => e.Yomifullname).HasColumnName("yomifullname");
            entity.Property(e => e.Yomilastname).HasColumnName("yomilastname");
            entity.Property(e => e.Yomimiddlename).HasColumnName("yomimiddlename");
        });
    }
}
