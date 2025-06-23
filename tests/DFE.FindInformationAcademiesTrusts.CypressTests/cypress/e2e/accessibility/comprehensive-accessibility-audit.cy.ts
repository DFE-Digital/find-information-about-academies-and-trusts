/* eslint-disable @typescript-eslint/no-explicit-any, @typescript-eslint/no-unsafe-assignment, @typescript-eslint/no-unsafe-member-access */
import 'wick-a11y';
import 'cypress-axe';
import { testTrustData, testSchoolData, testFederationData } from '../../support/test-data-store';
import { generateComprehensiveAccessibilityReport, type AuditPageResult } from '../../support/accessibility-report-generator';

/**
 * Comprehensive Accessibility Audit Report Generator
 * 
 * Generates ONE comprehensive HTML audit report covering ALL application areas:
 * - Core pages (home, search, cookies, accessibility, privacy)
 * - Trust pages (all overview, contacts, governance, ofsted, academies, financial subpages)
 * - School pages (all overview, contacts, SEN, federation subpages)
 * 
 * This audit covers EVERY page tested in the UI regression tests to ensure
 * complete accessibility coverage across the entire application.
 * 
 * The report includes:
 * - Executive summary with overall statistics
 * - Impact level breakdown (critical, serious, moderate, minor)
 * - Detailed findings for each page area
 * - Comprehensive coverage of all accessibility rules
 */

describe('Comprehensive Application Accessibility Audit', () => {
    // Store all audit results to generate one comprehensive report
    const auditResults: AuditPageResult[] = [];

    beforeEach(() => {
        cy.injectAxe();
    });

    const auditPage = (pageName: string, pageCategory: string, url: string) => {
        cy.visit(url);
        cy.injectAxe();

        cy.window().then((win) => {
            cy.wrap(null).then(() => {
                return win.axe.run(win.document, {
                    resultTypes: ['violations', 'passes']
                });
            }).then((results) => {
                // Calculate impact level breakdown for this page
                const impactBreakdown: Record<string, number> = {};
                const impactViolations: Record<string, any[]> = {
                    critical: [],
                    serious: [],
                    moderate: [],
                    minor: [],
                    unknown: []
                };

                results.violations?.forEach((violation: any) => {
                    const impact = violation.impact ?? 'unknown';
                    impactBreakdown[impact] = (impactBreakdown[impact] ?? 0) + 1;
                    impactViolations[impact].push(violation);
                });

                const violationsLength = results.violations?.length ?? 0;
                const passesLength = results.passes?.length ?? 0;
                const violationsArray = results.violations ?? [];
                const passesArray = results.passes ?? [];

                const pageData = {
                    pageName,
                    pageCategory,
                    url,
                    summary: {
                        violations: violationsLength,
                        passes: passesLength,
                        totalRulesChecked: violationsLength + passesLength
                    },
                    impactBreakdown,
                    impactViolations,
                    violations: violationsArray,
                    passes: passesArray
                };

                // Store results for comprehensive report
                auditResults.push(pageData);

                cy.log(`🎯 Audited ${pageCategory}: ${pageName}`);
                cy.log(`✅ ${results.passes.length} passed | ❌ ${results.violations.length} failed`);

                if (results.violations.length > 0) {
                    const impactSummary = Object.entries(impactBreakdown)
                        .map(([impact, count]) => `${impact}:${count}`)
                        .join(', ');
                    cy.log(`📊 Impact: ${impactSummary}`);
                }
            });
        });
    };

    const auditPageWithoutScreenshots = (pageName: string, pageCategory: string, url: string) => {
        cy.visit(url);
        cy.injectAxe();

        cy.window().then((win) => {
            cy.wrap(null).then(() => {
                return win.axe.run(win.document, {
                    resultTypes: ['violations', 'passes']
                });
            }).then((results) => {
                // Calculate impact level breakdown for this page
                const impactBreakdown: Record<string, number> = {};
                const impactViolations: Record<string, any[]> = {
                    critical: [],
                    serious: [],
                    moderate: [],
                    minor: [],
                    unknown: []
                };

                results.violations?.forEach((violation: any) => {
                    const impact = violation.impact ?? 'unknown';
                    impactBreakdown[impact] = (impactBreakdown[impact] ?? 0) + 1;
                    impactViolations[impact].push(violation);
                });

                const violationsLength = results.violations?.length ?? 0;
                const passesLength = results.passes?.length ?? 0;
                const violationsArray = results.violations ?? [];
                const passesArray = results.passes ?? [];

                const pageData = {
                    pageName,
                    pageCategory,
                    url,
                    summary: {
                        violations: violationsLength,
                        passes: passesLength,
                        totalRulesChecked: violationsLength + passesLength
                    },
                    impactBreakdown,
                    impactViolations,
                    violations: violationsArray,
                    passes: passesArray
                };

                // Store results for comprehensive report (but without screenshots for sensitive data)
                auditResults.push(pageData);

                cy.log(`🎯 Audited ${pageCategory}: ${pageName} (no screenshots - sensitive data)`);
                cy.log(`✅ ${results.passes.length} passed | ❌ ${results.violations.length} failed`);

                if (results.violations.length > 0) {
                    const impactSummary = Object.entries(impactBreakdown)
                        .map(([impact, count]) => `${impact}:${count}`)
                        .join(', ');
                    cy.log(`📊 Impact: ${impactSummary}`);
                }
            });
        });
    };

    // Main audit test - covers ALL application areas in ONE comprehensive report
    it('should generate single comprehensive accessibility audit covering EVERY page in the application', () => {
        // Clear previous results
        auditResults.length = 0;

        // ============================================================================
        // 1. CORE APPLICATION PAGES
        // ============================================================================
        auditPage('Home Page', 'Core', '/');
        auditPage('Search Page', 'Core', '/search');
        auditPage('Search Results', 'Core', '/search?searchTerm=academy');
        auditPage('Cookies Policy', 'Core', '/cookies');
        auditPage('Accessibility Statement', 'Core', '/accessibility');
        auditPage('Privacy Policy', 'Core', '/privacy');

        // ============================================================================
        // 2. TRUST PAGES - COMPLETE COVERAGE OF ALL SUBPAGES
        // ============================================================================

        // 2.1 Trust Overview subpages (all 3 subpages)
        auditPage('Trust Overview - Details', 'Trusts', `/trusts/overview/trust-details?uid=${testTrustData[1].uid}`);
        auditPage('Trust Overview - Summary', 'Trusts', `/trusts/overview/trust-summary?uid=${testTrustData[1].uid}`);
        auditPage('Trust Overview - Reference Numbers', 'Trusts', `/trusts/overview/reference-numbers?uid=${testTrustData[1].uid}`);

        // 2.2 Trust Contacts (audit functionality but disable screenshots for sensitive personal data)
        auditPageWithoutScreenshots('Trust Contacts - In DfE', 'Trusts', `/trusts/contacts/in-dfe?uid=${testTrustData[0].uid}`);
        auditPageWithoutScreenshots('Trust Contacts - In Trust', 'Trusts', `/trusts/contacts/in-the-trust?uid=${testTrustData[0].uid}`);
        auditPageWithoutScreenshots('Edit Trust Relationship Manager', 'Trusts', `/trusts/contacts/edittrustrelationshipmanager?uid=${testTrustData[0].uid}`);
        auditPageWithoutScreenshots('Edit SFSO Lead', 'Trusts', `/trusts/contacts/editsfsolead?uid=${testTrustData[0].uid}`);

        // 2.3 Trust Governance subpages (all 4 subpages)
        auditPage('Trust Governance - Leadership', 'Trusts', `/trusts/governance/trust-leadership?uid=${testTrustData[1].uid}`);
        auditPage('Trust Governance - Trustees', 'Trusts', `/trusts/governance/trustees?uid=${testTrustData[1].uid}`);
        auditPage('Trust Governance - Members', 'Trusts', `/trusts/governance/members?uid=${testTrustData[1].uid}`);
        auditPage('Trust Governance - Historic Members', 'Trusts', `/trusts/governance/historic-members?uid=${testTrustData[1].uid}`);

        // 2.4 Trust Ofsted subpages (all 4 subpages)
        auditPage('Trust Ofsted - Single Headline Grades', 'Trusts', `/trusts/ofsted/single-headline-grades?uid=${testTrustData[1].uid}`);
        auditPage('Trust Ofsted - Current Ratings', 'Trusts', `/trusts/ofsted/current-ratings?uid=${testTrustData[1].uid}`);
        auditPage('Trust Ofsted - Previous Ratings', 'Trusts', `/trusts/ofsted/previous-ratings?uid=${testTrustData[1].uid}`);
        auditPage('Trust Ofsted - Safeguarding and Concerns', 'Trusts', `/trusts/ofsted/safeguarding-and-concerns?uid=${testTrustData[1].uid}`);

        // 2.5 Trust Academies In-Trust subpages (all 3 subpages)
        auditPage('Trust Academies - In Trust Details', 'Trusts', `/trusts/academies/in-trust/details?uid=${testTrustData[1].uid}`);
        auditPage('Trust Academies - In Trust Pupil Numbers', 'Trusts', `/trusts/academies/in-trust/pupil-numbers?uid=${testTrustData[1].uid}`);
        auditPage('Trust Academies - In Trust Free School Meals', 'Trusts', `/trusts/academies/in-trust/free-school-meals?uid=${testTrustData[1].uid}`);

        // 2.6 Trust Pipeline Academies  subpages (all 3 subpages)
        auditPage('Trust Pipeline Academies - Pre Advisory', 'Trusts', `/trusts/academies/pipeline/pre-advisory-board?uid=${testTrustData[1].uid}`);
        auditPage('Trust Pipeline Academies - Post Advisory', 'Trusts', `/trusts/academies/pipeline/post-advisory-board?uid=${testTrustData[1].uid}`);
        auditPage('Trust Pipeline Academies - Free Schools', 'Trusts', `/trusts/academies/pipeline/free-schools?uid=${testTrustData[1].uid}`);

        // 2.7 Trust Financial Documents (audit functionality but disable screenshots for sensitive financial data)
        // All 4 financial document subpages
        auditPageWithoutScreenshots('Trust Financial Documents - Statements', 'Trusts', `/trusts/financial-documents/financial-statements?uid=${testTrustData[1].uid}`);
        auditPageWithoutScreenshots('Trust Financial Documents - Management Letters', 'Trusts', `/trusts/financial-documents/management-letters?uid=${testTrustData[1].uid}`);
        auditPageWithoutScreenshots('Trust Financial Documents - Internal Scrutiny Reports', 'Trusts', `/trusts/financial-documents/internal-scrutiny-reports?uid=${testTrustData[1].uid}`);
        auditPageWithoutScreenshots('Trust Financial Documents - Self Assessment Checklists', 'Trusts', `/trusts/financial-documents/self-assessment-checklists?uid=${testTrustData[1].uid}`);

        // ============================================================================
        // 3. SCHOOL PAGES - COMPLETE COVERAGE OF ALL SUBPAGES
        // ============================================================================

        // 3.1 School Overview subpages (all 3 subpages)
        auditPage('School Overview - Details', 'Schools', `/schools/overview/details?urn=${testSchoolData[1].urn}`);
        auditPage('School Overview - SEN Provision', 'Schools', `/schools/overview/sen?urn=${testSchoolData[0].urn}`);
        auditPage('School Overview - Federation Details', 'Schools', `/schools/overview/federation?urn=${testFederationData.schoolWithFederationDetails.urn}`);

        // 3.2 School Contacts (audit functionality but disable screenshots for sensitive personal data)
        auditPageWithoutScreenshots('School Contacts - In School', 'Schools', `/schools/contacts/in-the-school?urn=${testSchoolData[1].urn}`);

        // ============================================================================
        // 4. GENERATE COMPREHENSIVE REPORT AFTER ALL PAGES AUDITED
        // ============================================================================
        cy.then(() => {
            const timestamp = new Date().toISOString().replace(/[:.]/g, '-');
            const comprehensiveReport = generateComprehensiveAccessibilityReport(auditResults, timestamp);

            cy.writeFile(`cypress/accessibility/audit-reports/COMPREHENSIVE-AUDIT-REPORT-${timestamp}.html`, comprehensiveReport);

            // Log final summary
            const totalViolations = auditResults.reduce((sum, page) => sum + page.summary.violations, 0);
            const totalPasses = auditResults.reduce((sum, page) => sum + page.summary.passes, 0);
            const totalPages = auditResults.length;

            cy.log(`🎉 COMPREHENSIVE AUDIT COMPLETE`);
            cy.log(`📊 ${totalPages} pages audited in ONE report`);
            cy.log(`✅ ${totalPasses} rules passed across all pages`);
            cy.log(`❌ ${totalViolations} violations found across all pages`);
        });
    });


}); 
