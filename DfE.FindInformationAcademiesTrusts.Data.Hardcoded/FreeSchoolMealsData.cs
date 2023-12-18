using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.Hardcoded;

[ExcludeFromCodeCoverage]
public static class FreeSchoolMealsData
{
    public static Dictionary<int, FreeSchoolMealsAverage> Averages2022To23 { get; } = new();

    public static readonly DateTime LastUpdated = new(2023, 10, 2, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Data store for School statistics data taken from https://explore-education-statistics.service.gov.uk/data-tables/permalink/25bc8d0b-c700-4000-1b8a-08dbb99e3fd8
    /// </summary>
    static FreeSchoolMealsData()
    {
        PopulateLocalAuthorities();
        AddPercentagesByPhaseType();
    }

    private static void PopulateLocalAuthorities()
    {
        Averages2022To23.Add(334, new FreeSchoolMealsAverage(334, "Solihull", "E08000029"));
        Averages2022To23.Add(202, new FreeSchoolMealsAverage(202, "Camden", "E09000007"));
        Averages2022To23.Add(358, new FreeSchoolMealsAverage(358, "Trafford", "E08000009"));
        Averages2022To23.Add(856, new FreeSchoolMealsAverage(856, "Leicester", "E06000016"));
        Averages2022To23.Add(354, new FreeSchoolMealsAverage(354, "Rochdale", "E08000005"));
        Averages2022To23.Add(860, new FreeSchoolMealsAverage(860, "Staffordshire", "E10000028"));
        Averages2022To23.Add(839,
            new FreeSchoolMealsAverage(839, "Bournemouth, Christchurch and Poole", "E06000058"));
        Averages2022To23.Add(810, new FreeSchoolMealsAverage(810, "Kingston upon Hull, City of", "E06000010"));
        Averages2022To23.Add(936, new FreeSchoolMealsAverage(936, "Surrey", "E10000030"));
        Averages2022To23.Add(213, new FreeSchoolMealsAverage(213, "Westminster", "E09000033"));
        Averages2022To23.Add(303, new FreeSchoolMealsAverage(303, "Bexley", "E09000004"));
        Averages2022To23.Add(941, new FreeSchoolMealsAverage(941, "West Northamptonshire", "E06000062"));
        Averages2022To23.Add(908, new FreeSchoolMealsAverage(908, "Cornwall", "E06000052"));
        Averages2022To23.Add(359, new FreeSchoolMealsAverage(359, "Wigan", "E08000010"));
        Averages2022To23.Add(876, new FreeSchoolMealsAverage(876, "Halton", "E06000006"));
        Averages2022To23.Add(850, new FreeSchoolMealsAverage(850, "Hampshire", "E10000014"));
        Averages2022To23.Add(935, new FreeSchoolMealsAverage(935, "Suffolk", "E10000029"));
        Averages2022To23.Add(212, new FreeSchoolMealsAverage(212, "Wandsworth", "E09000032"));
        Averages2022To23.Add(909, new FreeSchoolMealsAverage(909, "Cumbria", "E10000006"));
        Averages2022To23.Add(332, new FreeSchoolMealsAverage(332, "Dudley", "E08000027"));
        Averages2022To23.Add(313, new FreeSchoolMealsAverage(313, "Hounslow", "E09000018"));
        Averages2022To23.Add(805, new FreeSchoolMealsAverage(805, "Hartlepool", "E06000001"));
        Averages2022To23.Add(319, new FreeSchoolMealsAverage(319, "Sutton", "E09000029"));
        Averages2022To23.Add(929, new FreeSchoolMealsAverage(929, "Northumberland", "E06000057"));
        Averages2022To23.Add(822, new FreeSchoolMealsAverage(822, "Bedford", "E06000055"));
        Averages2022To23.Add(320, new FreeSchoolMealsAverage(320, "Waltham Forest", "E09000031"));
        Averages2022To23.Add(211, new FreeSchoolMealsAverage(211, "Tower Hamlets", "E09000030"));
        Averages2022To23.Add(380, new FreeSchoolMealsAverage(380, "Bradford", "E08000032"));
        Averages2022To23.Add(204, new FreeSchoolMealsAverage(204, "Hackney", "E09000012"));
        Averages2022To23.Add(882, new FreeSchoolMealsAverage(882, "Southend-on-Sea", "E06000033"));
        Averages2022To23.Add(883, new FreeSchoolMealsAverage(883, "Thurrock", "E06000034"));
        Averages2022To23.Add(335, new FreeSchoolMealsAverage(335, "Walsall", "E08000030"));
        Averages2022To23.Add(868, new FreeSchoolMealsAverage(868, "Windsor and Maidenhead", "E06000040"));
        Averages2022To23.Add(311, new FreeSchoolMealsAverage(311, "Havering", "E09000016"));
        Averages2022To23.Add(393, new FreeSchoolMealsAverage(393, "South Tyneside", "E08000023"));
        Averages2022To23.Add(353, new FreeSchoolMealsAverage(353, "Oldham", "E08000004"));
        Averages2022To23.Add(926, new FreeSchoolMealsAverage(926, "Norfolk", "E10000020"));
        Averages2022To23.Add(357, new FreeSchoolMealsAverage(357, "Tameside", "E08000008"));
        Averages2022To23.Add(867, new FreeSchoolMealsAverage(867, "Bracknell Forest", "E06000036"));
        Averages2022To23.Add(330, new FreeSchoolMealsAverage(330, "Birmingham", "E08000025"));
        Averages2022To23.Add(342, new FreeSchoolMealsAverage(342, "St. Helens", "E08000013"));
        Averages2022To23.Add(893, new FreeSchoolMealsAverage(893, "Shropshire", "E06000051"));
        Averages2022To23.Add(304, new FreeSchoolMealsAverage(304, "Brent", "E09000005"));
        Averages2022To23.Add(861, new FreeSchoolMealsAverage(861, "Stoke-on-Trent", "E06000021"));
        Averages2022To23.Add(351, new FreeSchoolMealsAverage(351, "Bury", "E08000002"));
        Averages2022To23.Add(845, new FreeSchoolMealsAverage(845, "East Sussex", "E10000011"));
        Averages2022To23.Add(331, new FreeSchoolMealsAverage(331, "Coventry", "E08000026"));
        Averages2022To23.Add(821, new FreeSchoolMealsAverage(821, "Luton", "E06000032"));
        Averages2022To23.Add(203, new FreeSchoolMealsAverage(203, "Greenwich", "E09000011"));
        Averages2022To23.Add(382, new FreeSchoolMealsAverage(382, "Kirklees", "E08000034"));
        Averages2022To23.Add(879, new FreeSchoolMealsAverage(879, "Plymouth", "E06000026"));
        Averages2022To23.Add(813, new FreeSchoolMealsAverage(813, "North Lincolnshire", "E06000013"));
        Averages2022To23.Add(831, new FreeSchoolMealsAverage(831, "Derby", "E06000015"));
        Averages2022To23.Add(815, new FreeSchoolMealsAverage(815, "North Yorkshire", "E10000023"));
        Averages2022To23.Add(869, new FreeSchoolMealsAverage(869, "West Berkshire", "E06000037"));
        Averages2022To23.Add(838, new FreeSchoolMealsAverage(838, "Dorset", "E06000059"));
        Averages2022To23.Add(302, new FreeSchoolMealsAverage(302, "Barnet", "E09000003"));
        Averages2022To23.Add(874, new FreeSchoolMealsAverage(874, "Peterborough", "E06000031"));
        Averages2022To23.Add(207, new FreeSchoolMealsAverage(207, "Kensington and Chelsea", "E09000020"));
        Averages2022To23.Add(371, new FreeSchoolMealsAverage(371, "Doncaster", "E08000017"));
        Averages2022To23.Add(890, new FreeSchoolMealsAverage(890, "Blackpool", "E06000009"));
        Averages2022To23.Add(312, new FreeSchoolMealsAverage(312, "Hillingdon", "E09000017"));
        Averages2022To23.Add(840, new FreeSchoolMealsAverage(840, "County Durham", "E06000047"));
        Averages2022To23.Add(866, new FreeSchoolMealsAverage(866, "Swindon", "E06000030"));
        Averages2022To23.Add(878, new FreeSchoolMealsAverage(878, "Devon", "E10000008"));
        Averages2022To23.Add(336, new FreeSchoolMealsAverage(336, "Wolverhampton", "E08000031"));
        Averages2022To23.Add(841, new FreeSchoolMealsAverage(841, "Darlington", "E06000005"));
        Averages2022To23.Add(307, new FreeSchoolMealsAverage(307, "Ealing", "E09000009"));
        Averages2022To23.Add(308, new FreeSchoolMealsAverage(308, "Enfield", "E09000010"));
        Averages2022To23.Add(391, new FreeSchoolMealsAverage(391, "Newcastle upon Tyne", "E08000021"));
        Averages2022To23.Add(392, new FreeSchoolMealsAverage(392, "North Tyneside", "E08000022"));
        Averages2022To23.Add(811, new FreeSchoolMealsAverage(811, "East Riding of Yorkshire", "E06000011"));
        Averages2022To23.Add(812, new FreeSchoolMealsAverage(812, "North East Lincolnshire", "E06000012"));
        Averages2022To23.Add(807, new FreeSchoolMealsAverage(807, "Redcar and Cleveland", "E06000003"));
        Averages2022To23.Add(808, new FreeSchoolMealsAverage(808, "Stockton-on-Tees", "E06000004"));
        Averages2022To23.Add(823, new FreeSchoolMealsAverage(823, "Central Bedfordshire", "E06000056"));
        Averages2022To23.Add(873, new FreeSchoolMealsAverage(873, "Cambridgeshire", "E10000003"));
        Averages2022To23.Add(208, new FreeSchoolMealsAverage(208, "Lambeth", "E09000022"));
        Averages2022To23.Add(305, new FreeSchoolMealsAverage(305, "Bromley", "E09000006"));
        Averages2022To23.Add(938, new FreeSchoolMealsAverage(938, "West Sussex", "E10000032"));
        Averages2022To23.Add(373, new FreeSchoolMealsAverage(373, "Sheffield", "E08000019"));
        Averages2022To23.Add(352, new FreeSchoolMealsAverage(352, "Manchester", "E08000003"));
        Averages2022To23.Add(895, new FreeSchoolMealsAverage(895, "Cheshire East", "E06000049"));
        Averages2022To23.Add(896, new FreeSchoolMealsAverage(896, "Cheshire West and Chester", "E06000050"));
        Averages2022To23.Add(870, new FreeSchoolMealsAverage(870, "Reading", "E06000038"));
        Averages2022To23.Add(806, new FreeSchoolMealsAverage(806, "Middlesbrough", "E06000002"));
        Averages2022To23.Add(892, new FreeSchoolMealsAverage(892, "Nottingham", "E06000018"));
        Averages2022To23.Add(825, new FreeSchoolMealsAverage(825, "Buckinghamshire", "E06000060"));
        Averages2022To23.Add(826, new FreeSchoolMealsAverage(826, "Milton Keynes", "E06000042"));
        Averages2022To23.Add(889, new FreeSchoolMealsAverage(889, "Blackburn with Darwen", "E06000008"));
        Averages2022To23.Add(210, new FreeSchoolMealsAverage(210, "Southwark", "E09000028"));
        Averages2022To23.Add(310, new FreeSchoolMealsAverage(310, "Harrow", "E09000015"));
        Averages2022To23.Add(816, new FreeSchoolMealsAverage(816, "York", "E06000014"));
        Averages2022To23.Add(871, new FreeSchoolMealsAverage(871, "Slough", "E06000039"));
        Averages2022To23.Add(317, new FreeSchoolMealsAverage(317, "Redbridge", "E09000026"));
        Averages2022To23.Add(314, new FreeSchoolMealsAverage(314, "Kingston upon Thames", "E09000021"));
        Averages2022To23.Add(872, new FreeSchoolMealsAverage(872, "Wokingham", "E06000041"));
        Averages2022To23.Add(885, new FreeSchoolMealsAverage(885, "Worcestershire", "E10000034"));
        Averages2022To23.Add(383, new FreeSchoolMealsAverage(383, "Leeds", "E08000035"));
        Averages2022To23.Add(384, new FreeSchoolMealsAverage(384, "Wakefield", "E08000036"));
        Averages2022To23.Add(372, new FreeSchoolMealsAverage(372, "Rotherham", "E08000018"));
        Averages2022To23.Add(206, new FreeSchoolMealsAverage(206, "Islington", "E09000019"));
        Averages2022To23.Add(309, new FreeSchoolMealsAverage(309, "Haringey", "E09000014"));
        Averages2022To23.Add(855, new FreeSchoolMealsAverage(855, "Leicestershire", "E10000018"));
        Averages2022To23.Add(340, new FreeSchoolMealsAverage(340, "Knowsley", "E08000011"));
        Averages2022To23.Add(894, new FreeSchoolMealsAverage(894, "Telford and Wrekin", "E06000020"));
        Averages2022To23.Add(803, new FreeSchoolMealsAverage(803, "South Gloucestershire", "E06000025"));
        Averages2022To23.Add(880, new FreeSchoolMealsAverage(880, "Torbay", "E06000027"));
        Averages2022To23.Add(884, new FreeSchoolMealsAverage(884, "Herefordshire, County of", "E06000019"));
        Averages2022To23.Add(355, new FreeSchoolMealsAverage(355, "Salford", "E08000006"));
        Averages2022To23.Add(356, new FreeSchoolMealsAverage(356, "Stockport", "E08000007"));
        Averages2022To23.Add(888, new FreeSchoolMealsAverage(888, "Lancashire", "E10000017"));
        Averages2022To23.Add(881, new FreeSchoolMealsAverage(881, "Essex", "E10000012"));
        Averages2022To23.Add(316, new FreeSchoolMealsAverage(316, "Newham", "E09000025"));
        Averages2022To23.Add(887, new FreeSchoolMealsAverage(887, "Medway", "E06000035"));
        Averages2022To23.Add(886, new FreeSchoolMealsAverage(886, "Kent", "E10000016"));
        Averages2022To23.Add(925, new FreeSchoolMealsAverage(925, "Lincolnshire", "E10000019"));
        Averages2022To23.Add(381, new FreeSchoolMealsAverage(381, "Calderdale", "E08000033"));
        Averages2022To23.Add(301, new FreeSchoolMealsAverage(301, "Barking and Dagenham", "E09000002"));
        Averages2022To23.Add(801, new FreeSchoolMealsAverage(801, "Bristol, City of", "E06000023"));
        Averages2022To23.Add(802, new FreeSchoolMealsAverage(802, "North Somerset", "E06000024"));
        Averages2022To23.Add(394, new FreeSchoolMealsAverage(394, "Sunderland", "E08000024"));
        Averages2022To23.Add(350, new FreeSchoolMealsAverage(350, "Bolton", "E08000001"));
        Averages2022To23.Add(877, new FreeSchoolMealsAverage(877, "Warrington", "E06000007"));
        Averages2022To23.Add(852, new FreeSchoolMealsAverage(852, "Southampton", "E06000045"));
        Averages2022To23.Add(916, new FreeSchoolMealsAverage(916, "Gloucestershire", "E10000013"));
        Averages2022To23.Add(933, new FreeSchoolMealsAverage(933, "Somerset", "E10000027"));
        Averages2022To23.Add(315, new FreeSchoolMealsAverage(315, "Merton", "E09000024"));
        Averages2022To23.Add(209, new FreeSchoolMealsAverage(209, "Lewisham", "E09000023"));
        Averages2022To23.Add(333, new FreeSchoolMealsAverage(333, "Sandwell", "E08000028"));
        Averages2022To23.Add(931, new FreeSchoolMealsAverage(931, "Oxfordshire", "E10000025"));
        Averages2022To23.Add(390, new FreeSchoolMealsAverage(390, "Gateshead", "E08000037"));
        Averages2022To23.Add(341, new FreeSchoolMealsAverage(341, "Liverpool", "E08000012"));
        Averages2022To23.Add(919, new FreeSchoolMealsAverage(919, "Hertfordshire", "E10000015"));
        Averages2022To23.Add(343, new FreeSchoolMealsAverage(343, "Sefton", "E08000014"));
        Averages2022To23.Add(-1, new FreeSchoolMealsAverage(-1, "National"));
        Averages2022To23.Add(846, new FreeSchoolMealsAverage(846, "Brighton and Hove", "E06000043"));
        Averages2022To23.Add(205, new FreeSchoolMealsAverage(205, "Hammersmith and Fulham", "E09000013"));
        Averages2022To23.Add(306, new FreeSchoolMealsAverage(306, "Croydon", "E09000008"));
        Averages2022To23.Add(921, new FreeSchoolMealsAverage(921, "Isle of Wight", "E06000046"));
        Averages2022To23.Add(370, new FreeSchoolMealsAverage(370, "Barnsley", "E08000016"));
        Averages2022To23.Add(830, new FreeSchoolMealsAverage(830, "Derbyshire", "E10000007"));
        Averages2022To23.Add(201, new FreeSchoolMealsAverage(201, "City of London", "E09000001"));
        Averages2022To23.Add(851, new FreeSchoolMealsAverage(851, "Portsmouth", "E06000044"));
        Averages2022To23.Add(891, new FreeSchoolMealsAverage(891, "Nottinghamshire", "E10000024"));
        Averages2022To23.Add(857, new FreeSchoolMealsAverage(857, "Rutland", "E06000017"));
        Averages2022To23.Add(800, new FreeSchoolMealsAverage(800, "Bath and North East Somerset", "E06000022"));
        Averages2022To23.Add(937, new FreeSchoolMealsAverage(937, "Warwickshire", "E10000031"));
        Averages2022To23.Add(344, new FreeSchoolMealsAverage(344, "Wirral", "E08000015"));
        Averages2022To23.Add(865, new FreeSchoolMealsAverage(865, "Wiltshire", "E06000054"));
        Averages2022To23.Add(940, new FreeSchoolMealsAverage(940, "North Northamptonshire", "E06000061"));
        Averages2022To23.Add(318, new FreeSchoolMealsAverage(318, "Richmond upon Thames", "E09000027"));
        Averages2022To23.Add(420, new FreeSchoolMealsAverage(420, "Isles of Scilly", "E06000053"));
    }

    /// <summary>
    /// Hard-coding percentage of pupils eligible for free school meals per local authority by phase
    /// Does not include phase group types which are not present in Academies db Gias.Establishment table
    /// </summary>
    private static void AddPercentagesByPhaseType()
    {
        Averages2022To23[334].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.52941176);
        Averages2022To23[202].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.45945946);
        Averages2022To23[358].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 56.41025641);
        Averages2022To23[856].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 70.45454545);
        Averages2022To23[354].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.3495935);
        Averages2022To23[860].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 50.51020408);
        Averages2022To23[839].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54);
        Averages2022To23[810].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.06214689);
        Averages2022To23[936].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.16504854);
        Averages2022To23[213].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 72.72727273);
        Averages2022To23[303].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.66666667);
        Averages2022To23[941].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.07407407);
        Averages2022To23[908].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.63636364);
        Averages2022To23[359].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.2244898);
        Averages2022To23[876].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 70.27027027);
        Averages2022To23[850].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.54237288);
        Averages2022To23[935].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.9122807);
        Averages2022To23[212].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60.6557377);
        Averages2022To23[909].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.35714286);
        Averages2022To23[332].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 94.44444444);
        Averages2022To23[313].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.36633663);
        Averages2022To23[805].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.66666667);
        Averages2022To23[319].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.63302752);
        Averages2022To23[929].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 76.19047619);
        Averages2022To23[822].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.5);
        Averages2022To23[320].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 40.90909091);
        Averages2022To23[211].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.46268657);
        Averages2022To23[380].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 73.52941176);
        Averages2022To23[204].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 50);
        Averages2022To23[882].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.79310345);
        Averages2022To23[883].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 42.85714286);
        Averages2022To23[335].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.91176471);
        Averages2022To23[868].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52);
        Averages2022To23[311].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.54545455);
        Averages2022To23[393].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 71.42857143);
        Averages2022To23[353].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.14285714);
        Averages2022To23[926].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.1025641);
        Averages2022To23[357].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60.83916084);
        Averages2022To23[867].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 50);
        Averages2022To23[330].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 71.03064067);
        Averages2022To23[342].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.31914894);
        Averages2022To23[893].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.64912281);
        Averages2022To23[304].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 44.44444444);
        Averages2022To23[861].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 50);
        Averages2022To23[351].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 62.99212598);
        Averages2022To23[845].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.70212766);
        Averages2022To23[331].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.75609756);
        Averages2022To23[821].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 48.33333333);
        Averages2022To23[203].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 23.4939759);
        Averages2022To23[382].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 70.83333333);
        Averages2022To23[879].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 53.39366516);
        Averages2022To23[813].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 28.57142857);
        Averages2022To23[831].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.93442623);
        Averages2022To23[815].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 46.66666667);
        Averages2022To23[869].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 41.81818182);
        Averages2022To23[838].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.11570248);
        Averages2022To23[302].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.09090909);
        Averages2022To23[874].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.32653061);
        Averages2022To23[207].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 53.19148936);
        Averages2022To23[371].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 68.18181818);
        Averages2022To23[890].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 67.10526316);
        Averages2022To23[312].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 27.27272727);
        Averages2022To23[840].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.10169492);
        Averages2022To23[866].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 53.96825397);
        Averages2022To23[878].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 48.17518248);
        Averages2022To23[336].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.6875);
        Averages2022To23[841].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.66666667);
        Averages2022To23[307].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 43.33333333);
        Averages2022To23[308].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 44.18604651);
        Averages2022To23[391].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.57731959);
        Averages2022To23[392].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.67664671);
        Averages2022To23[811].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.93442623);
        Averages2022To23[812].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 72.22222222);
        Averages2022To23[807].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 80.88235294);
        Averages2022To23[808].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 71.18644068);
        Averages2022To23[823].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.12903226);
        Averages2022To23[873].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 33.33333333);
        Averages2022To23[208].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 45.76271186);
        Averages2022To23[305].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.63636364);
        Averages2022To23[938].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 51.4084507);
        Averages2022To23[373].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 78.89447236);
        Averages2022To23[352].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 79.05982906);
        Averages2022To23[895].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.53846154);
        Averages2022To23[896].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55);
        Averages2022To23[870].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.45945946);
        Averages2022To23[806].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 46.66666667);
        Averages2022To23[892].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.06756757);
        Averages2022To23[825].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 44.04761905);
        Averages2022To23[826].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 46.875);
        Averages2022To23[889].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 67.08860759);
        Averages2022To23[210].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.08333333);
        Averages2022To23[310].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 48.64864865);
        Averages2022To23[816].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.23880597);
        Averages2022To23[871].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 47.44525547);
        Averages2022To23[317].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 39.75903614);
        Averages2022To23[314].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 47.36842105);
        Averages2022To23[872].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.14285714);
        Averages2022To23[885].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 56.73758865);
        Averages2022To23[383].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 100);
        Averages2022To23[384].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.32994924);
        Averages2022To23[372].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.83516484);
        Averages2022To23[206].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.29213483);
        Averages2022To23[309].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 58.49056604);
        Averages2022To23[855].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 100);
        Averages2022To23[340].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 88);
        Averages2022To23[894].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 69.6969697);
        Averages2022To23[803].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 49.29577465);
        Averages2022To23[880].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60.46511628);
        Averages2022To23[884].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 50.98039216);
        Averages2022To23[355].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 71.83098592);
        Averages2022To23[356].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 65.17857143);
        Averages2022To23[888].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.2920354);
        Averages2022To23[881].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 46.46017699);
        Averages2022To23[316].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 37.26708075);
        Averages2022To23[887].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 49.23076923);
        Averages2022To23[886].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 14.28571429);
        Averages2022To23[925].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.77011494);
        Averages2022To23[381].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 73.68421053);
        Averages2022To23[301].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 28.98550725);
        Averages2022To23[801].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 56.73758865);
        Averages2022To23[802].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.63636364);
        Averages2022To23[394].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 75.51020408);
        Averages2022To23[350].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.16504854);
        Averages2022To23[877].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.55555556);
        Averages2022To23[852].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 79.06976744);
        Averages2022To23[916].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.8);
        Averages2022To23[933].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 62.87878788);
        Averages2022To23[315].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 24.24242424);
        Averages2022To23[209].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 46.66666667);
        Averages2022To23[333].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.03614458);
        Averages2022To23[931].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 62.5);
        Averages2022To23[390].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.55555556);
        Averages2022To23[341].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 27.86377709);
        Averages2022To23[919].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 37.77777778);
        Averages2022To23[343].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.42028986);
        Averages2022To23[-1].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.78940186);
        Averages2022To23[846].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 73.86363636);
        Averages2022To23[205].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 62.90322581);
        Averages2022To23[306].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 46.06741573);
        Averages2022To23[921].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 53.84615385);
        Averages2022To23[370].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 67.85714286);
        Averages2022To23[830].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 67.55555556);
        Averages2022To23[334].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.38137452);
        Averages2022To23[372].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.26287001);
        Averages2022To23[885].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.33358434);
        Averages2022To23[201].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.08118081);
        Averages2022To23[358].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.36216734);
        Averages2022To23[354].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.49967227);
        Averages2022To23[860].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.87423826);
        Averages2022To23[839].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.16339625);
        Averages2022To23[810].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.16687638);
        Averages2022To23[850].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.34419412);
        Averages2022To23[851].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.27061775);
        Averages2022To23[213].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 37.18562874);
        Averages2022To23[303].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.291146);
        Averages2022To23[941].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.73721475);
        Averages2022To23[908].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.80772737);
        Averages2022To23[341].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.89295451);
        Averages2022To23[-1].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.99569177);
        Averages2022To23[876].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 37.91144201);
        Averages2022To23[935].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.44629254);
        Averages2022To23[212].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.0734943);
        Averages2022To23[909].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.08463599);
        Averages2022To23[381].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.71686013);
        Averages2022To23[332].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.58804636);
        Averages2022To23[350].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.72310127);
        Averages2022To23[805].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 35.61735261);
        Averages2022To23[891].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.60948621);
        Averages2022To23[319].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.83225267);
        Averages2022To23[929].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.28610231);
        Averages2022To23[306].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.72287509);
        Averages2022To23[320].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.4846328);
        Averages2022To23[883].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.97919445);
        Averages2022To23[210].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 35.37725097);
        Averages2022To23[380].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.0442018);
        Averages2022To23[857].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 10.41592606);
        Averages2022To23[882].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.62658228);
        Averages2022To23[335].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.78429162);
        Averages2022To23[373].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.01643639);
        Averages2022To23[868].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 13.51403492);
        Averages2022To23[311].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.4689572);
        Averages2022To23[393].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 32.93887147);
        Averages2022To23[384].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.22729368);
        Averages2022To23[353].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.7489894);
        Averages2022To23[359].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.70225828);
        Averages2022To23[356].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.61504659);
        Averages2022To23[357].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.97555403);
        Averages2022To23[867].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 11.92322606);
        Averages2022To23[938].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.43792256);
        Averages2022To23[800].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 17.74811975);
        Averages2022To23[391].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 39.9677574);
        Averages2022To23[342].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.39139397);
        Averages2022To23[888].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.05005821);
        Averages2022To23[893].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.03945584);
        Averages2022To23[317].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.29708315);
        Averages2022To23[826].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.92822827);
        Averages2022To23[889].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.26219192);
        Averages2022To23[845].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.2348285);
        Averages2022To23[330].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 39.61376467);
        Averages2022To23[937].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.51311209);
        Averages2022To23[821].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.38544591);
        Averages2022To23[203].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.87645761);
        Averages2022To23[382].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.73140987);
        Averages2022To23[879].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.93513383);
        Averages2022To23[813].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.35670622);
        Averages2022To23[873].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.24094548);
        Averages2022To23[869].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.63900783);
        Averages2022To23[838].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.69982108);
        Averages2022To23[202].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 41.82142187);
        Averages2022To23[874].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.38224119);
        Averages2022To23[207].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 32.63960135);
        Averages2022To23[371].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.97104008);
        Averages2022To23[211].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 36.6463318);
        Averages2022To23[803].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.04733526);
        Averages2022To23[840].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 32.23746973);
        Averages2022To23[866].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.48405499);
        Averages2022To23[878].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.78656852);
        Averages2022To23[336].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 39.02411539);
        Averages2022To23[841].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.56178258);
        Averages2022To23[856].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.57321265);
        Averages2022To23[307].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.57161651);
        Averages2022To23[308].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.21348315);
        Averages2022To23[392].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.34379809);
        Averages2022To23[343].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.63518175);
        Averages2022To23[811].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.18662377);
        Averages2022To23[807].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.69876622);
        Averages2022To23[808].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.81706872);
        Averages2022To23[823].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 12.05454241);
        Averages2022To23[304].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.04042407);
        Averages2022To23[936].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.40815531);
        Averages2022To23[890].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 38.67730314);
        Averages2022To23[344].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.18157424);
        Averages2022To23[352].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 42.18689982);
        Averages2022To23[895].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.80283746);
        Averages2022To23[896].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.00047955);
        Averages2022To23[870].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.76428471);
        Averages2022To23[925].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.01659115);
        Averages2022To23[351].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.57486693);
        Averages2022To23[892].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 35.87401575);
        Averages2022To23[894].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.15737498);
        Averages2022To23[825].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.26887348);
        Averages2022To23[209].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.5918144);
        Averages2022To23[310].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.84634646);
        Averages2022To23[816].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.87657176);
        Averages2022To23[871].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.6840896);
        Averages2022To23[301].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.7426953);
        Averages2022To23[313].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.05395337);
        Averages2022To23[872].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 9.254467036);
        Averages2022To23[383].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.30904973);
        Averages2022To23[802].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.04720803);
        Averages2022To23[208].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 37.10252752);
        Averages2022To23[822].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.43220841);
        Averages2022To23[206].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 41.59765482);
        Averages2022To23[309].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.2593922);
        Averages2022To23[815].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 17.63115197);
        Averages2022To23[831].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.16458772);
        Averages2022To23[340].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 36.24916276);
        Averages2022To23[204].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 37.94902451);
        Averages2022To23[880].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.69365217);
        Averages2022To23[884].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.35401219);
        Averages2022To23[926].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.82055967);
        Averages2022To23[355].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.00036996);
        Averages2022To23[855].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.32754912);
        Averages2022To23[316].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.73097335);
        Averages2022To23[806].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 38.09702896);
        Averages2022To23[865].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.18125504);
        Averages2022To23[940].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.65128474);
        Averages2022To23[886].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.87715902);
        Averages2022To23[318].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 12.13391627);
        Averages2022To23[861].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 38.42585122);
        Averages2022To23[887].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.19944567);
        Averages2022To23[801].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.28615213);
        Averages2022To23[394].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.82770241);
        Averages2022To23[852].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.27862967);
        Averages2022To23[916].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.65336658);
        Averages2022To23[933].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.34462211);
        Averages2022To23[314].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.53741351);
        Averages2022To23[315].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.49084465);
        Averages2022To23[881].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.74160585);
        Averages2022To23[333].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.82145282);
        Averages2022To23[302].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.13936375);
        Averages2022To23[931].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.6483453);
        Averages2022To23[390].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.18622819);
        Averages2022To23[919].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.23452916);
        Averages2022To23[312].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.14847845);
        Averages2022To23[846].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.08158666);
        Averages2022To23[205].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.80802661);
        Averages2022To23[305].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.39153268);
        Averages2022To23[331].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.15925463);
        Averages2022To23[921].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.67398765);
        Averages2022To23[370].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.16470033);
        Averages2022To23[812].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 31.19932905);
        Averages2022To23[877].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.30226365);
        Averages2022To23[830].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.29019262);
        Averages2022To23[885].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.71397128);
        Averages2022To23[358].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.14826818);
        Averages2022To23[354].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.71536949);
        Averages2022To23[334].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.60313101);
        Averages2022To23[810].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.04638489);
        Averages2022To23[850].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.36301554);
        Averages2022To23[851].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.3662738);
        Averages2022To23[213].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 37.44991751);
        Averages2022To23[830].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.62700602);
        Averages2022To23[886].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.20476682);
        Averages2022To23[303].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.32784798);
        Averages2022To23[941].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.72008443);
        Averages2022To23[880].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.92998166);
        Averages2022To23[908].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.50447607);
        Averages2022To23[341].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.58163572);
        Averages2022To23[-1].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.69174097);
        Averages2022To23[316].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 35.88520055);
        Averages2022To23[876].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 38.06380638);
        Averages2022To23[212].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.88430871);
        Averages2022To23[896].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.78219706);
        Averages2022To23[909].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.26413583);
        Averages2022To23[381].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.8491761);
        Averages2022To23[350].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.93806446);
        Averages2022To23[312].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.46837682);
        Averages2022To23[394].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.7627341);
        Averages2022To23[891].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.326069);
        Averages2022To23[319].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.15546574);
        Averages2022To23[929].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.62056176);
        Averages2022To23[821].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.01003764);
        Averages2022To23[306].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 30.1636244);
        Averages2022To23[883].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.20237128);
        Averages2022To23[210].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 35.1995114);
        Averages2022To23[373].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.82446348);
        Averages2022To23[380].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.89661183);
        Averages2022To23[203].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.32035101);
        Averages2022To23[857].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 12.81973203);
        Averages2022To23[882].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.48456249);
        Averages2022To23[335].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.63620387);
        Averages2022To23[868].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 12.54142238);
        Averages2022To23[311].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.31360528);
        Averages2022To23[393].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.12307194);
        Averages2022To23[384].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.10793621);
        Averages2022To23[353].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 30.79308094);
        Averages2022To23[356].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.92359258);
        Averages2022To23[852].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 33.84500745);
        Averages2022To23[938].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.10733629);
        Averages2022To23[420].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 3.041825095);
        Averages2022To23[800].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.61756797);
        Averages2022To23[391].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 38.11513719);
        Averages2022To23[342].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.07296137);
        Averages2022To23[888].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.22544871);
        Averages2022To23[893].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.22349536);
        Averages2022To23[359].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.09020684);
        Averages2022To23[317].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.59245886);
        Averages2022To23[860].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.56039967);
        Averages2022To23[826].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.54510116);
        Averages2022To23[889].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.34281005);
        Averages2022To23[845].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.53020599);
        Averages2022To23[330].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 36.48543601);
        Averages2022To23[937].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.22165928);
        Averages2022To23[879].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.74871736);
        Averages2022To23[812].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.51954831);
        Averages2022To23[873].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.60946262);
        Averages2022To23[869].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 13.07504143);
        Averages2022To23[202].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 39.67622834);
        Averages2022To23[874].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.84315513);
        Averages2022To23[207].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.85645395);
        Averages2022To23[332].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.50111967);
        Averages2022To23[371].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.37806805);
        Averages2022To23[211].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 42.38501371);
        Averages2022To23[803].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.8677686);
        Averages2022To23[840].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.64719269);
        Averages2022To23[866].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.52763683);
        Averages2022To23[878].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.95471147);
        Averages2022To23[336].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 37.71574083);
        Averages2022To23[841].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.00740302);
        Averages2022To23[856].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.63798809);
        Averages2022To23[838].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.49013799);
        Averages2022To23[307].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.49011122);
        Averages2022To23[308].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.83894559);
        Averages2022To23[390].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.36541304);
        Averages2022To23[392].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.52472722);
        Averages2022To23[811].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.48922015);
        Averages2022To23[807].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.06682721);
        Averages2022To23[808].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.51569903);
        Averages2022To23[823].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 13.37128455);
        Averages2022To23[320].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.15887405);
        Averages2022To23[304].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.26499455);
        Averages2022To23[318].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 13.91348343);
        Averages2022To23[936].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 12.11020645);
        Averages2022To23[890].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 43.86125654);
        Averages2022To23[344].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.14179501);
        Averages2022To23[372].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.49050385);
        Averages2022To23[352].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 42.39984453);
        Averages2022To23[895].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.72360598);
        Averages2022To23[925].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.52559035);
        Averages2022To23[343].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.2408098);
        Averages2022To23[351].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.74328726);
        Averages2022To23[892].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 37.14522478);
        Averages2022To23[894].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.12991354);
        Averages2022To23[825].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 11.20111196);
        Averages2022To23[805].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 38.80315501);
        Averages2022To23[209].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.86120847);
        Averages2022To23[310].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.65679871);
        Averages2022To23[816].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.26057887);
        Averages2022To23[870].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.74945601);
        Averages2022To23[865].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.31258768);
        Averages2022To23[301].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.53452258);
        Averages2022To23[313].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.41236526);
        Averages2022To23[871].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.04612428);
        Averages2022To23[872].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 9.314478943);
        Averages2022To23[383].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.14528381);
        Averages2022To23[813].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.8007899);
        Averages2022To23[205].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.4082039);
        Averages2022To23[206].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 44.24579809);
        Averages2022To23[309].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.94621207);
        Averages2022To23[815].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.10264323);
        Averages2022To23[831].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.30335896);
        Averages2022To23[802].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.88587732);
        Averages2022To23[340].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 48.53245856);
        Averages2022To23[357].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 28.67293767);
        Averages2022To23[822].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.35203305);
        Averages2022To23[204].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 42.62740042);
        Averages2022To23[382].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.89783456);
        Averages2022To23[884].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.00471215);
        Averages2022To23[926].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.07594317);
        Averages2022To23[855].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.92415851);
        Averages2022To23[806].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 41.74597965);
        Averages2022To23[839].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 16.9647893);
        Averages2022To23[940].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.323482);
        Averages2022To23[935].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.63937087);
        Averages2022To23[355].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 34.48013396);
        Averages2022To23[861].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.99300699);
        Averages2022To23[887].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.80207092);
        Averages2022To23[801].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 28.52979921);
        Averages2022To23[867].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 12.56632899);
        Averages2022To23[916].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.27767944);
        Averages2022To23[933].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.71261787);
        Averages2022To23[314].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 12.48211162);
        Averages2022To23[315].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.10837438);
        Averages2022To23[881].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 16.87018876);
        Averages2022To23[208].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 36.23602642);
        Averages2022To23[333].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.19643647);
        Averages2022To23[302].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.46335079);
        Averages2022To23[931].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.04952961);
        Averages2022To23[919].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 12.82789814);
        Averages2022To23[846].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.45614577);
        Averages2022To23[305].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.0785554);
        Averages2022To23[331].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.92920918);
        Averages2022To23[921].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.83474853);
        Averages2022To23[370].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.82570228);
        Averages2022To23[877].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.39615881);
        Averages2022To23[885].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.15566038);
        Averages2022To23[935].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.42287234);
        Averages2022To23[358].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.87677725);
        Averages2022To23[850].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.32038835);
        Averages2022To23[851].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.97301349);
        Averages2022To23[212].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.856018);
        Averages2022To23[213].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 61.38211382);
        Averages2022To23[800].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.28209765);
        Averages2022To23[886].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.68044988);
        Averages2022To23[941].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.23781009);
        Averages2022To23[880].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.66893039);
        Averages2022To23[908].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.28205128);
        Averages2022To23[341].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.16814159);
        Averages2022To23[-1].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.98689461);
        Averages2022To23[211].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.42708333);
        Averages2022To23[896].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.21338156);
        Averages2022To23[909].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.04109589);
        Averages2022To23[381].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.30434783);
        Averages2022To23[350].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.13018322);
        Averages2022To23[312].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.30377166);
        Averages2022To23[394].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.96342738);
        Averages2022To23[857].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 12.5);
        Averages2022To23[891].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.99165275);
        Averages2022To23[319].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.34525661);
        Averages2022To23[821].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 35.58673469);
        Averages2022To23[306].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.30368098);
        Averages2022To23[883].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 32.32931727);
        Averages2022To23[210].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.90023753);
        Averages2022To23[373].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.17479417);
        Averages2022To23[203].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.06382979);
        Averages2022To23[303].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.66304348);
        Averages2022To23[882].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.31782946);
        Averages2022To23[334].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.15984148);
        Averages2022To23[335].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.35294118);
        Averages2022To23[867].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 35.85858586);
        Averages2022To23[310].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 35.77075099);
        Averages2022To23[855].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 35.03733487);
        Averages2022To23[316].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.50877193);
        Averages2022To23[353].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.38415546);
        Averages2022To23[919].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.18149466);
        Averages2022To23[356].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.56987788);
        Averages2022To23[852].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.27963176);
        Averages2022To23[938].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 36.5901319);
        Averages2022To23[933].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.46116028);
        Averages2022To23[342].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 50.87719298);
        Averages2022To23[359].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.58848614);
        Averages2022To23[317].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.96018377);
        Averages2022To23[893].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.51696607);
        Averages2022To23[384].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.92307692);
        Averages2022To23[860].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.62068966);
        Averages2022To23[826].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.4707828);
        Averages2022To23[330].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.98097252);
        Averages2022To23[202].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.88995215);
        Averages2022To23[921].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.5049505);
        Averages2022To23[879].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.84667571);
        Averages2022To23[812].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.57983193);
        Averages2022To23[830].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.01324503);
        Averages2022To23[813].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.07492795);
        Averages2022To23[371].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.23809524);
        Averages2022To23[889].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.81081081);
        Averages2022To23[868].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 26.63316583);
        Averages2022To23[311].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.46153846);
        Averages2022To23[803].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 30.49403748);
        Averages2022To23[352].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.84754522);
        Averages2022To23[866].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.16546763);
        Averages2022To23[878].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.13580247);
        Averages2022To23[336].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.43564356);
        Averages2022To23[841].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.39233038);
        Averages2022To23[856].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.91075515);
        Averages2022To23[838].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.80188679);
        Averages2022To23[307].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.90010515);
        Averages2022To23[390].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.64864865);
        Averages2022To23[392].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.32075472);
        Averages2022To23[811].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.46153846);
        Averages2022To23[807].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.10526316);
        Averages2022To23[808].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.6941896);
        Averages2022To23[823].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 36.24401914);
        Averages2022To23[207].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.71523179);
        Averages2022To23[304].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.86602871);
        Averages2022To23[318].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 32.63473054);
        Averages2022To23[810].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.28954424);
        Averages2022To23[936].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 35.12895854);
        Averages2022To23[890].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 61.61790017);
        Averages2022To23[343].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.89873418);
        Averages2022To23[372].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.86175115);
        Averages2022To23[895].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.05996759);
        Averages2022To23[925].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.07321594);
        Averages2022To23[805].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.57627119);
        Averages2022To23[351].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.58577406);
        Averages2022To23[873].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.54102259);
        Averages2022To23[894].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.35336195);
        Averages2022To23[825].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 34.96420048);
        Averages2022To23[888].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.4057971);
        Averages2022To23[209].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.47368421);
        Averages2022To23[815].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.58219178);
        Averages2022To23[816].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 31.08974359);
        Averages2022To23[870].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.64723032);
        Averages2022To23[865].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.22137405);
        Averages2022To23[301].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.44422701);
        Averages2022To23[313].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.61135371);
        Averages2022To23[871].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.9463807);
        Averages2022To23[383].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.71981661);
        Averages2022To23[869].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 37.77239709);
        Averages2022To23[308].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.16239316);
        Averages2022To23[205].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.09219858);
        Averages2022To23[206].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 61.47110333);
        Averages2022To23[309].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.63511188);
        Averages2022To23[391].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 63.70875995);
        Averages2022To23[831].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 50.58139535);
        Averages2022To23[802].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.39393939);
        Averages2022To23[929].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.87794955);
        Averages2022To23[340].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.47058824);
        Averages2022To23[357].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.24711908);
        Averages2022To23[822].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.26829268);
        Averages2022To23[204].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 61.00628931);
        Averages2022To23[887].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.4904943);
        Averages2022To23[937].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.67150319);
        Averages2022To23[382].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.21020656);
        Averages2022To23[884].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.95431472);
        Averages2022To23[926].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.51882414);
        Averages2022To23[354].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.01365706);
        Averages2022To23[344].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.85232068);
        Averages2022To23[874].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.95375723);
        Averages2022To23[315].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.59839357);
        Averages2022To23[806].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 58.20668693);
        Averages2022To23[839].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.26132771);
        Averages2022To23[940].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.49790795);
        Averages2022To23[872].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 23.29192547);
        Averages2022To23[840].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.37342908);
        Averages2022To23[892].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 59.18367347);
        Averages2022To23[355].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 59.02702703);
        Averages2022To23[861].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.43362832);
        Averages2022To23[380].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.93328795);
        Averages2022To23[845].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.69230769);
        Averages2022To23[801].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.72171254);
        Averages2022To23[393].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 55.53772071);
        Averages2022To23[916].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.84100418);
        Averages2022To23[314].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 32.69230769);
        Averages2022To23[881].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.68660969);
        Averages2022To23[208].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 63.32378223);
        Averages2022To23[332].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.40425532);
        Averages2022To23[333].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.09656925);
        Averages2022To23[302].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.19205298);
        Averages2022To23[931].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.25095057);
        Averages2022To23[320].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.33333333);
        Averages2022To23[876].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.64705882);
        Averages2022To23[846].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.29931973);
        Averages2022To23[305].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.35023041);
        Averages2022To23[331].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.40229885);
        Averages2022To23[370].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 55.02283105);
        Averages2022To23[877].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.42431762);
    }
}
