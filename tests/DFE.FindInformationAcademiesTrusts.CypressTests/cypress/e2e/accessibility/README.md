# 🛡️ Accessibility Testing Suite

## 📁 Folder Structure

```text
cypress/e2e/accessibility/
├── comprehensive-accessibility-audit.cy.ts    # 🎯 Complete application coverage
├── components/                                 # 🧩 Component-specific tests
│   ├── data-sources-accessibility.cy.ts       # Data sources components
│   ├── header-search-accessibility.cy.ts      # Header search + autocomplete
│   ├── pagination-accessibility.cy.ts         # Pagination interactions
│   ├── navigation-accessibility.cy.ts         # Navigation components
│   ├── data-download-accessibility.cy.ts      # Download functionality
│   ├── financial-docs-accessibility.cy.ts     # Financial documents + forms
│   └── trust-contacts-accessibility.cy.ts     # Contact forms + interactions
└── README.md
```

## 🎯 Testing Strategy

### **Comprehensive Audit (Main Level)**

- **`comprehensive-accessibility-audit.cy.ts`** - Single test covering **35+ pages** across the entire application
- **Full WCAG 2.1 AA compliance** evaluation
- **Smart data sensitivity handling** (screenshots disabled for personal/financial data)
- **Generates consolidated HTML report** with executive summary and detailed violation analysis
- **Clean pass/fail results** focused on actionable accessibility issues

### **Component Tests (Components Folder)**

- **Focused on interactive components** and user interactions
- **Specific behavior testing** that requires dynamic scenarios
- **Form interactions, autocomplete, pagination, downloads**
- **Complement the comprehensive audit** with detailed component coverage

## 🚀 Running Tests

### Using NPM Scripts (Recommended)

```bash
# All accessibility tests
npm run cy:a11y:run

# Comprehensive audit only (fast, complete coverage)
npm run cy:a11y:audit

# Component tests only (detailed interactions)
npm run cy:a11y:components

# Interactive mode for development
npm run cy:a11y:open              # All tests
npm run cy:a11y:audit:open        # Comprehensive audit only
npm run cy:a11y:components:open   # Component tests only
```

### Using Cypress Directly

```bash
# All accessibility tests
npx cypress run --spec "cypress/e2e/accessibility/**/*.cy.ts"

# Comprehensive audit only
npx cypress run --spec "cypress/e2e/accessibility/comprehensive-accessibility-audit.cy.ts"

# Component tests only
npx cypress run --spec "cypress/e2e/accessibility/components/**/*.cy.ts"
```

## 📊 Coverage

### **Pages Covered (Comprehensive Audit)**

- **Core:** Home, Search Results, Cookies, Accessibility, Privacy
- **Trusts:** All Overview, Governance, Ofsted, Academies, Financial Docs subpages
- **Schools:** All Overview subpages, Federation details
- **Contacts:** Trust and School contacts (audited without screenshots)

### **Components Covered (Component Tests)**

- **Interactive Elements:** Search, autocomplete, pagination, forms
- **Data Handling:** Downloads, financial documents, contact forms
- **Navigation:** Header search, data sources, navigation components

## 🔧 Data Sensitivity

- **✅ Screenshots Enabled:** General pages, SEN, governance, Ofsted, academies
- **🚫 Screenshots Disabled:** Trust contacts, school contacts, financial documents
- **All pages audited** for accessibility violations regardless of screenshot settings

## 📈 Benefits

- **Single comprehensive report** for complete application overview
- **46% reduction** in test file count while maintaining 100% coverage
- **Clear separation** between page-level and component-level testing
- **Efficient CI/CD execution** with focused component testing
- **Maintainable structure** with logical organization
