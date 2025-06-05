export class TestDataStore {
    public static readonly GetTrustSubpagesFor = (uid: number, pageName: string) => {

        const page = TestDataStore.GetAllTrustSubpagesForUid(uid).find(p => p.pageName == pageName);

        if (page === undefined)
            throw new Error(`Page ${pageName} is not in the Cypress test data. Is this a new page that needs adding to TestDataStore?`);

        return page.subpages;
    };

    public static readonly GetAllTrustSubpagesForUid = (uid: number) =>
        [
            {
                pageName: "Overview",
                subpages: [
                    { subpageName: "Trust details", url: `/trusts/overview/trust-details?uid=${uid}` },
                    { subpageName: "Trust summary", url: `/trusts/overview/trust-summary?uid=${uid}` },
                    { subpageName: "Reference numbers", url: `/trusts/overview/reference-numbers?uid=${uid}` },
                ]
            },
            {
                pageName: "Contacts",
                subpages: [
                    { subpageName: "In DfE", url: `/trusts/contacts/in-dfe?uid=${uid}` },
                    { subpageName: "In this trust", url: `/trusts/contacts/in-the-trust?uid=${uid}` },
                ]
            },
            {
                pageName: "Ofsted",
                subpages: [
                    { subpageName: "Single headline grades", url: `/trusts/ofsted/single-headline-grades?uid=${uid}` },
                    { subpageName: "Current ratings", url: `/trusts/ofsted/current-ratings?uid=${uid}` },
                    { subpageName: "Previous ratings", url: `/trusts/ofsted/previous-ratings?uid=${uid}` },
                    { subpageName: "Safeguarding and concerns", url: `/trusts/ofsted/safeguarding-and-concerns?uid=${uid}` }
                ]
            },
            {
                pageName: "Governance",
                subpages: [
                    { subpageName: `Trust leadership`, url: `/trusts/governance/trust-leadership?uid=${uid}` },
                    { subpageName: "Trustees", url: `/trusts/governance/trustees?uid=${uid}` },
                    { subpageName: "Members", url: `/trusts/governance/members?uid=${uid}` },
                    { subpageName: "Historic members", url: `/trusts/governance/historic-members?uid=${uid}` }
                ]
            },
        ];

    public static readonly GetAllSchoolSubpagesForUrn = (urn: number) =>
        [
            {
                pageName: "Overview",
                subpages: [
                    { subpageName: "School details", url: `/schools/overview/details?urn=${urn}` },
                    { subpageName: "SEN (special educational needs)", url: `/schools/overview/sen?urn=${urn}` },
                ]
            },
            {
                pageName: "Contacts",
                subpages: [
                    { subpageName: "In this school", url: `/schools/contacts/in-the-school?urn=${urn}` },
                ]
            },
        ];
}

export const testSchoolData = [
    {
        schoolName: "The Meadows Primary School",
        typeOfSchool: "Community school",
        urn: 123452
    },
    {
        schoolName: "Abbey Grange Church of England Academy",
        typeOfSchool: "Academy converter",
        urn: 137083
    }
];
export const testPreAdvisoryData = [
    {
        uid: 16002
    },
    {
        uid: 4921
    }
];
export const testPostAdvisoryData = [
    {
        uid: 17584
    },
    {
        uid: 16857
    }
];
export const testFreeSchoolsData = [
    {
        uid: 17538
    },
    {
        uid: 15786
    }
];
export const testTrustData = [
    {
        trustName: "Ashton West End Primary Academy",
        typeOfTrust: "single academy trust with contacts",
        uid: 5527
    },
    {
        trustName: "Aspire North East Multi Academy Trust",
        typeOfTrust: "multi academy trust with contacts",
        uid: 5712
    }
];
export const trustsWithGovernanceData = [
    {
        typeOfTrust: "single academy trust with governance data",
        uid: 5527
    },
    {
        typeOfTrust: "multi academy trust with governance data",
        uid: 5712
    }
];
