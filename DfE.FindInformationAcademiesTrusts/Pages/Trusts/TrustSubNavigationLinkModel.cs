﻿using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Pages.Trusts;

public record TrustSubNavigationLinkModel(
    string LinkText,
    string Page,
    string Uid,
    string ServiceName,
    bool LinkIsActive)
{
    public string TestId => $"{ServiceName}-{LinkText}-subnav".Kebabify();
}
