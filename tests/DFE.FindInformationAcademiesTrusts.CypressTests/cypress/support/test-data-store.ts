export class TestDataStore {
    public static readonly GetTrustSubpagesForUid = (uid: number) =>
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
                    { subpageName: "In the trust", url: `/trusts/contacts/in-the-trust?uid=${uid}` },
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
}
