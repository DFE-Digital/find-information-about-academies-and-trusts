using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data.Hardcoded;

[ExcludeFromCodeCoverage]
public static class FreeSchoolMealsData
{
    public static Dictionary<int, FreeSchoolMealsAverage> Averages2023To24 { get; } = new();

    public static readonly DateTime LastUpdated = new(2024, 8, 6, 0, 0, 0, DateTimeKind.Utc);

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
        Averages2023To24.Add(830, new FreeSchoolMealsAverage(830, "Derbyshire", "E10000007"));
        Averages2023To24.Add(831, new FreeSchoolMealsAverage(831, "Derby", "E06000015"));
        Averages2023To24.Add(855, new FreeSchoolMealsAverage(855, "Leicestershire", "E10000018"));
        Averages2023To24.Add(856, new FreeSchoolMealsAverage(856, "Leicester", "E06000016"));
        Averages2023To24.Add(857, new FreeSchoolMealsAverage(857, "Rutland", "E06000017"));
        Averages2023To24.Add(891, new FreeSchoolMealsAverage(891, "Nottinghamshire", "E10000024"));
        Averages2023To24.Add(892, new FreeSchoolMealsAverage(892, "Nottingham", "E06000018"));
        Averages2023To24.Add(925, new FreeSchoolMealsAverage(925, "Lincolnshire", "E10000019"));
        Averages2023To24.Add(940, new FreeSchoolMealsAverage(940, "North Northamptonshire", "E06000061"));
        Averages2023To24.Add(941, new FreeSchoolMealsAverage(941, "West Northamptonshire", "E06000062"));
        Averages2023To24.Add(821, new FreeSchoolMealsAverage(821, "Luton", "E06000032"));
        Averages2023To24.Add(822, new FreeSchoolMealsAverage(822, "Bedford", "E06000055"));
        Averages2023To24.Add(823, new FreeSchoolMealsAverage(823, "Central Bedfordshire", "E06000056"));
        Averages2023To24.Add(873, new FreeSchoolMealsAverage(873, "Cambridgeshire", "E10000003"));
        Averages2023To24.Add(874, new FreeSchoolMealsAverage(874, "Peterborough", "E06000031"));
        Averages2023To24.Add(881, new FreeSchoolMealsAverage(881, "Essex", "E10000012"));
        Averages2023To24.Add(882, new FreeSchoolMealsAverage(882, "Southend-on-Sea", "E06000033"));
        Averages2023To24.Add(883, new FreeSchoolMealsAverage(883, "Thurrock", "E06000034"));
        Averages2023To24.Add(919, new FreeSchoolMealsAverage(919, "Hertfordshire", "E10000015"));
        Averages2023To24.Add(926, new FreeSchoolMealsAverage(926, "Norfolk", "E10000020"));
        Averages2023To24.Add(935, new FreeSchoolMealsAverage(935, "Suffolk", "E10000029"));
        Averages2023To24.Add(201, new FreeSchoolMealsAverage(201, "City of London", "E09000001"));
        Averages2023To24.Add(202, new FreeSchoolMealsAverage(202, "Camden", "E09000007"));
        Averages2023To24.Add(203, new FreeSchoolMealsAverage(203, "Greenwich", "E09000011"));
        Averages2023To24.Add(204, new FreeSchoolMealsAverage(204, "Hackney", "E09000012"));
        Averages2023To24.Add(205, new FreeSchoolMealsAverage(205, "Hammersmith and Fulham", "E09000013"));
        Averages2023To24.Add(206, new FreeSchoolMealsAverage(206, "Islington", "E09000019"));
        Averages2023To24.Add(207, new FreeSchoolMealsAverage(207, "Kensington and Chelsea", "E09000020"));
        Averages2023To24.Add(208, new FreeSchoolMealsAverage(208, "Lambeth", "E09000022"));
        Averages2023To24.Add(209, new FreeSchoolMealsAverage(209, "Lewisham", "E09000023"));
        Averages2023To24.Add(210, new FreeSchoolMealsAverage(210, "Southwark", "E09000028"));
        Averages2023To24.Add(211, new FreeSchoolMealsAverage(211, "Tower Hamlets", "E09000030"));
        Averages2023To24.Add(212, new FreeSchoolMealsAverage(212, "Wandsworth", "E09000032"));
        Averages2023To24.Add(213, new FreeSchoolMealsAverage(213, "Westminster", "E09000033"));
        Averages2023To24.Add(301, new FreeSchoolMealsAverage(301, "Barking and Dagenham", "E09000002"));
        Averages2023To24.Add(302, new FreeSchoolMealsAverage(302, "Barnet", "E09000003"));
        Averages2023To24.Add(303, new FreeSchoolMealsAverage(303, "Bexley", "E09000004"));
        Averages2023To24.Add(304, new FreeSchoolMealsAverage(304, "Brent", "E09000005"));
        Averages2023To24.Add(305, new FreeSchoolMealsAverage(305, "Bromley", "E09000006"));
        Averages2023To24.Add(306, new FreeSchoolMealsAverage(306, "Croydon", "E09000008"));
        Averages2023To24.Add(307, new FreeSchoolMealsAverage(307, "Ealing", "E09000009"));
        Averages2023To24.Add(308, new FreeSchoolMealsAverage(308, "Enfield", "E09000010"));
        Averages2023To24.Add(309, new FreeSchoolMealsAverage(309, "Haringey", "E09000014"));
        Averages2023To24.Add(310, new FreeSchoolMealsAverage(310, "Harrow", "E09000015"));
        Averages2023To24.Add(311, new FreeSchoolMealsAverage(311, "Havering", "E09000016"));
        Averages2023To24.Add(312, new FreeSchoolMealsAverage(312, "Hillingdon", "E09000017"));
        Averages2023To24.Add(313, new FreeSchoolMealsAverage(313, "Hounslow", "E09000018"));
        Averages2023To24.Add(314, new FreeSchoolMealsAverage(314, "Kingston upon Thames", "E09000021"));
        Averages2023To24.Add(315, new FreeSchoolMealsAverage(315, "Merton", "E09000024"));
        Averages2023To24.Add(316, new FreeSchoolMealsAverage(316, "Newham", "E09000025"));
        Averages2023To24.Add(317, new FreeSchoolMealsAverage(317, "Redbridge", "E09000026"));
        Averages2023To24.Add(318, new FreeSchoolMealsAverage(318, "Richmond upon Thames", "E09000027"));
        Averages2023To24.Add(319, new FreeSchoolMealsAverage(319, "Sutton", "E09000029"));
        Averages2023To24.Add(320, new FreeSchoolMealsAverage(320, "Waltham Forest", "E09000031"));
        Averages2023To24.Add(390, new FreeSchoolMealsAverage(390, "Gateshead", "E08000037"));
        Averages2023To24.Add(391, new FreeSchoolMealsAverage(391, "Newcastle upon Tyne", "E08000021"));
        Averages2023To24.Add(392, new FreeSchoolMealsAverage(392, "North Tyneside", "E08000022"));
        Averages2023To24.Add(393, new FreeSchoolMealsAverage(393, "South Tyneside", "E08000023"));
        Averages2023To24.Add(394, new FreeSchoolMealsAverage(394, "Sunderland", "E08000024"));
        Averages2023To24.Add(805, new FreeSchoolMealsAverage(805, "Hartlepool", "E06000001"));
        Averages2023To24.Add(806, new FreeSchoolMealsAverage(806, "Middlesbrough", "E06000002"));
        Averages2023To24.Add(807, new FreeSchoolMealsAverage(807, "Redcar and Cleveland", "E06000003"));
        Averages2023To24.Add(808, new FreeSchoolMealsAverage(808, "Stockton-on-Tees", "E06000004"));
        Averages2023To24.Add(840, new FreeSchoolMealsAverage(840, "County Durham", "E06000047"));
        Averages2023To24.Add(841, new FreeSchoolMealsAverage(841, "Darlington", "E06000005"));
        Averages2023To24.Add(929, new FreeSchoolMealsAverage(929, "Northumberland", "E06000057"));
        Averages2023To24.Add(340, new FreeSchoolMealsAverage(340, "Knowsley", "E08000011"));
        Averages2023To24.Add(341, new FreeSchoolMealsAverage(341, "Liverpool", "E08000012"));
        Averages2023To24.Add(342, new FreeSchoolMealsAverage(342, "St. Helens", "E08000013"));
        Averages2023To24.Add(343, new FreeSchoolMealsAverage(343, "Sefton", "E08000014"));
        Averages2023To24.Add(344, new FreeSchoolMealsAverage(344, "Wirral", "E08000015"));
        Averages2023To24.Add(350, new FreeSchoolMealsAverage(350, "Bolton", "E08000001"));
        Averages2023To24.Add(351, new FreeSchoolMealsAverage(351, "Bury", "E08000002"));
        Averages2023To24.Add(352, new FreeSchoolMealsAverage(352, "Manchester", "E08000003"));
        Averages2023To24.Add(353, new FreeSchoolMealsAverage(353, "Oldham", "E08000004"));
        Averages2023To24.Add(354, new FreeSchoolMealsAverage(354, "Rochdale", "E08000005"));
        Averages2023To24.Add(355, new FreeSchoolMealsAverage(355, "Salford", "E08000006"));
        Averages2023To24.Add(356, new FreeSchoolMealsAverage(356, "Stockport", "E08000007"));
        Averages2023To24.Add(357, new FreeSchoolMealsAverage(357, "Tameside", "E08000008"));
        Averages2023To24.Add(358, new FreeSchoolMealsAverage(358, "Trafford", "E08000009"));
        Averages2023To24.Add(359, new FreeSchoolMealsAverage(359, "Wigan", "E08000010"));
        Averages2023To24.Add(876, new FreeSchoolMealsAverage(876, "Halton", "E06000006"));
        Averages2023To24.Add(877, new FreeSchoolMealsAverage(877, "Warrington", "E06000007"));
        Averages2023To24.Add(888, new FreeSchoolMealsAverage(888, "Lancashire", "E10000017"));
        Averages2023To24.Add(889, new FreeSchoolMealsAverage(889, "Blackburn with Darwen", "E06000008"));
        Averages2023To24.Add(890, new FreeSchoolMealsAverage(890, "Blackpool", "E06000009"));
        Averages2023To24.Add(895, new FreeSchoolMealsAverage(895, "Cheshire East", "E06000049"));
        Averages2023To24.Add(896, new FreeSchoolMealsAverage(896, "Cheshire West and Chester", "E06000050"));
        Averages2023To24.Add(942, new FreeSchoolMealsAverage(942, "Cumberland", "E06000063"));
        Averages2023To24.Add(943, new FreeSchoolMealsAverage(943, "Westmorland and Furness", "E06000064"));
        Averages2023To24.Add(825, new FreeSchoolMealsAverage(825, "Buckinghamshire", "E06000060"));
        Averages2023To24.Add(826, new FreeSchoolMealsAverage(826, "Milton Keynes", "E06000042"));
        Averages2023To24.Add(845, new FreeSchoolMealsAverage(845, "East Sussex", "E10000011"));
        Averages2023To24.Add(846, new FreeSchoolMealsAverage(846, "Brighton and Hove", "E06000043"));
        Averages2023To24.Add(850, new FreeSchoolMealsAverage(850, "Hampshire", "E10000014"));
        Averages2023To24.Add(851, new FreeSchoolMealsAverage(851, "Portsmouth", "E06000044"));
        Averages2023To24.Add(852, new FreeSchoolMealsAverage(852, "Southampton", "E06000045"));
        Averages2023To24.Add(867, new FreeSchoolMealsAverage(867, "Bracknell Forest", "E06000036"));
        Averages2023To24.Add(868, new FreeSchoolMealsAverage(868, "Windsor and Maidenhead", "E06000040"));
        Averages2023To24.Add(869, new FreeSchoolMealsAverage(869, "West Berkshire", "E06000037"));
        Averages2023To24.Add(870, new FreeSchoolMealsAverage(870, "Reading", "E06000038"));
        Averages2023To24.Add(871, new FreeSchoolMealsAverage(871, "Slough", "E06000039"));
        Averages2023To24.Add(872, new FreeSchoolMealsAverage(872, "Wokingham", "E06000041"));
        Averages2023To24.Add(886, new FreeSchoolMealsAverage(886, "Kent", "E10000016"));
        Averages2023To24.Add(887, new FreeSchoolMealsAverage(887, "Medway", "E06000035"));
        Averages2023To24.Add(921, new FreeSchoolMealsAverage(921, "Isle of Wight", "E06000046"));
        Averages2023To24.Add(931, new FreeSchoolMealsAverage(931, "Oxfordshire", "E10000025"));
        Averages2023To24.Add(936, new FreeSchoolMealsAverage(936, "Surrey", "E10000030"));
        Averages2023To24.Add(938, new FreeSchoolMealsAverage(938, "West Sussex", "E10000032"));
        Averages2023To24.Add(420, new FreeSchoolMealsAverage(420, "Isles of Scilly", "E06000053"));
        Averages2023To24.Add(800, new FreeSchoolMealsAverage(800, "Bath and North East Somerset", "E06000022"));
        Averages2023To24.Add(801, new FreeSchoolMealsAverage(801, "Bristol, City of", "E06000023"));
        Averages2023To24.Add(802, new FreeSchoolMealsAverage(802, "North Somerset", "E06000024"));
        Averages2023To24.Add(803, new FreeSchoolMealsAverage(803, "South Gloucestershire", "E06000025"));
        Averages2023To24.Add(838, new FreeSchoolMealsAverage(838, "Dorset", "E06000059"));
        Averages2023To24.Add(839, new FreeSchoolMealsAverage(839, "Bournemouth, Christchurch and Poole", "E06000058"));
        Averages2023To24.Add(865, new FreeSchoolMealsAverage(865, "Wiltshire", "E06000054"));
        Averages2023To24.Add(866, new FreeSchoolMealsAverage(866, "Swindon", "E06000030"));
        Averages2023To24.Add(878, new FreeSchoolMealsAverage(878, "Devon", "E10000008"));
        Averages2023To24.Add(879, new FreeSchoolMealsAverage(879, "Plymouth", "E06000026"));
        Averages2023To24.Add(880, new FreeSchoolMealsAverage(880, "Torbay", "E06000027"));
        Averages2023To24.Add(908, new FreeSchoolMealsAverage(908, "Cornwall", "E06000052"));
        Averages2023To24.Add(916, new FreeSchoolMealsAverage(916, "Gloucestershire", "E10000013"));
        Averages2023To24.Add(933, new FreeSchoolMealsAverage(933, "Somerset", "E06000066"));
        Averages2023To24.Add(330, new FreeSchoolMealsAverage(330, "Birmingham", "E08000025"));
        Averages2023To24.Add(331, new FreeSchoolMealsAverage(331, "Coventry", "E08000026"));
        Averages2023To24.Add(332, new FreeSchoolMealsAverage(332, "Dudley", "E08000027"));
        Averages2023To24.Add(333, new FreeSchoolMealsAverage(333, "Sandwell", "E08000028"));
        Averages2023To24.Add(334, new FreeSchoolMealsAverage(334, "Solihull", "E08000029"));
        Averages2023To24.Add(335, new FreeSchoolMealsAverage(335, "Walsall", "E08000030"));
        Averages2023To24.Add(336, new FreeSchoolMealsAverage(336, "Wolverhampton", "E08000031"));
        Averages2023To24.Add(860, new FreeSchoolMealsAverage(860, "Staffordshire", "E10000028"));
        Averages2023To24.Add(861, new FreeSchoolMealsAverage(861, "Stoke-on-Trent", "E06000021"));
        Averages2023To24.Add(884, new FreeSchoolMealsAverage(884, "Herefordshire, County of", "E06000019"));
        Averages2023To24.Add(885, new FreeSchoolMealsAverage(885, "Worcestershire", "E10000034"));
        Averages2023To24.Add(893, new FreeSchoolMealsAverage(893, "Shropshire", "E06000051"));
        Averages2023To24.Add(894, new FreeSchoolMealsAverage(894, "Telford and Wrekin", "E06000020"));
        Averages2023To24.Add(937, new FreeSchoolMealsAverage(937, "Warwickshire", "E10000031"));
        Averages2023To24.Add(370, new FreeSchoolMealsAverage(370, "Barnsley", "E08000016"));
        Averages2023To24.Add(371, new FreeSchoolMealsAverage(371, "Doncaster", "E08000017"));
        Averages2023To24.Add(372, new FreeSchoolMealsAverage(372, "Rotherham", "E08000018"));
        Averages2023To24.Add(373, new FreeSchoolMealsAverage(373, "Sheffield", "E08000019"));
        Averages2023To24.Add(380, new FreeSchoolMealsAverage(380, "Bradford", "E08000032"));
        Averages2023To24.Add(381, new FreeSchoolMealsAverage(381, "Calderdale", "E08000033"));
        Averages2023To24.Add(382, new FreeSchoolMealsAverage(382, "Kirklees", "E08000034"));
        Averages2023To24.Add(383, new FreeSchoolMealsAverage(383, "Leeds", "E08000035"));
        Averages2023To24.Add(384, new FreeSchoolMealsAverage(384, "Wakefield", "E08000036"));
        Averages2023To24.Add(810, new FreeSchoolMealsAverage(810, "Kingston upon Hull, City of", "E06000010"));
        Averages2023To24.Add(811, new FreeSchoolMealsAverage(811, "East Riding of Yorkshire", "E06000011"));
        Averages2023To24.Add(812, new FreeSchoolMealsAverage(812, "North East Lincolnshire", "E06000012"));
        Averages2023To24.Add(813, new FreeSchoolMealsAverage(813, "North Lincolnshire", "E06000013"));
        Averages2023To24.Add(815, new FreeSchoolMealsAverage(815, "North Yorkshire", "E06000065"));
        Averages2023To24.Add(816, new FreeSchoolMealsAverage(816, "York", "E06000014"));
        Averages2023To24.Add(-1, new FreeSchoolMealsAverage(-1, "National"));
    }

    /// <summary>
    /// Hard-coding percentage of pupils eligible for free school meals per local authority by phase
    /// Does not include phase group types which are not present in Academies db Gias.Establishment table
    /// </summary>
    private static void AddPercentagesByPhaseType()
    {
        AddStateFundedApSchoolsFsmData();
        AddStateFundedPrimarySchoolFsmData();
        AddStateFundedSecondaryFsmData();
        AddStateFundedSpecialSchoolFsmData();
    }

    private static void AddStateFundedSpecialSchoolFsmData()
    {
        Averages2023To24[830].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.82174688);
        Averages2023To24[831].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.09677419);
        Averages2023To24[855].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 35.6865285);
        Averages2023To24[856].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.33011272);
        Averages2023To24[857].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 0);
        Averages2023To24[891].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.57315937);
        Averages2023To24[892].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 60.72072072);
        Averages2023To24[925].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.55096012);
        Averages2023To24[940].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.14088159);
        Averages2023To24[941].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.41176471);
        Averages2023To24[821].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 36.76662321);
        Averages2023To24[822].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.1443299);
        Averages2023To24[823].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.12154696);
        Averages2023To24[873].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.59042218);
        Averages2023To24[874].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.62178517);
        Averages2023To24[881].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.16504854);
        Averages2023To24[882].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 50);
        Averages2023To24[883].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.58093126);
        Averages2023To24[919].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.16738197);
        Averages2023To24[926].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.75518672);
        Averages2023To24[935].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.08945687);
        Averages2023To24[202].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.44444444);
        Averages2023To24[203].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 55.01022495);
        Averages2023To24[204].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 58.86524823);
        Averages2023To24[205].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 59.73282443);
        Averages2023To24[206].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 61.13445378);
        Averages2023To24[207].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.22413793);
        Averages2023To24[208].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 65.03496503);
        Averages2023To24[209].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.9760479);
        Averages2023To24[210].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.59944367);
        Averages2023To24[211].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 58.35866261);
        Averages2023To24[212].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.7867036);
        Averages2023To24[213].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 63.02521008);
        Averages2023To24[301].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.09543568);
        Averages2023To24[302].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.13377926);
        Averages2023To24[303].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.28571429);
        Averages2023To24[304].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.96228869);
        Averages2023To24[305].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.69451697);
        Averages2023To24[306].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.29885057);
        Averages2023To24[307].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.6635514);
        Averages2023To24[308].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 55.89430894);
        Averages2023To24[309].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.65082267);
        Averages2023To24[310].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.30275229);
        Averages2023To24[311].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.12903226);
        Averages2023To24[312].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.84965831);
        Averages2023To24[313].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.69653524);
        Averages2023To24[314].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 34.15730337);
        Averages2023To24[315].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.87846482);
        Averages2023To24[316].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.5412844);
        Averages2023To24[317].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.77419355);
        Averages2023To24[318].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 32.92307692);
        Averages2023To24[319].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.68439108);
        Averages2023To24[320].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.04863222);
        Averages2023To24[390].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.0960452);
        Averages2023To24[391].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 65.05190311);
        Averages2023To24[392].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.35648755);
        Averages2023To24[393].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.14285714);
        Averages2023To24[394].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.16591928);
        Averages2023To24[805].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 59.4488189);
        Averages2023To24[806].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 58.16993464);
        Averages2023To24[807].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.33333333);
        Averages2023To24[808].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.29202037);
        Averages2023To24[840].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 60.26038438);
        Averages2023To24[841].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.22128852);
        Averages2023To24[929].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.89669088);
        Averages2023To24[340].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 60.81967213);
        Averages2023To24[341].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.6405868);
        Averages2023To24[342].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.46236559);
        Averages2023To24[343].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.51506456);
        Averages2023To24[344].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.36232926);
        Averages2023To24[350].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.88092613);
        Averages2023To24[351].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.29223744);
        Averages2023To24[352].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 59.34515689);
        Averages2023To24[353].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 50.28712059);
        Averages2023To24[354].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.21465969);
        Averages2023To24[355].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 60.75514874);
        Averages2023To24[356].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.65435746);
        Averages2023To24[357].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.80124224);
        Averages2023To24[358].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.16384915);
        Averages2023To24[359].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.60622914);
        Averages2023To24[876].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 56.58914729);
        Averages2023To24[877].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.40682415);
        Averages2023To24[888].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.16352201);
        Averages2023To24[889].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.09115282);
        Averages2023To24[890].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 62.54826255);
        Averages2023To24[895].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 37.84246575);
        Averages2023To24[896].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.86977648);
        Averages2023To24[942].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 37.22627737);
        Averages2023To24[943].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.91845494);
        Averages2023To24[825].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 36.75496689);
        Averages2023To24[826].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.99260173);
        Averages2023To24[845].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.88424197);
        Averages2023To24[846].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.44071588);
        Averages2023To24[850].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.156142);
        Averages2023To24[851].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.98816568);
        Averages2023To24[852].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 59.16666667);
        Averages2023To24[867].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 37.80487805);
        Averages2023To24[868].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 26.70454545);
        Averages2023To24[869].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 37.57575758);
        Averages2023To24[870].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.89940828);
        Averages2023To24[871].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.33333333);
        Averages2023To24[872].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 26.21082621);
        Averages2023To24[886].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.03202196);
        Averages2023To24[887].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.52155172);
        Averages2023To24[921].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.90163934);
        Averages2023To24[931].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.43624161);
        Averages2023To24[936].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.56749311);
        Averages2023To24[938].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 38.86554622);
        Averages2023To24[800].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.53374233);
        Averages2023To24[801].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 58.63219349);
        Averages2023To24[802].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.95744681);
        Averages2023To24[803].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 31.41831239);
        Averages2023To24[838].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.88674389);
        Averages2023To24[839].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 50.5787037);
        Averages2023To24[865].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 40.89026915);
        Averages2023To24[866].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.57976654);
        Averages2023To24[878].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.95856185);
        Averages2023To24[879].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.5147929);
        Averages2023To24[880].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.73195876);
        Averages2023To24[908].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.57768053);
        Averages2023To24[916].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.69208838);
        Averages2023To24[933].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.31095755);
        Averages2023To24[330].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.02934809);
        Averages2023To24[331].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.49280576);
        Averages2023To24[332].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 44.082519);
        Averages2023To24[333].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.22615804);
        Averages2023To24[334].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 51.43678161);
        Averages2023To24[335].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 58.25136612);
        Averages2023To24[336].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 59.6432553);
        Averages2023To24[860].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 41.29480614);
        Averages2023To24[861].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 52.83893395);
        Averages2023To24[884].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 49.70238095);
        Averages2023To24[885].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.72608126);
        Averages2023To24[893].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.35378323);
        Averages2023To24[894].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.79773692);
        Averages2023To24[937].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 42.90450928);
        Averages2023To24[370].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57);
        Averages2023To24[371].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 46.38109306);
        Averages2023To24[372].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.93358634);
        Averages2023To24[373].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 57.41758242);
        Averages2023To24[380].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.96825397);
        Averages2023To24[381].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 39.28571429);
        Averages2023To24[382].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 48.92183288);
        Averages2023To24[383].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 50.61043285);
        Averages2023To24[384].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 53.72340426);
        Averages2023To24[810].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 54.64632455);
        Averages2023To24[811].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 37.57225434);
        Averages2023To24[812].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 50.46728972);
        Averages2023To24[813].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 43.82352941);
        Averages2023To24[815].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 45.36453645);
        Averages2023To24[816].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 32);
        Averages2023To24[-1].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSpecialSchool, 47.81778466);
    }

    private static void AddStateFundedSecondaryFsmData()
    {
        Averages2023To24[830].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.84727871);
        Averages2023To24[831].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.2591016);
        Averages2023To24[855].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.27717187);
        Averages2023To24[856].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.20595104);
        Averages2023To24[857].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.11605022);
        Averages2023To24[891].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.48610535);
        Averages2023To24[892].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 39.2855183);
        Averages2023To24[925].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.74015057);
        Averages2023To24[940].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.62669545);
        Averages2023To24[941].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.04636992);
        Averages2023To24[821].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.67567568);
        Averages2023To24[822].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.06388206);
        Averages2023To24[823].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.34548944);
        Averages2023To24[873].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.11347291);
        Averages2023To24[874].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.35734853);
        Averages2023To24[881].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.62525941);
        Averages2023To24[882].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.54597234);
        Averages2023To24[883].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.33643874);
        Averages2023To24[919].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.54513208);
        Averages2023To24[926].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.80910912);
        Averages2023To24[935].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.93835477);
        Averages2023To24[202].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 45.46295073);
        Averages2023To24[203].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 30.00311429);
        Averages2023To24[204].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 46.8983037);
        Averages2023To24[205].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.7016129);
        Averages2023To24[206].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 50.15467384);
        Averages2023To24[207].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 37.98893718);
        Averages2023To24[208].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 41.09243034);
        Averages2023To24[209].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.88647811);
        Averages2023To24[210].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 38.75777147);
        Averages2023To24[211].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 45.07207619);
        Averages2023To24[212].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.71900489);
        Averages2023To24[213].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 44.4526161);
        Averages2023To24[301].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.47738301);
        Averages2023To24[302].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.20840687);
        Averages2023To24[303].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.81999508);
        Averages2023To24[304].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.13258272);
        Averages2023To24[305].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.26907946);
        Averages2023To24[306].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 34.6937751);
        Averages2023To24[307].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 30.04412781);
        Averages2023To24[308].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.41038438);
        Averages2023To24[309].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.12426651);
        Averages2023To24[310].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.27298545);
        Averages2023To24[311].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.70083722);
        Averages2023To24[312].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.69904946);
        Averages2023To24[313].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.68316262);
        Averages2023To24[314].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.34405817);
        Averages2023To24[315].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.66875757);
        Averages2023To24[316].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 40.83677313);
        Averages2023To24[317].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.09713836);
        Averages2023To24[318].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 16.07003891);
        Averages2023To24[319].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 16.50205761);
        Averages2023To24[320].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.76910569);
        Averages2023To24[390].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 28.7926852);
        Averages2023To24[391].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 41.49715216);
        Averages2023To24[392].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.90527313);
        Averages2023To24[393].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 33.1570476);
        Averages2023To24[394].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 35.27936289);
        Averages2023To24[805].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 40.82311734);
        Averages2023To24[806].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 45.0442173);
        Averages2023To24[807].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.69065626);
        Averages2023To24[808].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 28.19552003);
        Averages2023To24[840].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.79637127);
        Averages2023To24[841].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.87637699);
        Averages2023To24[929].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.15952504);
        Averages2023To24[340].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 49.16172735);
        Averages2023To24[341].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 34.45440429);
        Averages2023To24[342].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.0384431);
        Averages2023To24[343].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.46284023);
        Averages2023To24[344].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.85005171);
        Averages2023To24[350].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 28.84406443);
        Averages2023To24[351].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.89833479);
        Averages2023To24[352].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 44.90536687);
        Averages2023To24[353].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 34.15700977);
        Averages2023To24[354].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 34.15436289);
        Averages2023To24[355].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 36.13005912);
        Averages2023To24[356].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.22922311);
        Averages2023To24[357].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 31.50143919);
        Averages2023To24[358].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 16.26295563);
        Averages2023To24[359].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.70686836);
        Averages2023To24[876].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 39.75978982);
        Averages2023To24[877].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.63456845);
        Averages2023To24[888].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.22035776);
        Averages2023To24[889].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.20525931);
        Averages2023To24[890].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 45.88578243);
        Averages2023To24[895].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.18541061);
        Averages2023To24[896].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.15444456);
        Averages2023To24[942].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.70027504);
        Averages2023To24[943].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.77392831);
        Averages2023To24[825].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 13.97534624);
        Averages2023To24[826].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.48511296);
        Averages2023To24[845].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.7617305);
        Averages2023To24[846].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.11930874);
        Averages2023To24[850].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.09564839);
        Averages2023To24[851].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 35.33543449);
        Averages2023To24[852].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 35.74857818);
        Averages2023To24[867].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.02109248);
        Averages2023To24[868].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.56082148);
        Averages2023To24[869].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.97843873);
        Averages2023To24[870].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.42769382);
        Averages2023To24[871].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.4286261);
        Averages2023To24[872].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 10.42764039);
        Averages2023To24[886].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.07903984);
        Averages2023To24[887].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.21805146);
        Averages2023To24[921].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.65940054);
        Averages2023To24[931].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 16.9340226);
        Averages2023To24[936].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 14.02070962);
        Averages2023To24[938].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 15.63611432);
        Averages2023To24[420].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 2.822580645);
        Averages2023To24[800].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.94264907);
        Averages2023To24[801].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.38175451);
        Averages2023To24[802].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.23727063);
        Averages2023To24[803].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.21064272);
        Averages2023To24[838].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.77755392);
        Averages2023To24[839].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 19.60496769);
        Averages2023To24[865].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.09943936);
        Averages2023To24[866].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.11835578);
        Averages2023To24[878].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.5284499);
        Averages2023To24[879].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.03361898);
        Averages2023To24[880].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 24.07315868);
        Averages2023To24[908].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 23.88110231);
        Averages2023To24[916].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.1758584);
        Averages2023To24[933].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.64131061);
        Averages2023To24[330].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 40.13500877);
        Averages2023To24[331].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 30.07298612);
        Averages2023To24[332].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.71018004);
        Averages2023To24[333].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 36.25079702);
        Averages2023To24[334].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.87705375);
        Averages2023To24[335].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 37.24386285);
        Averages2023To24[336].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 41.97121301);
        Averages2023To24[860].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 20.57640751);
        Averages2023To24[861].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 34.63501956);
        Averages2023To24[884].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.48650927);
        Averages2023To24[885].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.04103872);
        Averages2023To24[893].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 18.95806861);
        Averages2023To24[894].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.53588325);
        Averages2023To24[937].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 21.76751778);
        Averages2023To24[370].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.32041707);
        Averages2023To24[371].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 30.39945451);
        Averages2023To24[372].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.56695157);
        Averages2023To24[373].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 33.5372005);
        Averages2023To24[380].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.99882566);
        Averages2023To24[381].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 26.24633431);
        Averages2023To24[382].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 28.39938691);
        Averages2023To24[383].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.21433715);
        Averages2023To24[384].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 27.29264019);
        Averages2023To24[810].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 34.04605263);
        Averages2023To24[811].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 22.5655847);
        Averages2023To24[812].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 32.34711835);
        Averages2023To24[813].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 29.99511957);
        Averages2023To24[815].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 17.88595357);
        Averages2023To24[816].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 16.65302782);
        Averages2023To24[-1].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedSecondary, 25.62099314);
    }

    private static void AddStateFundedPrimarySchoolFsmData()
    {
        Averages2023To24[830].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.90919045);
        Averages2023To24[831].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 35.19736842);
        Averages2023To24[855].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.04777932);
        Averages2023To24[856].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.93879339);
        Averages2023To24[857].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 10.70258464);
        Averages2023To24[891].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.7417703);
        Averages2023To24[892].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 37.4110203);
        Averages2023To24[925].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.38843125);
        Averages2023To24[940].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.14764074);
        Averages2023To24[941].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.1191201);
        Averages2023To24[821].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.69806763);
        Averages2023To24[822].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.82858685);
        Averages2023To24[823].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 13.07137129);
        Averages2023To24[873].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.28367916);
        Averages2023To24[874].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.15631681);
        Averages2023To24[881].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.2421148);
        Averages2023To24[882].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.84138512);
        Averages2023To24[883].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.04517811);
        Averages2023To24[919].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.24030851);
        Averages2023To24[926].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.78095238);
        Averages2023To24[935].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.43298521);
        Averages2023To24[201].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.72131148);
        Averages2023To24[202].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 43.12885879);
        Averages2023To24[203].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.07503039);
        Averages2023To24[204].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 39.74463619);
        Averages2023To24[205].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 31.20963328);
        Averages2023To24[206].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 43.18092699);
        Averages2023To24[207].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.46994536);
        Averages2023To24[208].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 37.0382372);
        Averages2023To24[209].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.12732705);
        Averages2023To24[210].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 36.76960998);
        Averages2023To24[211].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 38.2033262);
        Averages2023To24[212].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.5461265);
        Averages2023To24[213].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 39.53190945);
        Averages2023To24[301].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.15384615);
        Averages2023To24[302].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.23899914);
        Averages2023To24[303].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.07666713);
        Averages2023To24[304].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.56487789);
        Averages2023To24[305].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.94215117);
        Averages2023To24[306].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.94993663);
        Averages2023To24[307].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.67067605);
        Averages2023To24[308].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.88869101);
        Averages2023To24[309].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.3137435);
        Averages2023To24[310].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.59871285);
        Averages2023To24[311].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.83572828);
        Averages2023To24[312].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.64518019);
        Averages2023To24[313].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.05449023);
        Averages2023To24[314].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.92082826);
        Averages2023To24[315].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.27300535);
        Averages2023To24[316].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 31.38226456);
        Averages2023To24[317].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.63361135);
        Averages2023To24[318].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 12.5172717);
        Averages2023To24[319].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.90746664);
        Averages2023To24[320].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.90632955);
        Averages2023To24[390].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.47459777);
        Averages2023To24[391].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 40.17703911);
        Averages2023To24[392].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.97354677);
        Averages2023To24[393].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 34.83230436);
        Averages2023To24[394].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.45660307);
        Averages2023To24[805].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 39.7260274);
        Averages2023To24[806].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 42.23180822);
        Averages2023To24[807].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.62693825);
        Averages2023To24[808].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.67872205);
        Averages2023To24[840].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.50207684);
        Averages2023To24[841].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.45557123);
        Averages2023To24[929].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.45098039);
        Averages2023To24[340].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 37.97672462);
        Averages2023To24[341].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 35.06019516);
        Averages2023To24[342].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.2244898);
        Averages2023To24[343].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.36882766);
        Averages2023To24[344].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.54214705);
        Averages2023To24[350].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.90398788);
        Averages2023To24[351].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.00991656);
        Averages2023To24[352].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 43.68684536);
        Averages2023To24[353].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 32.76614663);
        Averages2023To24[354].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 30.1983538);
        Averages2023To24[355].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.27989477);
        Averages2023To24[356].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.38929479);
        Averages2023To24[357].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.18088705);
        Averages2023To24[358].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.24474461);
        Averages2023To24[359].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.75647668);
        Averages2023To24[876].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 38.56321259);
        Averages2023To24[877].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.00489896);
        Averages2023To24[888].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.7007882);
        Averages2023To24[889].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.18281536);
        Averages2023To24[890].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 39.75009055);
        Averages2023To24[895].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.15221297);
        Averages2023To24[896].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.12872701);
        Averages2023To24[942].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.99343069);
        Averages2023To24[943].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.37963203);
        Averages2023To24[825].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.30390515);
        Averages2023To24[826].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.21039613);
        Averages2023To24[845].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.73313027);
        Averages2023To24[846].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.41041276);
        Averages2023To24[850].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.15688213);
        Averages2023To24[851].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.36302895);
        Averages2023To24[852].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 34.07575836);
        Averages2023To24[867].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 12.06015038);
        Averages2023To24[868].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 13.94477318);
        Averages2023To24[869].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.41901164);
        Averages2023To24[870].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.50873152);
        Averages2023To24[871].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.13731944);
        Averages2023To24[872].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 9.855662237);
        Averages2023To24[886].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.70599041);
        Averages2023To24[887].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.98564955);
        Averages2023To24[921].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.19075009);
        Averages2023To24[931].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.84552435);
        Averages2023To24[936].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.37729959);
        Averages2023To24[938].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 15.53917972);
        Averages2023To24[800].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 17.21366879);
        Averages2023To24[801].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.64471915);
        Averages2023To24[802].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.12826035);
        Averages2023To24[803].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 14.25478626);
        Averages2023To24[838].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.92237097);
        Averages2023To24[839].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.11230804);
        Averages2023To24[865].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 16.25231314);
        Averages2023To24[866].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.0911336);
        Averages2023To24[878].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.53619498);
        Averages2023To24[879].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.65667915);
        Averages2023To24[880].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.29219975);
        Averages2023To24[908].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 22.47960445);
        Averages2023To24[916].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.70724029);
        Averages2023To24[933].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 20.74847514);
        Averages2023To24[330].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 41.65254118);
        Averages2023To24[331].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.20534753);
        Averages2023To24[332].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.00786992);
        Averages2023To24[333].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 34.61718021);
        Averages2023To24[334].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 23.61743656);
        Averages2023To24[335].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 35.25427452);
        Averages2023To24[336].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 42.66725564);
        Averages2023To24[860].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.38114284);
        Averages2023To24[861].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 39.27943058);
        Averages2023To24[884].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.00321052);
        Averages2023To24[885].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.47529218);
        Averages2023To24[893].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.43483955);
        Averages2023To24[894].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.80379329);
        Averages2023To24[937].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 21.97442131);
        Averages2023To24[370].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.4126544);
        Averages2023To24[371].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.4337228);
        Averages2023To24[372].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 27.60848315);
        Averages2023To24[373].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.38303737);
        Averages2023To24[380].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 28.80250308);
        Averages2023To24[381].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.17884331);
        Averages2023To24[382].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.18961537);
        Averages2023To24[383].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 25.69212156);
        Averages2023To24[384].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 26.30772817);
        Averages2023To24[810].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 34.41106585);
        Averages2023To24[811].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 19.71420116);
        Averages2023To24[812].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 33.39212228);
        Averages2023To24[813].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 29.54164763);
        Averages2023To24[815].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 18.07020044);
        Averages2023To24[816].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 17.18044825);
        Averages2023To24[-1].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedPrimary, 24.85431565);
    }

    private static void AddStateFundedApSchoolsFsmData()
    {
        Averages2023To24[830].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 68.82022472);
        Averages2023To24[831].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.45544554);
        Averages2023To24[855].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 83.33333333);
        Averages2023To24[856].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 69.23076923);
        Averages2023To24[892].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 71.57190635);
        Averages2023To24[925].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.48044693);
        Averages2023To24[941].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 47.66839378);
        Averages2023To24[821].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 44.11764706);
        Averages2023To24[822].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 69.23076923);
        Averages2023To24[823].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60.81081081);
        Averages2023To24[873].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.55555556);
        Averages2023To24[874].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 50);
        Averages2023To24[881].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 47.61904762);
        Averages2023To24[882].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.92957746);
        Averages2023To24[883].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 48.21428571);
        Averages2023To24[919].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 43.47826087);
        Averages2023To24[926].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.55748373);
        Averages2023To24[935].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 76.19047619);
        Averages2023To24[202].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.70588235);
        Averages2023To24[203].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 32.66666667);
        Averages2023To24[204].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 69.73684211);
        Averages2023To24[205].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 70.49180328);
        Averages2023To24[206].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.16666667);
        Averages2023To24[207].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 80.76923077);
        Averages2023To24[208].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 44.32989691);
        Averages2023To24[209].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 48.48484848);
        Averages2023To24[210].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 75.55555556);
        Averages2023To24[211].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 39.43661972);
        Averages2023To24[212].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60);
        Averages2023To24[213].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 68.33333333);
        Averages2023To24[301].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 23.89380531);
        Averages2023To24[302].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 56.60377358);
        Averages2023To24[303].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 40);
        Averages2023To24[304].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 34.28571429);
        Averages2023To24[305].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60);
        Averages2023To24[306].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.76190476);
        Averages2023To24[307].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 49.35064935);
        Averages2023To24[308].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 51.02040816);
        Averages2023To24[309].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.54545455);
        Averages2023To24[310].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 40);
        Averages2023To24[311].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.14285714);
        Averages2023To24[312].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 40.81632653);
        Averages2023To24[313].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 74.33628319);
        Averages2023To24[314].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.84552846);
        Averages2023To24[315].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 29.85074627);
        Averages2023To24[316].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 31.50684932);
        Averages2023To24[317].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.78947368);
        Averages2023To24[319].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.63302752);
        Averages2023To24[320].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 30.76923077);
        Averages2023To24[390].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 65.76576577);
        Averages2023To24[391].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 78.18181818);
        Averages2023To24[392].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.78343949);
        Averages2023To24[393].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 74.19354839);
        Averages2023To24[394].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 73.6);
        Averages2023To24[805].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 81.08108108);
        Averages2023To24[806].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 50);
        Averages2023To24[807].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 82.66666667);
        Averages2023To24[808].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 56.4516129);
        Averages2023To24[840].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 73.72262774);
        Averages2023To24[841].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.01694915);
        Averages2023To24[929].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 80);
        Averages2023To24[340].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 81.69014085);
        Averages2023To24[341].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 58.73015873);
        Averages2023To24[342].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.47368421);
        Averages2023To24[343].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 58.33333333);
        Averages2023To24[350].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 70.49180328);
        Averages2023To24[351].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.7008547);
        Averages2023To24[352].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 84.66257669);
        Averages2023To24[353].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.73770492);
        Averages2023To24[354].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 62.80487805);
        Averages2023To24[355].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 70);
        Averages2023To24[356].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.51612903);
        Averages2023To24[357].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 71.875);
        Averages2023To24[358].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 53.84615385);
        Averages2023To24[359].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 76.33587786);
        Averages2023To24[876].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 77.41935484);
        Averages2023To24[877].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.14285714);
        Averages2023To24[888].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.13537118);
        Averages2023To24[889].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 54.11764706);
        Averages2023To24[890].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 72.17391304);
        Averages2023To24[895].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.12903226);
        Averages2023To24[896].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 71.42857143);
        Averages2023To24[942].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 58.92857143);
        Averages2023To24[943].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 51.11111111);
        Averages2023To24[825].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.33333333);
        Averages2023To24[826].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.14285714);
        Averages2023To24[845].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 65.71428571);
        Averages2023To24[846].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 72.09302326);
        Averages2023To24[850].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.83018868);
        Averages2023To24[852].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 80);
        Averages2023To24[867].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.14285714);
        Averages2023To24[868].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 38.88888889);
        Averages2023To24[869].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 44.06779661);
        Averages2023To24[870].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 74.07407407);
        Averages2023To24[871].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.34782609);
        Averages2023To24[872].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 53.33333333);
        Averages2023To24[886].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 57.14285714);
        Averages2023To24[887].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 68.88888889);
        Averages2023To24[921].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 58.33333333);
        Averages2023To24[931].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 40);
        Averages2023To24[936].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 62.03703704);
        Averages2023To24[938].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60);
        Averages2023To24[801].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.43356643);
        Averages2023To24[802].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 72.34042553);
        Averages2023To24[803].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 47.61904762);
        Averages2023To24[838].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.56692913);
        Averages2023To24[839].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.66666667);
        Averages2023To24[866].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 65.51724138);
        Averages2023To24[878].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.66666667);
        Averages2023To24[879].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.19213974);
        Averages2023To24[880].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.58333333);
        Averages2023To24[908].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 62.35955056);
        Averages2023To24[916].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 51.72413793);
        Averages2023To24[933].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 52.90697674);
        Averages2023To24[330].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 69.93006993);
        Averages2023To24[331].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 68.51851852);
        Averages2023To24[332].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60);
        Averages2023To24[333].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.97247706);
        Averages2023To24[334].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 67.05882353);
        Averages2023To24[335].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 56.60377358);
        Averages2023To24[336].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 76.52173913);
        Averages2023To24[860].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 56.17977528);
        Averages2023To24[861].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 0);
        Averages2023To24[884].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.79310345);
        Averages2023To24[885].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.53846154);
        Averages2023To24[893].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 63.15789474);
        Averages2023To24[894].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 62.82051282);
        Averages2023To24[370].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 72.22222222);
        Averages2023To24[371].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.44444444);
        Averages2023To24[372].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 70.1754386);
        Averages2023To24[373].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 75.57251908);
        Averages2023To24[380].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 66.0130719);
        Averages2023To24[381].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 71.21212121);
        Averages2023To24[382].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 56.47058824);
        Averages2023To24[383].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 42.85714286);
        Averages2023To24[384].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 64.92146597);
        Averages2023To24[810].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 60.85858586);
        Averages2023To24[811].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 59.52380952);
        Averages2023To24[812].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 75.38461538);
        Averages2023To24[813].PercentOfPupilsByPhase.Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 37.5);
        Averages2023To24[815].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.71428571);
        Averages2023To24[816].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 55.625);
        Averages2023To24[-1].PercentOfPupilsByPhase
            .Add(ExploreEducationStatisticsPhaseType.StateFundedApSchool, 61.91375717);
    }
}
