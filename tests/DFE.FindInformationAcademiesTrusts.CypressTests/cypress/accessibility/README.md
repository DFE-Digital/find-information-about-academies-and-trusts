# 🛡️ Accessibility Testing & Audit Reports

This directory contains accessibility testing reports and audit documentation for the Find Information about Academies and Trusts application.

## 📊 **Two Types of Accessibility Testing**

### 1. **🔍 Component Accessibility Tests**

- **Location**: `cypress/e2e/accessibility/components/`
- **Generated by**: Individual component accessibility tests
- **Failure behavior**: ❌ **Fails on critical & serious violations**, ⚠️ warns on moderate & minor
- **Content**: Focused testing of specific UI components and pages
- **Use case**: Day-to-day development, targeted accessibility testing

**Test files:**

- `data-download-accessibility.cy.ts`
- `data-sources-accessibility.cy.ts`
- `financial-docs-accessibility.cy.ts`
- `header-search-accessibility.cy.ts`
- `navigation-accessibility.cy.ts`
- `pagination-accessibility.cy.ts`
- `trust-contacts-accessibility.cy.ts`

### 2. **📋 Comprehensive Accessibility Audit**

- **Location**: `cypress/e2e/accessibility/comprehensive-accessibility-audit.cy.ts`
- **Generated by**: Single comprehensive audit test
- **Failure behavior**: ❌ **Fails on critical & serious violations**, ⚠️ warns on moderate & minor
- **Content**: Tests **ALL 35+ pages** across the entire application in one audit
- **Reports**: `/cypress/accessibility/audit-reports/COMPREHENSIVE-AUDIT-REPORT-*.html`
- **Use case**: Compliance audits, comprehensive documentation, stakeholder reports

**Pages covered:**

- **Core pages**: Home, search, cookies, accessibility, privacy
- **Trust pages**: All overview, contacts, governance, ofsted, academies, financial subpages  
- **School pages**: All overview, contacts, SEN, federation subpages

## 🛡️ **Accessibility Governance**

### **Consistent Failure Criteria Across All Tests**

Both component tests and comprehensive audit now enforce the same standards:

- ✅ **Tests PASS** with moderate/minor violations (logged as warnings)
- ❌ **Tests FAIL** with critical/serious violations (blocks deployment)

This ensures consistent accessibility governance across all testing approaches.

### **Impact Level Priority**

1. **Critical** 🔴 - Blocks CI/CD, fix immediately
2. **Serious** 🟠 - Blocks CI/CD, fix in current sprint  
3. **Moderate** 🟡 - Logged but doesn't block, fix in next release
4. **Minor** 🔵 - Logged but doesn't block, fix when convenient

## 🚀 **How to Run Tests**

### Component Accessibility Tests

```bash
# Run all component accessibility tests
npm run cy:a11y:components

# Run specific component test
npx cypress run --spec "cypress/e2e/accessibility/components/navigation-accessibility.cy.ts"

# Open interactive mode for component tests
npm run cy:a11y:components:open
```

### Comprehensive Accessibility Audit

```bash
# Run comprehensive audit (all 35+ pages)
npm run cy:a11y:audit

# Open interactive mode for comprehensive audit
npm run cy:a11y:audit:open
```

### All Accessibility Tests

```bash
# Run ALL accessibility tests (components + comprehensive audit)
npm run cy:a11y:run
```

## 📈 **CI/CD Integration**

The GitHub Actions workflow includes accessibility testing in the matrix:

```yaml
strategy:
  matrix:
    include:
      - group: ui-trusts
        spec: cypress/e2e/regression/trusts/**/*.cy.ts
      - group: ui-schools-and-general  
        spec: cypress/e2e/regression/schools/**/*.cy.ts, cypress/e2e/regression/*.cy.ts
      - group: a11y-comprehensive
        spec: cypress/e2e/accessibility/comprehensive-accessibility-audit.cy.ts
      - group: a11y-components
        spec: cypress/e2e/accessibility/components/**/*.cy.ts
```

### **Accessibility as Quality Gate**

- 🚫 **Critical/Serious violations** = CI/CD pipeline fails
- ⚠️ **Moderate/Minor violations** = CI/CD continues with warnings
- 📊 **Comprehensive reports** = Always generated for audit trail

## 🏗️ **Architecture & Implementation**

### **Helper Classes (Maintainability)**

The comprehensive audit uses clean helper classes for maintainability:

- **`ComprehensiveAuditHelper`**: Handles axe.run() logic, failure checking, result processing
- **`AuditPageDefinitions`**: Contains all page URLs organized by category (core, trusts, schools)

### **Adding New Pages to Audit**

To add new pages to the comprehensive audit:

1. Add to appropriate method in `audit-page-definitions.ts`:

   ```typescript
   // For regular pages
   this.auditHelper.auditPage('Page Name', 'Category', '/url');
   
   // For sensitive data pages (no screenshots)
   this.auditHelper.auditPageWithoutScreenshots('Page Name', 'Category', '/url');
   ```

### **Sensitive Data Handling**

Pages with sensitive data (contacts, financial) use `auditPageWithoutScreenshots()`:

- ✅ **Accessibility testing** still happens
- ❌ **Screenshots disabled** to protect sensitive information
- 📋 **Results included** in comprehensive report

## 📊 **Comprehensive Audit Reports**

### **Report Structure**

The comprehensive audit generates a single HTML report covering all pages:

- **Executive Summary**: Overall statistics and impact breakdown
- **Detailed Results**: Page-by-page findings organized by category
- **Violation Details**: Specific accessibility issues with remediation guidance

### **Report Artifacts in CI/CD**

Reports are automatically uploaded as GitHub Actions artifacts:

- **Screenshots**: Uploaded on test failures
- **Reports**: Uploaded on test failures  
- **Accessibility Reports**: Always uploaded (regardless of pass/fail)

## 🎯 **Best Practices**

### **For Development Teams**

- ✅ Run component tests during development for quick feedback
- ✅ Use comprehensive audit for pre-release validation
- ✅ Fix critical/serious violations immediately (they block CI/CD)
- ⚠️ Address moderate/minor violations in upcoming sprints

### **For Compliance Teams**

- 📋 Generate comprehensive audit reports monthly/quarterly
- 📊 Use reports for stakeholder presentations and compliance documentation
- 📈 Track accessibility improvements over time

### **For QA Teams**

- 🔍 Use component tests for targeted testing during feature development
- 📋 Run comprehensive audit before major releases
- 🔄 Maintain historical audit trail for regression detection

## 🔧 **Configuration**

### **Component Tests Configuration**

Component tests use wick-a11y with consistent failure criteria:

```typescript
cy.checkAccessibility(null, {
    generateReport: false,  // No screenshots for sensitive data
    includedImpacts: ['critical', 'serious'],  // These fail the test
    onlyWarnImpacts: ['moderate', 'minor']     // These only warn
});
```

### **Comprehensive Audit Configuration**

The comprehensive audit is configured in the helper classes and can be customized by:

- Modifying page lists in `AuditPageDefinitions`
- Adjusting failure criteria in `ComprehensiveAuditHelper`
- Adding new page categories as needed

## 🔗 **Related Documentation**

- [WCAG 2.1 Guidelines](https://www.w3.org/WAI/WCAG21/quickref/)
- [axe-core Rules](https://github.com/dequelabs/axe-core/blob/master/doc/rule-descriptions.md)
- [wick-a11y Documentation](https://github.com/sclavijosuero/wick-a11y)
- [Cypress Accessibility Testing](https://docs.cypress.io/guides/accessibility-testing)

---
