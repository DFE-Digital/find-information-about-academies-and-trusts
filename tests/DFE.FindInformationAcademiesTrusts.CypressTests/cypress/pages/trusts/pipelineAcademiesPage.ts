import { TableUtility } from "../tableUtility";

class PipelineAcademies {

    elements = {
        preAdvisory: {
            section: () => cy.get('[data-testid="pre-advisory-board-table"]'),
            schoolName: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-school-name"]'),
            schoolNameHeader: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-school-name-header"]'),
            urn: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-URN"]'),
            urnHeader: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-URN-header"]'),
            ageRange: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-age-range"]'),
            ageRangeHeader: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-age-range-header"]'),
            localAuthority: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-local-authority"]'),
            localAuthorityHeader: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-local-authority-header"]'),
            projectType: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-project-type"]'),
            projectTypeHeader: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-project-type-header"]'),
            proposedConversionTransferDate: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-conversion-transfer-date"]'),
            proposedConversionTransferDateHeader: () => this.elements.preAdvisory.section().find('[data-testid="pre-advisory-board-conversion-transfer-date-header"]'),
        },
        postAdvisory: {
            section: () => cy.get('[data-testid="post-advisory-board-table"]'),
            schoolName: () => this.elements.postAdvisory.section().find('[data-testid="post-advisory-board-school-name"]'),
            schoolNameHeader: () => this.elements.postAdvisory.section().find('[data-testid="post-advisory-board-school-name-header"]'),
            urn: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-URN"]'),
            urnHeader: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-URN-header"]'),
            ageRange: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-age-range"]'),
            ageRangeHeader: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-age-range-header"]'),
            localAuthority: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-local-authority"]'),
            localAuthorityHeader: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-local-authority-header"]'),
            projectType: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-project-type"]'),
            projectTypeHeader: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-project-type-header"]'),
            proposedConversionTransferDate: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-conversion-transfer-date"]'),
            proposedConversionTransferDateHeader: () => this.elements.postAdvisory.section().find('[data-testid="pre-advisory-board-conversion-transfer-date-header"]'),

        },
        freeSchools: {
            section: () => cy.get('[data-testid="free-schools-table"]'),
            schoolName: () => this.elements.freeSchools.section().find('[data-testid="free-schools-school-name"]'),
            schoolNameHeader: () => this.elements.freeSchools.section().find('[data-testid="free-schools-school-name-header"]'),
            urn: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-URN"]'),
            urnHeader: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-URN-header"]'),
            ageRange: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-age-range"]'),
            ageRangeHeader: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-age-range-header"]'),
            localAuthority: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-local-authority"]'),
            localAuthorityHeader: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-local-authority-header"]'),
            projectType: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-project-type"]'),
            projectTypeHeader: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-project-type-header"]'),
            proposedConversionTransferDate: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-conversion-transfer-date"]'),
            proposedConversionTransferDateHeader: () => this.elements.freeSchools.section().find('[data-testid="pre-advisory-board-conversion-transfer-date-header"]'),
        },

    };

}

const pipelineAcademiesPage = new PipelineAcademies();
export default pipelineAcademiesPage;

